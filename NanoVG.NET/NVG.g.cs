// 
// AUTO-GENERATED CODE
// 

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace NanoVG
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NVGpaint
    {
        public fixed float xform[6];
        public fixed float extent[2];
        public float radius;
        public float feather;
        public NVGcolor innerColor;
        public NVGcolor outerColor;
        public int image;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NVGcompositeOperationState
    {
        public int srcRGB;
        public int dstRGB;
        public int srcAlpha;
        public int dstAlpha;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NVGglyphPosition
    {
        public IntPtr str;
        public float x;
        public float minx, maxx;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NVGtextRow
    {
        public IntPtr start;
        public IntPtr end;
        public IntPtr next;
        public float width;
        public float minx, maxx;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NVGscissor
    {
        public fixed float xform[6];
        public fixed float extent[2];
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct NVGvertex
    {
        public float x,y,u,v;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NVGpath
    {
        public int first;
        public int count;
        public byte closed;
        public int nbevel;
        public NVGvertex* fill;
        public int nfill;
        public NVGvertex* stroke;
        public int nstroke;
        public int winding;
        public int convex;
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public unsafe struct NVGparams
    {
        public void* userPtr;
        public int edgeAntiAlias;
    }
    
    public enum NVGwinding
    {
        NVG_CCW = 1,
        NVG_CW  = 2,
    }
    
    public enum NVGsolidity
    {
        NVG_SOLID = 1,
        NVG_HOLE  = 2,
    }
    
    public enum NVGlineCap
    {
        NVG_BUTT,
        NVG_ROUND,
        NVG_SQUARE,
        NVG_BEVEL,
        NVG_MITER,
    }
    
    public enum NVGalign
    {
        NVG_ALIGN_LEFT     = 1<<0,
        NVG_ALIGN_CENTER   = 1<<1,
        NVG_ALIGN_RIGHT    = 1<<2,
        NVG_ALIGN_TOP      = 1<<3,
        NVG_ALIGN_MIDDLE   = 1<<4,
        NVG_ALIGN_BOTTOM   = 1<<5,
        NVG_ALIGN_BASELINE = 1<<6,
    }
    
    public enum NVGblendFactor
    {
        NVG_ZERO                = 1<<0,
        NVG_ONE                 = 1<<1,
        NVG_SRC_COLOR           = 1<<2,
        NVG_ONE_MINUS_SRC_COLOR = 1<<3,
        NVG_DST_COLOR           = 1<<4,
        NVG_ONE_MINUS_DST_COLOR = 1<<5,
        NVG_SRC_ALPHA           = 1<<6,
        NVG_ONE_MINUS_SRC_ALPHA = 1<<7,
        NVG_DST_ALPHA           = 1<<8,
        NVG_ONE_MINUS_DST_ALPHA = 1<<9,
        NVG_SRC_ALPHA_SATURATE  = 1<<10,
    }
    
    public enum NVGcompositeOperation
    {
        NVG_SOURCE_OVER,
        NVG_SOURCE_IN,
        NVG_SOURCE_OUT,
        NVG_ATOP,
        NVG_DESTINATION_OVER,
        NVG_DESTINATION_IN,
        NVG_DESTINATION_OUT,
        NVG_DESTINATION_ATOP,
        NVG_LIGHTER,
        NVG_COPY,
        NVG_XOR,
    }
    
    public enum NVGimageFlags
    {
        NVG_IMAGE_GENERATE_MIPMAPS = 1<<0,
        NVG_IMAGE_REPEATX          = 1<<1,
        NVG_IMAGE_REPEATY          = 1<<2,
        NVG_IMAGE_FLIPY            = 1<<3,
        NVG_IMAGE_PREMULTIPLIED    = 1<<4,
        NVG_IMAGE_NEAREST          = 1<<5,
    }
    
    public enum NVGtexture
    {
        NVG_TEXTURE_ALPHA = 0x01,
        NVG_TEXTURE_RGBA  = 0x02,
    }
    
    public static partial class NVG
    {
        public const string LibraryName = "NanoVG";
        public const string FunctionPrefix = "nvg";
        
        /// <summary>
        /// Begin drawing a new frame <br/>
        /// Calls to nanovg drawing API should be wrapped in nvgBeginFrame() &amp; nvgEndFrame() <br/>
        /// nvgBeginFrame() defines the size of the window to render to in relation currently <br/>
        /// set viewport (i.e. glViewport on GL backends). Device pixel ration allows to <br/>
        /// control the rendering on Hi-DPI devices. <br/>
        /// For example, GLFW returns two dimension for an opened window: window size and <br/>
        /// frame buffer size. In that case you would set windowWidth/Height to the window size <br/>
        /// devicePixelRatio to: frameBufferWidth / windowWidth.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(BeginFrame))]
        public static extern void BeginFrame(this NVGcontext ctx, float windowWidth, float windowHeight, float devicePixelRatio);
        
        /// <summary>
        /// Cancels drawing the current frame.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(CancelFrame))]
        public static extern void CancelFrame(this NVGcontext ctx);
        
        /// <summary>
        /// Ends drawing flushing remaining render state.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(EndFrame))]
        public static extern void EndFrame(this NVGcontext ctx);
        
        /// <summary>
        /// Sets the composite operation. The op parameter should be one of NVGcompositeOperation.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(GlobalCompositeOperation))]
        public static extern void GlobalCompositeOperation(this NVGcontext ctx, int op);
        
        /// <summary>
        /// Sets the composite operation with custom pixel arithmetic. The parameters should be one of NVGblendFactor.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(GlobalCompositeBlendFunc))]
        public static extern void GlobalCompositeBlendFunc(this NVGcontext ctx, int sfactor, int dfactor);
        
        /// <summary>
        /// Sets the composite operation with custom pixel arithmetic for RGB and alpha components separately. The parameters should be one of NVGblendFactor.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(GlobalCompositeBlendFuncSeparate))]
        public static extern void GlobalCompositeBlendFuncSeparate(this NVGcontext ctx, int srcRGB, int dstRGB, int srcAlpha, int dstAlpha);
        
        /// <summary>
        /// Returns a color value from red, green, blue values. Alpha will be set to 255 (1.0f).
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(RGB))]
        public static extern NVGcolor RGB(byte r, byte g, byte b);
        
        /// <summary>
        /// Returns a color value from red, green, blue values. Alpha will be set to 1.0f.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(RGBf))]
        public static extern NVGcolor RGBf(float r, float g, float b);
        
        /// <summary>
        /// Returns a color value from red, green, blue and alpha values.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(RGBA))]
        public static extern NVGcolor RGBA(byte r, byte g, byte b, byte a);
        
        /// <summary>
        /// Returns a color value from red, green, blue and alpha values.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(RGBAf))]
        public static extern NVGcolor RGBAf(float r, float g, float b, float a);
        
        /// <summary>
        /// Linearly interpolates from color c0 to c1, and returns resulting color value.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(LerpRGBA))]
        public static extern NVGcolor LerpRGBA(NVGcolor c0, NVGcolor c1, float u);
        
        /// <summary>
        /// Sets transparency of a color value.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TransRGBA))]
        public static extern NVGcolor TransRGBA(NVGcolor c0, byte a);
        
        /// <summary>
        /// Sets transparency of a color value.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TransRGBAf))]
        public static extern NVGcolor TransRGBAf(NVGcolor c0, float a);
        
        /// <summary>
        /// Returns color value specified by hue, saturation and lightness. <br/>
        /// HSL values are all in range [0..1], alpha will be set to 255.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(HSL))]
        public static extern NVGcolor HSL(float h, float s, float l);
        
        /// <summary>
        /// Returns color value specified by hue, saturation and lightness and alpha. <br/>
        /// HSL values are all in range [0..1], alpha in range [0..255]
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(HSLA))]
        public static extern NVGcolor HSLA(float h, float s, float l, byte a);
        
        /// <summary>
        /// Pushes and saves the current render state into a state stack. <br/>
        /// A matching nvgRestore() must be used to restore the state.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(Save))]
        public static extern void Save(this NVGcontext ctx);
        
        /// <summary>
        /// Pops and restores current render state.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(Restore))]
        public static extern void Restore(this NVGcontext ctx);
        
        /// <summary>
        /// Resets current render state to default values. Does not affect the render state stack.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(Reset))]
        public static extern void Reset(this NVGcontext ctx);
        
        /// <summary>
        /// Sets whether to draw antialias for nvgStroke() and nvgFill(). It&apos;s enabled by default.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(ShapeAntiAlias))]
        public static extern void ShapeAntiAlias(this NVGcontext ctx, int enabled);
        
        /// <summary>
        /// Sets current stroke style to a solid color.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(StrokeColor))]
        public static extern void StrokeColor(this NVGcontext ctx, NVGcolor color);
        
        /// <summary>
        /// Sets current stroke style to a paint, which can be a one of the gradients or a pattern.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(StrokePaint))]
        public static extern void StrokePaint(this NVGcontext ctx, NVGpaint paint);
        
        /// <summary>
        /// Sets current fill style to a solid color.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(FillColor))]
        public static extern void FillColor(this NVGcontext ctx, NVGcolor color);
        
        /// <summary>
        /// Sets current fill style to a paint, which can be a one of the gradients or a pattern.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(FillPaint))]
        public static extern void FillPaint(this NVGcontext ctx, NVGpaint paint);
        
        /// <summary>
        /// Sets the miter limit of the stroke style. <br/>
        /// Miter limit controls when a sharp corner is beveled.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(MiterLimit))]
        public static extern void MiterLimit(this NVGcontext ctx, float limit);
        
        /// <summary>
        /// Sets the stroke width of the stroke style.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(StrokeWidth))]
        public static extern void StrokeWidth(this NVGcontext ctx, float size);
        
        /// <summary>
        /// Sets how the end of the line (cap) is drawn, <br/>
        /// Can be one of: NVG_BUTT (default), NVG_ROUND, NVG_SQUARE.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(LineCap))]
        public static extern void LineCap(this NVGcontext ctx, int cap);
        
        /// <summary>
        /// Sets how sharp path corners are drawn. <br/>
        /// Can be one of NVG_MITER (default), NVG_ROUND, NVG_BEVEL.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(LineJoin))]
        public static extern void LineJoin(this NVGcontext ctx, int join);
        
        /// <summary>
        /// Sets the transparency applied to all rendered shapes. <br/>
        /// Already transparent paths will get proportionally more transparent as well.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(GlobalAlpha))]
        public static extern void GlobalAlpha(this NVGcontext ctx, float alpha);
        
        /// <summary>
        /// Resets current transform to a identity matrix.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(ResetTransform))]
        public static extern void ResetTransform(this NVGcontext ctx);
        
        /// <summary>
        /// Premultiplies current coordinate system by specified matrix. <br/>
        /// The parameters are interpreted as matrix as follows: <br/>
        /// [a c e] <br/>
        /// [b d f] <br/>
        /// [0 0 1]
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(Transform))]
        public static extern void Transform(this NVGcontext ctx, float a, float b, float c, float d, float e, float f);
        
        /// <summary>
        /// Translates current coordinate system.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(Translate))]
        public static extern void Translate(this NVGcontext ctx, float x, float y);
        
        /// <summary>
        /// Rotates current coordinate system. Angle is specified in radians.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(Rotate))]
        public static extern void Rotate(this NVGcontext ctx, float angle);
        
        /// <summary>
        /// Skews the current coordinate system along X axis. Angle is specified in radians.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(SkewX))]
        public static extern void SkewX(this NVGcontext ctx, float angle);
        
        /// <summary>
        /// Skews the current coordinate system along Y axis. Angle is specified in radians.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(SkewY))]
        public static extern void SkewY(this NVGcontext ctx, float angle);
        
        /// <summary>
        /// Scales the current coordinate system.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(Scale))]
        public static extern void Scale(this NVGcontext ctx, float x, float y);
        
        /// <summary>
        /// Stores the top part (a-f) of the current transformation matrix in to the specified buffer. <br/>
        /// [a c e] <br/>
        /// [b d f] <br/>
        /// [0 0 1] <br/>
        /// There should be space for 6 floats in the return buffer for the values a-f.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(CurrentTransform))]
        public static extern void CurrentTransform(this NVGcontext ctx, float[] xform);
        
        /// <summary>
        /// Sets the transform to identity matrix.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TransformIdentity))]
        public static extern void TransformIdentity(float[] dst);
        
        /// <summary>
        /// Sets the transform to translation matrix matrix.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TransformTranslate))]
        public static extern void TransformTranslate(float[] dst, float tx, float ty);
        
        /// <summary>
        /// Sets the transform to scale matrix.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TransformScale))]
        public static extern void TransformScale(float[] dst, float sx, float sy);
        
        /// <summary>
        /// Sets the transform to rotate matrix. Angle is specified in radians.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TransformRotate))]
        public static extern void TransformRotate(float[] dst, float a);
        
        /// <summary>
        /// Sets the transform to skew-x matrix. Angle is specified in radians.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TransformSkewX))]
        public static extern void TransformSkewX(float[] dst, float a);
        
        /// <summary>
        /// Sets the transform to skew-y matrix. Angle is specified in radians.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TransformSkewY))]
        public static extern void TransformSkewY(float[] dst, float a);
        
        /// <summary>
        /// Sets the transform to the result of multiplication of two transforms, of A = A*B.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TransformMultiply))]
        public static extern void TransformMultiply(float[] dst, float[] src);
        
        /// <summary>
        /// Sets the transform to the result of multiplication of two transforms, of A = B*A.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TransformPremultiply))]
        public static extern void TransformPremultiply(float[] dst, float[] src);
        
        /// <summary>
        /// Sets the destination to inverse of specified transform. <br/>
        /// Returns 1 if the inverse could be calculated, else 0.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TransformInverse))]
        public static extern int TransformInverse(float[] dst, float[] src);
        
        /// <summary>
        /// Transform a point by given transform.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TransformPoint))]
        public static extern void TransformPoint(float[] dstx, float[] dsty, float[] xform, float srcx, float srcy);
        
        /// <summary>
        /// Converts degrees to radians and vice versa.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(DegToRad))]
        public static extern float DegToRad(float deg);
        
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(RadToDeg))]
        public static extern float RadToDeg(float rad);
        
        /// <summary>
        /// Creates image by loading it from the disk from specified file name. <br/>
        /// Returns handle to the image.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(CreateImage))]
        public static extern int CreateImage(this NVGcontext ctx, [MarshalAs(UnmanagedType.LPUTF8Str)] string filename, int imageFlags);
        
        /// <summary>
        /// Creates image by loading it from the specified chunk of memory. <br/>
        /// Returns handle to the image.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(CreateImageMem))]
        public static extern int CreateImageMem(this NVGcontext ctx, int imageFlags, byte[] data, int ndata);
        
        /// <summary>
        /// Creates image from specified image data. <br/>
        /// Returns handle to the image.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(CreateImageRGBA))]
        public static extern int CreateImageRGBA(this NVGcontext ctx, int w, int h, int imageFlags, byte[] data);
        
        /// <summary>
        /// Updates image data specified by image handle.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(UpdateImage))]
        public static extern void UpdateImage(this NVGcontext ctx, int image, byte[] data);
        
        /// <summary>
        /// Returns the dimensions of a created image.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(ImageSize))]
        public static extern void ImageSize(this NVGcontext ctx, int image, out int w, out int h);
        
        /// <summary>
        /// Deletes created image.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(DeleteImage))]
        public static extern void DeleteImage(this NVGcontext ctx, int image);
        
        /// <summary>
        /// Creates and returns a linear gradient. Parameters (sx,sy)-(ex,ey) specify the start and end coordinates <br/>
        /// of the linear gradient, icol specifies the start color and ocol the end color. <br/>
        /// The gradient is transformed by the current transform when it is passed to nvgFillPaint() or nvgStrokePaint().
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(LinearGradient))]
        public static extern NVGpaint LinearGradient(this NVGcontext ctx, float sx, float sy, float ex, float ey, NVGcolor icol, NVGcolor ocol);
        
        /// <summary>
        /// Creates and returns a box gradient. Box gradient is a feathered rounded rectangle, it is useful for rendering <br/>
        /// drop shadows or highlights for boxes. Parameters (x,y) define the top-left corner of the rectangle, <br/>
        /// (w,h) define the size of the rectangle, r defines the corner radius, and f feather. Feather defines how blurry <br/>
        /// the border of the rectangle is. Parameter icol specifies the inner color and ocol the outer color of the gradient. <br/>
        /// The gradient is transformed by the current transform when it is passed to nvgFillPaint() or nvgStrokePaint().
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(BoxGradient))]
        public static extern NVGpaint BoxGradient(this NVGcontext ctx, float x, float y, float w, float h, float r, float f, NVGcolor icol, NVGcolor ocol);
        
        /// <summary>
        /// Creates and returns a radial gradient. Parameters (cx,cy) specify the center, inr and outr specify <br/>
        /// the inner and outer radius of the gradient, icol specifies the start color and ocol the end color. <br/>
        /// The gradient is transformed by the current transform when it is passed to nvgFillPaint() or nvgStrokePaint().
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(RadialGradient))]
        public static extern NVGpaint RadialGradient(this NVGcontext ctx, float cx, float cy, float inr, float outr, NVGcolor icol, NVGcolor ocol);
        
        /// <summary>
        /// Creates and returns an image pattern. Parameters (ox,oy) specify the left-top location of the image pattern, <br/>
        /// (ex,ey) the size of one image, angle rotation around the top-left corner, image is handle to the image to render. <br/>
        /// The gradient is transformed by the current transform when it is passed to nvgFillPaint() or nvgStrokePaint().
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(ImagePattern))]
        public static extern NVGpaint ImagePattern(this NVGcontext ctx, float ox, float oy, float ex, float ey, float angle, int image, float alpha);
        
        /// <summary>
        /// Sets the current scissor rectangle. <br/>
        /// The scissor rectangle is transformed by the current transform.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(Scissor))]
        public static extern void Scissor(this NVGcontext ctx, float x, float y, float w, float h);
        
        /// <summary>
        /// Intersects current scissor rectangle with the specified rectangle. <br/>
        /// The scissor rectangle is transformed by the current transform. <br/>
        /// Note: in case the rotation of previous scissor rect differs from <br/>
        /// the current one, the intersection will be done between the specified <br/>
        /// rectangle and the previous scissor rectangle transformed in the current <br/>
        /// transform space. The resulting shape is always rectangle.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(IntersectScissor))]
        public static extern void IntersectScissor(this NVGcontext ctx, float x, float y, float w, float h);
        
        /// <summary>
        /// Reset and disables scissoring.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(ResetScissor))]
        public static extern void ResetScissor(this NVGcontext ctx);
        
        /// <summary>
        /// Clears the current path and sub-paths.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(BeginPath))]
        public static extern void BeginPath(this NVGcontext ctx);
        
        /// <summary>
        /// Starts new sub-path with specified point as first point.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(MoveTo))]
        public static extern void MoveTo(this NVGcontext ctx, float x, float y);
        
        /// <summary>
        /// Adds line segment from the last point in the path to the specified point.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(LineTo))]
        public static extern void LineTo(this NVGcontext ctx, float x, float y);
        
        /// <summary>
        /// Adds cubic bezier segment from last point in the path via two control points to the specified point.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(BezierTo))]
        public static extern void BezierTo(this NVGcontext ctx, float c1x, float c1y, float c2x, float c2y, float x, float y);
        
        /// <summary>
        /// Adds quadratic bezier segment from last point in the path via a control point to the specified point.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(QuadTo))]
        public static extern void QuadTo(this NVGcontext ctx, float cx, float cy, float x, float y);
        
        /// <summary>
        /// Adds an arc segment at the corner defined by the last path point, and two specified points.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(ArcTo))]
        public static extern void ArcTo(this NVGcontext ctx, float x1, float y1, float x2, float y2, float radius);
        
        /// <summary>
        /// Closes current sub-path with a line segment.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(ClosePath))]
        public static extern void ClosePath(this NVGcontext ctx);
        
        /// <summary>
        /// Sets the current sub-path winding, see NVGwinding and NVGsolidity.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(PathWinding))]
        public static extern void PathWinding(this NVGcontext ctx, int dir);
        
        /// <summary>
        /// Creates new circle arc shaped sub-path. The arc center is at cx,cy, the arc radius is r, <br/>
        /// and the arc is drawn from angle a0 to a1, and swept in direction dir (NVG_CCW, or NVG_CW). <br/>
        /// Angles are specified in radians.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(Arc))]
        public static extern void Arc(this NVGcontext ctx, float cx, float cy, float r, float a0, float a1, int dir);
        
        /// <summary>
        /// Creates new rectangle shaped sub-path.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(Rect))]
        public static extern void Rect(this NVGcontext ctx, float x, float y, float w, float h);
        
        /// <summary>
        /// Creates new rounded rectangle shaped sub-path.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(RoundedRect))]
        public static extern void RoundedRect(this NVGcontext ctx, float x, float y, float w, float h, float r);
        
        /// <summary>
        /// Creates new rounded rectangle shaped sub-path with varying radii for each corner.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(RoundedRectVarying))]
        public static extern void RoundedRectVarying(this NVGcontext ctx, float x, float y, float w, float h, float radTopLeft, float radTopRight, float radBottomRight, float radBottomLeft);
        
        /// <summary>
        /// Creates new ellipse shaped sub-path.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(Ellipse))]
        public static extern void Ellipse(this NVGcontext ctx, float cx, float cy, float rx, float ry);
        
        /// <summary>
        /// Creates new circle shaped sub-path.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(Circle))]
        public static extern void Circle(this NVGcontext ctx, float cx, float cy, float r);
        
        /// <summary>
        /// Fills the current path with current fill style.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(Fill))]
        public static extern void Fill(this NVGcontext ctx);
        
        /// <summary>
        /// Fills the current path with current stroke style.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(Stroke))]
        public static extern void Stroke(this NVGcontext ctx);
        
        /// <summary>
        /// Creates font by loading it from the disk from specified file name. <br/>
        /// Returns handle to the font.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(CreateFont))]
        public static extern int CreateFont(this NVGcontext ctx, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string filename);
        
        /// <summary>
        /// fontIndex specifies which font face to load from a .ttf/.ttc file.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(CreateFontAtIndex))]
        public static extern int CreateFontAtIndex(this NVGcontext ctx, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string filename, int fontIndex);
        
        /// <summary>
        /// Creates font by loading it from the specified memory chunk. <br/>
        /// Returns handle to the font.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(CreateFontMem))]
        public static extern int CreateFontMem(this NVGcontext ctx, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, byte[] data, int ndata, int freeData);
        
        /// <summary>
        /// fontIndex specifies which font face to load from a .ttf/.ttc file.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(CreateFontMemAtIndex))]
        public static extern int CreateFontMemAtIndex(this NVGcontext ctx, [MarshalAs(UnmanagedType.LPUTF8Str)] string name, byte[] data, int ndata, int freeData, int fontIndex);
        
        /// <summary>
        /// Finds a loaded font of specified name, and returns handle to it, or -1 if the font is not found.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(FindFont))]
        public static extern int FindFont(this NVGcontext ctx, [MarshalAs(UnmanagedType.LPUTF8Str)] string name);
        
        /// <summary>
        /// Adds a fallback font by handle.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(AddFallbackFontId))]
        public static extern int AddFallbackFontId(this NVGcontext ctx, int baseFont, int fallbackFont);
        
        /// <summary>
        /// Adds a fallback font by name.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(AddFallbackFont))]
        public static extern int AddFallbackFont(this NVGcontext ctx, [MarshalAs(UnmanagedType.LPUTF8Str)] string baseFont, [MarshalAs(UnmanagedType.LPUTF8Str)] string fallbackFont);
        
        /// <summary>
        /// Resets fallback fonts by handle.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(ResetFallbackFontsId))]
        public static extern void ResetFallbackFontsId(this NVGcontext ctx, int baseFont);
        
        /// <summary>
        /// Resets fallback fonts by name.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(ResetFallbackFonts))]
        public static extern void ResetFallbackFonts(this NVGcontext ctx, [MarshalAs(UnmanagedType.LPUTF8Str)] string baseFont);
        
        /// <summary>
        /// Sets the font size of current text style.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(FontSize))]
        public static extern void FontSize(this NVGcontext ctx, float size);
        
        /// <summary>
        /// Sets the blur of current text style.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(FontBlur))]
        public static extern void FontBlur(this NVGcontext ctx, float blur);
        
        /// <summary>
        /// Sets the letter spacing of current text style.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TextLetterSpacing))]
        public static extern void TextLetterSpacing(this NVGcontext ctx, float spacing);
        
        /// <summary>
        /// Sets the proportional line height of current text style. The line height is specified as multiple of font size.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TextLineHeight))]
        public static extern void TextLineHeight(this NVGcontext ctx, float lineHeight);
        
        /// <summary>
        /// Sets the text align of current text style, see NVGalign for options.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TextAlign))]
        public static extern void TextAlign(this NVGcontext ctx, int align);
        
        /// <summary>
        /// Sets the font face based on specified id of current text style.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(FontFaceId))]
        public static extern void FontFaceId(this NVGcontext ctx, int font);
        
        /// <summary>
        /// Sets the font face based on specified name of current text style.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(FontFace))]
        public static extern void FontFace(this NVGcontext ctx, [MarshalAs(UnmanagedType.LPUTF8Str)] string font);
        
        /// <summary>
        /// Draws text string at specified location. If end is specified only the sub-string up to the end is drawn.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(Text))]
        public static extern float Text(this NVGcontext ctx, float x, float y, IntPtr @string, IntPtr end);
        
        /// <summary>
        /// Draws multi-line text string at specified location wrapped at the specified width. If end is specified only the sub-string up to the end is drawn. <br/>
        /// White space is stripped at the beginning of the rows, the text is split at word boundaries or when new-line characters are encountered. <br/>
        /// Words longer than the max width are slit at nearest character (i.e. no hyphenation).
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TextBox))]
        public static extern void TextBox(this NVGcontext ctx, float x, float y, float breakRowWidth, IntPtr @string, IntPtr end);
        
        /// <summary>
        /// Measures the specified text string. Parameter bounds should be a pointer to float[4], <br/>
        /// if the bounding box of the text should be returned. The bounds value are [xmin,ymin, xmax,ymax] <br/>
        /// Returns the horizontal advance of the measured text (i.e. where the next character should drawn). <br/>
        /// Measured values are returned in local coordinate space.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TextBounds))]
        public static extern float TextBounds(this NVGcontext ctx, float x, float y, IntPtr @string, IntPtr end, float[] bounds);
        
        /// <summary>
        /// Measures the specified multi-text string. Parameter bounds should be a pointer to float[4], <br/>
        /// if the bounding box of the text should be returned. The bounds value are [xmin,ymin, xmax,ymax] <br/>
        /// Measured values are returned in local coordinate space.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TextBoxBounds))]
        public static extern void TextBoxBounds(this NVGcontext ctx, float x, float y, float breakRowWidth, IntPtr @string, IntPtr end, float[] bounds);
        
        /// <summary>
        /// Calculates the glyph x positions of the specified text. If end is specified only the sub-string will be used. <br/>
        /// Measured values are returned in local coordinate space.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TextGlyphPositions))]
        public static extern int TextGlyphPositions(this NVGcontext ctx, float x, float y, IntPtr @string, IntPtr end, IntPtr positions, int maxPositions);
        
        /// <summary>
        /// Returns the vertical metrics based on the current text style. <br/>
        /// Measured values are returned in local coordinate space.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TextMetrics))]
        public static extern void TextMetrics(this NVGcontext ctx, out float ascender, out float descender, out float lineh);
        
        /// <summary>
        /// Breaks the specified text into lines. If end is specified only the sub-string will be used. <br/>
        /// White space is stripped at the beginning of the rows, the text is split at word boundaries or when new-line characters are encountered. <br/>
        /// Words longer than the max width are slit at nearest character (i.e. no hyphenation).
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(TextBreakLines))]
        public static extern int TextBreakLines(this NVGcontext ctx, IntPtr @string, IntPtr end, float breakRowWidth, IntPtr rows, int maxRows);
        
        /// <summary>
        /// Debug function to dump cached path data.
        /// </summary>
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(DebugDumpPathCache))]
        public static extern void DebugDumpPathCache(this NVGcontext ctx);
        
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(CreateGL3))]
        public static extern NVGcontext CreateGL3(int flags);
        
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(DeleteGL3))]
        public static extern void DeleteGL3(this NVGcontext ctx);
        
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(lCreateImageFromHandleGL3))]
        public static extern int lCreateImageFromHandleGL3(this NVGcontext ctx, uint textureId, int w, int h, int flags);
        
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(lImageHandleGL3))]
        public static extern uint lImageHandleGL3(this NVGcontext ctx, int image);
        
        [DllImport(LibraryName, EntryPoint = FunctionPrefix + nameof(CreateGL3Context))]
        public static extern NVGcontext CreateGL3Context(int flags);
    }
}
