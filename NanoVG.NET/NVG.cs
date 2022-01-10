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

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NVGcolor
    {
        public float r,g,b,a;
    }
}
