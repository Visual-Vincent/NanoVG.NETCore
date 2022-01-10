using System;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace NanoVG.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings() {
                APIVersion = new Version(3, 3),
                Size = new Vector2i(800, 600),
                Title = "NanoVG.NET Test",
                NumberOfSamples = 8, // 8x MSAA,
                Flags = RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? ContextFlags.ForwardCompatible : ContextFlags.Default
            };

            var gameWindowSettings = new GameWindowSettings();

            using(var window = new TestWindow(gameWindowSettings, nativeWindowSettings))
            {
                window.CenterWindow();
                window.VSync = VSyncMode.On;
                window.Run();
            }
        }
    }
}
