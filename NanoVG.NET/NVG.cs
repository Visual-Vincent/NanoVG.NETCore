using System;
using System.Runtime.InteropServices;

namespace NanoVG
{
    public static unsafe partial class NVG
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

    public enum NVGcreateFlags
    {
        NVG_ANTIALIAS       = 1<<0,
        NVG_STENCIL_STROKES = 1<<1,
        NVG_DEBUG           = 1<<2,
    }
}
