using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace NanoVG
{
    public static partial class NVG
    {
        static NVG()
        {
            NativeLibrary.SetDllImportResolver(typeof(NVG).Assembly, (name, assembly, searchPath) =>
            {
                if(name != LibraryName)
                    return IntPtr.Zero;

                if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return NativeLibrary.Load($"{LibraryName}.dll", assembly, searchPath);
                }
                else if(RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return NativeLibrary.Load($"{LibraryName}.so", assembly, searchPath);
                }
                else if(RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return NativeLibrary.Load($"{LibraryName}.dylib", assembly, searchPath);
                }

                // Unsupported platform
                return IntPtr.Zero;
            });
        }
    }

    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct NVGcontext
    {
        [FieldOffset(0)]
        public IntPtr Handle;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NVGcolor
    {
        public float r,g,b,a;
    }

    public class TextRow
    {
        public string SourceText { get; }
        public string Text { get; }
        public int NextLinePosition { get; }
        public float MaxX { get; }
        public float MinX { get; }
        public float Width { get; }

        internal TextRow(string sourceText, string text, int nextLinePos, float minX, float maxX, float width)
        {
            SourceText = sourceText;
            Text = text;
            NextLinePosition = nextLinePos;
            MinX = minX;
            MaxX = maxX;
            Width = width;
        }
    }

    public enum NVGcreateFlags
    {
        NVG_ANTIALIAS       = 1<<0,
        NVG_STENCIL_STROKES = 1<<1,
        NVG_DEBUG           = 1<<2,
    }

    public static partial class NVG
    {
        private static T GetStringPointers<T>(string str, int length, Func<IntPtr, IntPtr, T> callback)
        {
            if(length > str.Length)
                throw new ArgumentOutOfRangeException(nameof(length));

            byte[] utf8 = Encoding.UTF8.GetBytes(str);
            int byteLength = length > 0 ? Encoding.UTF8.GetByteCount(str, 0, length) : 0;

            GCHandle handle = default;
            try
            {
                handle = GCHandle.Alloc(utf8, GCHandleType.Pinned);

                IntPtr strPtr = handle.AddrOfPinnedObject();
                IntPtr endPtr = length > 0 ? endPtr = IntPtr.Add(strPtr, byteLength) : IntPtr.Zero;

                return callback(strPtr, endPtr);
            }
            finally
            {
                if(handle.IsAllocated)
                    handle.Free();
            }
        }
        
        /// <summary>
        /// Draws text string at specified location.
        /// </summary>
        public static float Text(this NVGcontext ctx, float x, float y, string @string, int length = -1)
        {
            return GetStringPointers(@string, length, (strPtr, endPtr) => Text(ctx, x, y, strPtr, endPtr));
        }

        /// <summary>
        /// Draws multi-line text string at specified location wrapped at the specified width. <br/>
        /// White space is stripped at the beginning of the rows, the text is split at word boundaries or when new-line characters are encountered. <br/>
        /// Words longer than the max width are slit at nearest character (i.e. no hyphenation).
        /// </summary>
        public static void TextBox(this NVGcontext ctx, float x, float y, float breakRowWidth, string @string, int length = -1)
        {
            GetStringPointers(@string, length, (strPtr, endPtr) => {
                TextBox(ctx, x, y, breakRowWidth, strPtr, endPtr);
                return (object)null;
            });
        }

        /// <summary>
        /// Measures the specified text string. Parameter bounds should be a pointer to float[4], <br/>
        /// if the bounding box of the text should be returned. The bounds value are [xmin,ymin, xmax,ymax] <br/>
        /// Returns the horizontal advance of the measured text (i.e. where the next character should drawn). <br/>
        /// Measured values are returned in local coordinate space.
        /// </summary>
        public static float TextBounds(this NVGcontext ctx, float x, float y, string @string, out float[] bounds, int strLength = -1)
        {
            float[] retBounds = new float[4];
            float retVal = GetStringPointers(@string, strLength, (strPtr, endPtr) => TextBounds(ctx, x, y, strPtr, endPtr, retBounds));

            bounds = retBounds;
            return retVal;
        }

        /// <summary>
        /// Measures the specified multi-text string. Parameter bounds should be a pointer to float[4], <br/>
        /// if the bounding box of the text should be returned. The bounds value are [xmin,ymin, xmax,ymax] <br/>
        /// Measured values are returned in local coordinate space.
        /// </summary>
        public static void TextBoxBounds(this NVGcontext ctx, float x, float y, float breakRowWidth, string @string, out float[] bounds, int strLength = -1)
        {
            float[] retBounds = new float[4];

            GetStringPointers(@string, strLength, (strPtr, endPtr) => {
                TextBoxBounds(ctx, x, y, breakRowWidth, strPtr, endPtr, retBounds);
                return (object)null;
            });

            bounds = retBounds;
        }

        /// <summary>
        /// Calculates the glyph x positions of the specified text. Measured values are returned in local coordinate space.
        /// </summary>
        public static NVGglyphPosition[] TextGlyphPositions(this NVGcontext ctx, float x, float y, string @string, int maxPositions, int strLength = -1)
        {
            if(maxPositions <= 0)
                return Array.Empty<NVGglyphPosition>();

            NVGglyphPosition[] positions = new NVGglyphPosition[maxPositions];
            int posCount = GetStringPointers(@string, strLength, (strPtr, endPtr) => {
                unsafe
                {
                    fixed(NVGglyphPosition* ptr = &positions[0])
                    {
                        return TextGlyphPositions(ctx, x, y, strPtr, endPtr, new IntPtr(ptr), maxPositions);
                    }
                }
            });

            if(posCount <= 0)
                return Array.Empty<NVGglyphPosition>();

            if(posCount != positions.Length)
                Array.Resize(ref positions, posCount);

            return positions;
        }

        /// <summary>
        /// Breaks the specified text into lines. <br/>
        /// White space is stripped at the beginning of the rows, the text is split at word boundaries or when new-line characters are encountered. <br/>
        /// Words longer than the max width are slit at nearest character (i.e. no hyphenation).
        /// </summary>
        public static TextRow[] TextBreakLines(this NVGcontext ctx, string @string, float breakRowWidth, int maxRows, int strLength = -1)
        {
            if(@string.Length <= 0)
                return Array.Empty<TextRow>();

            NVGtextRow[] _rows = new NVGtextRow[maxRows];
            byte[] str = Encoding.UTF8.GetBytes(@string);
            long startOffset = 0;

            int rowCount = GetStringPointers(@string, strLength, (strPtr, endPtr) => {
                unsafe
                {
                    fixed(NVGtextRow* ptr = &_rows[0])
                    {
                        int rowCount = TextBreakLines(ctx, strPtr, endPtr, breakRowWidth, new IntPtr(ptr), maxRows);
                        startOffset = strPtr.ToInt64();
                        return rowCount;
                    }
                }
            });

            if(rowCount <= 0)
                return Array.Empty<TextRow>();

            List<TextRow> rows = new List<TextRow>();

            for(int i = 0; i < rowCount; i++)
            {
                NVGtextRow row = _rows[i];
                int start = (int)(row.start.ToInt64() - startOffset);
                int length = (int)(row.end.ToInt64() - row.start.ToInt64());
                int nextLine = (int)(row.next.ToInt64() - startOffset);

                // Convert from byte pos to char pos
                nextLine = Encoding.UTF8.GetCharCount(str, 0, nextLine);

                rows.Add(new TextRow(@string, Encoding.UTF8.GetString(str, start, length), nextLine, row.minx, row.maxx, row.width));
            }

            return rows.ToArray();
        }
    }
}
