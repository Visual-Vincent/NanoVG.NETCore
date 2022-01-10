using System;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace NanoVG.Test
{
    class TestWindow : GameWindow
    {
        public TestWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        NVGcontext ctx;

        protected override void OnLoad()
        {
            base.OnLoad();
            ctx = NVG.CreateGL3Context((int)(NVGcreateFlags.NVG_STENCIL_STROKES | NVGcreateFlags.NVG_ANTIALIAS));
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            ctx.BeginFrame(ClientSize.X, ClientSize.Y, 96.0f);
            OnRenderUI(ctx);
            ctx.EndFrame();

            SwapBuffers();
        }

        protected virtual void OnRenderUI(NVGcontext ctx)
        {
            ctx.BeginPath();

            ctx.RoundedRect(8, 8, 300, 300, 16);
            ctx.FillColor(new NVGcolor() { r = 255, g = 0, b = 0, a = 255 });
            ctx.Fill();

            ctx.Restore();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
        }
    }
}
