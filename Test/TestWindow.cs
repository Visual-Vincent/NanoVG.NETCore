using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace NanoVG.Test
{
    class TestWindow : GameWindow
    {
        public TestWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        NVGcontext vg;
        DemoData demoData;

        float pixelRatio;
        bool blowup = false;

        protected override void OnLoad()
        {
            base.OnLoad();
#if DEMO_MSAA
            vg = NVG.CreateGL3Context((int)(NVGcreateFlags.NVG_STENCIL_STROKES));
#else
            vg = NVG.CreateGL3Context((int)(NVGcreateFlags.NVG_ANTIALIAS | NVGcreateFlags.NVG_STENCIL_STROKES));
#endif
            demoData = NVGDemo.loadDemoData(vg);

            if(demoData == null)
                Environment.Exit(1);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch(e.Key)
            {
                case Keys.Escape:
                    Close();
                    break;

                case Keys.Space:
                    blowup = !blowup;
                    break;
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);

            if(e.Width >= e.Height)
                pixelRatio = ClientSize.X / ClientSize.Y;
            else
                pixelRatio = ClientSize.Y / ClientSize.X;

        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            if(WindowState == WindowState.Minimized)
                return;

            GL.ClearColor(0.3f, 0.3f, 0.3f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            vg.BeginFrame(ClientSize.X, ClientSize.Y, pixelRatio);

            DrawFps(vg, e);
            NVGDemo.renderDemo(vg, MouseState.X, MouseState.Y, ClientSize.X, ClientSize.Y, (float)GLFW.GetTime(), blowup, demoData);

            vg.EndFrame();

            SwapBuffers();
        }

        private void DrawFps(NVGcontext vg, FrameEventArgs e)
        {
            int fps = (int)Math.Round(1.0 / e.Time);
            vg.Save();

            vg.FontSize(28.0f);
            vg.FontFace("sans");
            vg.FillColor(NVG.RGBA(255, 255, 255, 255));
            vg.TextAlign((int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_TOP));
            vg.Text(4.0f, 4.0f, $"{fps} FPS");

            vg.Restore();
        }
    }
}
