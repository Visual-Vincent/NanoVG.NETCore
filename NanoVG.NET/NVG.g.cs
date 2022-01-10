// 
// AUTO-GENERATED CODE
// 

using System;
using System.Text;
using System.Runtime.InteropServices;

namespace NanoVG
{
    public unsafe static partial class NVG
    {
        public const string LibraryName = "NanoVG";

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
        [DllImport(LibraryName)]
        public static extern void nvgBeginFrame(NVGcontext* ctx, float windowWidth, float windowHeight, float devicePixelRatio);

        /// <summary>
        /// Cancels drawing the current frame.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgCancelFrame(NVGcontext* ctx);

        /// <summary>
        /// Ends drawing flushing remaining render state.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgEndFrame(NVGcontext* ctx);

        /// <summary>
        /// Sets the composite operation. The op parameter should be one of NVGcompositeOperation.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgGlobalCompositeOperation(NVGcontext* ctx, int op);

        /// <summary>
        /// Sets the composite operation with custom pixel arithmetic. The parameters should be one of NVGblendFactor.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgGlobalCompositeBlendFunc(NVGcontext* ctx, int sfactor, int dfactor);

        /// <summary>
        /// Sets the composite operation with custom pixel arithmetic for RGB and alpha components separately. The parameters should be one of NVGblendFactor.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgGlobalCompositeBlendFuncSeparate(NVGcontext* ctx, int srcRGB, int dstRGB, int srcAlpha, int dstAlpha);

        /// <summary>
        /// Returns a color value from red, green, blue values. Alpha will be set to 255 (1.0f).
        /// </summary>
        [DllImport(LibraryName)]
        public static extern NVGcolor nvgRGB(byte r, byte g, byte b);

        /// <summary>
        /// Returns a color value from red, green, blue values. Alpha will be set to 1.0f.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern NVGcolor nvgRGBf(float r, float g, float b);

        /// <summary>
        /// Returns a color value from red, green, blue and alpha values.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern NVGcolor nvgRGBA(byte r, byte g, byte b, byte a);

        /// <summary>
        /// Returns a color value from red, green, blue and alpha values.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern NVGcolor nvgRGBAf(float r, float g, float b, float a);

        /// <summary>
        /// Linearly interpolates from color c0 to c1, and returns resulting color value.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern NVGcolor nvgLerpRGBA(NVGcolor c0, NVGcolor c1, float u);

        /// <summary>
        /// Sets transparency of a color value.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern NVGcolor nvgTransRGBA(NVGcolor c0, byte a);

        /// <summary>
        /// Sets transparency of a color value.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern NVGcolor nvgTransRGBAf(NVGcolor c0, float a);

        /// <summary>
        /// Returns color value specified by hue, saturation and lightness. <br/>
        /// HSL values are all in range [0..1], alpha will be set to 255.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern NVGcolor nvgHSL(float h, float s, float l);

        /// <summary>
        /// Returns color value specified by hue, saturation and lightness and alpha. <br/>
        /// HSL values are all in range [0..1], alpha in range [0..255]
        /// </summary>
        [DllImport(LibraryName)]
        public static extern NVGcolor nvgHSLA(float h, float s, float l, byte a);

        /// <summary>
        /// Pushes and saves the current render state into a state stack. <br/>
        /// A matching nvgRestore() must be used to restore the state.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgSave(NVGcontext* ctx);

        /// <summary>
        /// Pops and restores current render state.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgRestore(NVGcontext* ctx);

        /// <summary>
        /// Resets current render state to default values. Does not affect the render state stack.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgReset(NVGcontext* ctx);

        /// <summary>
        /// Sets whether to draw antialias for nvgStroke() and nvgFill(). It&apos;s enabled by default.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgShapeAntiAlias(NVGcontext* ctx, int enabled);

        /// <summary>
        /// Sets current stroke style to a solid color.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgStrokeColor(NVGcontext* ctx, NVGcolor color);

        /// <summary>
        /// Sets current stroke style to a paint, which can be a one of the gradients or a pattern.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgStrokePaint(NVGcontext* ctx, NVGpaint paint);

        /// <summary>
        /// Sets current fill style to a solid color.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgFillColor(NVGcontext* ctx, NVGcolor color);

        /// <summary>
        /// Sets current fill style to a paint, which can be a one of the gradients or a pattern.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgFillPaint(NVGcontext* ctx, NVGpaint paint);

        /// <summary>
        /// Sets the miter limit of the stroke style. <br/>
        /// Miter limit controls when a sharp corner is beveled.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgMiterLimit(NVGcontext* ctx, float limit);

        /// <summary>
        /// Sets the stroke width of the stroke style.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgStrokeWidth(NVGcontext* ctx, float size);

        /// <summary>
        /// Sets how the end of the line (cap) is drawn, <br/>
        /// Can be one of: NVG_BUTT (default), NVG_ROUND, NVG_SQUARE.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgLineCap(NVGcontext* ctx, int cap);

        /// <summary>
        /// Sets how sharp path corners are drawn. <br/>
        /// Can be one of NVG_MITER (default), NVG_ROUND, NVG_BEVEL.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgLineJoin(NVGcontext* ctx, int join);

        /// <summary>
        /// Sets the transparency applied to all rendered shapes. <br/>
        /// Already transparent paths will get proportionally more transparent as well.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgGlobalAlpha(NVGcontext* ctx, float alpha);

        /// <summary>
        /// Resets current transform to a identity matrix.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgResetTransform(NVGcontext* ctx);

        /// <summary>
        /// Premultiplies current coordinate system by specified matrix. <br/>
        /// The parameters are interpreted as matrix as follows: <br/>
        /// [a c e] <br/>
        /// [b d f] <br/>
        /// [0 0 1]
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTransform(NVGcontext* ctx, float a, float b, float c, float d, float e, float f);

        /// <summary>
        /// Translates current coordinate system.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTranslate(NVGcontext* ctx, float x, float y);

        /// <summary>
        /// Rotates current coordinate system. Angle is specified in radians.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgRotate(NVGcontext* ctx, float angle);

        /// <summary>
        /// Skews the current coordinate system along X axis. Angle is specified in radians.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgSkewX(NVGcontext* ctx, float angle);

        /// <summary>
        /// Skews the current coordinate system along Y axis. Angle is specified in radians.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgSkewY(NVGcontext* ctx, float angle);

        /// <summary>
        /// Scales the current coordinate system.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgScale(NVGcontext* ctx, float x, float y);

        /// <summary>
        /// Stores the top part (a-f) of the current transformation matrix in to the specified buffer. <br/>
        /// [a c e] <br/>
        /// [b d f] <br/>
        /// [0 0 1] <br/>
        /// There should be space for 6 floats in the return buffer for the values a-f.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgCurrentTransform(NVGcontext* ctx, float* xform);

        /// <summary>
        /// Sets the transform to identity matrix.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTransformIdentity(float* dst);

        /// <summary>
        /// Sets the transform to translation matrix matrix.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTransformTranslate(float* dst, float tx, float ty);

        /// <summary>
        /// Sets the transform to scale matrix.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTransformScale(float* dst, float sx, float sy);

        /// <summary>
        /// Sets the transform to rotate matrix. Angle is specified in radians.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTransformRotate(float* dst, float a);

        /// <summary>
        /// Sets the transform to skew-x matrix. Angle is specified in radians.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTransformSkewX(float* dst, float a);

        /// <summary>
        /// Sets the transform to skew-y matrix. Angle is specified in radians.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTransformSkewY(float* dst, float a);

        /// <summary>
        /// Sets the transform to the result of multiplication of two transforms, of A = A*B.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTransformMultiply(float* dst, float* src);

        /// <summary>
        /// Sets the transform to the result of multiplication of two transforms, of A = B*A.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTransformPremultiply(float* dst, float* src);

        /// <summary>
        /// Sets the destination to inverse of specified transform. <br/>
        /// Returns 1 if the inverse could be calculated, else 0.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern int nvgTransformInverse(float* dst, float* src);

        /// <summary>
        /// Transform a point by given transform.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTransformPoint(float* dstx, float* dsty, float* xform, float srcx, float srcy);

        /// <summary>
        /// Converts degrees to radians and vice versa.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern float nvgDegToRad(float deg);

        [DllImport(LibraryName)]
        public static extern float nvgRadToDeg(float rad);

        /// <summary>
        /// Creates image by loading it from the disk from specified file name. <br/>
        /// Returns handle to the image.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern int nvgCreateImage(NVGcontext* ctx, string filename, int imageFlags);

        /// <summary>
        /// Creates image by loading it from the specified chunk of memory. <br/>
        /// Returns handle to the image.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern int nvgCreateImageMem(NVGcontext* ctx, int imageFlags, byte[] data, int ndata);

        /// <summary>
        /// Creates image from specified image data. <br/>
        /// Returns handle to the image.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern int nvgCreateImageRGBA(NVGcontext* ctx, int w, int h, int imageFlags, byte[] data);

        /// <summary>
        /// Updates image data specified by image handle.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgUpdateImage(NVGcontext* ctx, int image, byte[] data);

        /// <summary>
        /// Returns the dimensions of a created image.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgImageSize(NVGcontext* ctx, int image, int* w, int* h);

        /// <summary>
        /// Deletes created image.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgDeleteImage(NVGcontext* ctx, int image);

        /// <summary>
        /// Creates and returns a linear gradient. Parameters (sx,sy)-(ex,ey) specify the start and end coordinates <br/>
        /// of the linear gradient, icol specifies the start color and ocol the end color. <br/>
        /// The gradient is transformed by the current transform when it is passed to nvgFillPaint() or nvgStrokePaint().
        /// </summary>
        [DllImport(LibraryName)]
        public static extern NVGpaint nvgLinearGradient(NVGcontext* ctx, float sx, float sy, float ex, float ey, NVGcolor icol, NVGcolor ocol);

        /// <summary>
        /// Creates and returns a box gradient. Box gradient is a feathered rounded rectangle, it is useful for rendering <br/>
        /// drop shadows or highlights for boxes. Parameters (x,y) define the top-left corner of the rectangle, <br/>
        /// (w,h) define the size of the rectangle, r defines the corner radius, and f feather. Feather defines how blurry <br/>
        /// the border of the rectangle is. Parameter icol specifies the inner color and ocol the outer color of the gradient. <br/>
        /// The gradient is transformed by the current transform when it is passed to nvgFillPaint() or nvgStrokePaint().
        /// </summary>
        [DllImport(LibraryName)]
        public static extern NVGpaint nvgBoxGradient(NVGcontext* ctx, float x, float y, float w, float h, float r, float f, NVGcolor icol, NVGcolor ocol);

        /// <summary>
        /// Creates and returns a radial gradient. Parameters (cx,cy) specify the center, inr and outr specify <br/>
        /// the inner and outer radius of the gradient, icol specifies the start color and ocol the end color. <br/>
        /// The gradient is transformed by the current transform when it is passed to nvgFillPaint() or nvgStrokePaint().
        /// </summary>
        [DllImport(LibraryName)]
        public static extern NVGpaint nvgRadialGradient(NVGcontext* ctx, float cx, float cy, float inr, float outr, NVGcolor icol, NVGcolor ocol);

        /// <summary>
        /// Creates and returns an image pattern. Parameters (ox,oy) specify the left-top location of the image pattern, <br/>
        /// (ex,ey) the size of one image, angle rotation around the top-left corner, image is handle to the image to render. <br/>
        /// The gradient is transformed by the current transform when it is passed to nvgFillPaint() or nvgStrokePaint().
        /// </summary>
        [DllImport(LibraryName)]
        public static extern NVGpaint nvgImagePattern(NVGcontext* ctx, float ox, float oy, float ex, float ey, float angle, int image, float alpha);

        /// <summary>
        /// Sets the current scissor rectangle. <br/>
        /// The scissor rectangle is transformed by the current transform.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgScissor(NVGcontext* ctx, float x, float y, float w, float h);

        /// <summary>
        /// Intersects current scissor rectangle with the specified rectangle. <br/>
        /// The scissor rectangle is transformed by the current transform. <br/>
        /// Note: in case the rotation of previous scissor rect differs from <br/>
        /// the current one, the intersection will be done between the specified <br/>
        /// rectangle and the previous scissor rectangle transformed in the current <br/>
        /// transform space. The resulting shape is always rectangle.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgIntersectScissor(NVGcontext* ctx, float x, float y, float w, float h);

        /// <summary>
        /// Reset and disables scissoring.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgResetScissor(NVGcontext* ctx);

        /// <summary>
        /// Clears the current path and sub-paths.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgBeginPath(NVGcontext* ctx);

        /// <summary>
        /// Starts new sub-path with specified point as first point.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgMoveTo(NVGcontext* ctx, float x, float y);

        /// <summary>
        /// Adds line segment from the last point in the path to the specified point.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgLineTo(NVGcontext* ctx, float x, float y);

        /// <summary>
        /// Adds cubic bezier segment from last point in the path via two control points to the specified point.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgBezierTo(NVGcontext* ctx, float c1x, float c1y, float c2x, float c2y, float x, float y);

        /// <summary>
        /// Adds quadratic bezier segment from last point in the path via a control point to the specified point.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgQuadTo(NVGcontext* ctx, float cx, float cy, float x, float y);

        /// <summary>
        /// Adds an arc segment at the corner defined by the last path point, and two specified points.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgArcTo(NVGcontext* ctx, float x1, float y1, float x2, float y2, float radius);

        /// <summary>
        /// Closes current sub-path with a line segment.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgClosePath(NVGcontext* ctx);

        /// <summary>
        /// Sets the current sub-path winding, see NVGwinding and NVGsolidity.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgPathWinding(NVGcontext* ctx, int dir);

        /// <summary>
        /// Creates new circle arc shaped sub-path. The arc center is at cx,cy, the arc radius is r, <br/>
        /// and the arc is drawn from angle a0 to a1, and swept in direction dir (NVG_CCW, or NVG_CW). <br/>
        /// Angles are specified in radians.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgArc(NVGcontext* ctx, float cx, float cy, float r, float a0, float a1, int dir);

        /// <summary>
        /// Creates new rectangle shaped sub-path.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgRect(NVGcontext* ctx, float x, float y, float w, float h);

        /// <summary>
        /// Creates new rounded rectangle shaped sub-path.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgRoundedRect(NVGcontext* ctx, float x, float y, float w, float h, float r);

        /// <summary>
        /// Creates new rounded rectangle shaped sub-path with varying radii for each corner.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgRoundedRectVarying(NVGcontext* ctx, float x, float y, float w, float h, float radTopLeft, float radTopRight, float radBottomRight, float radBottomLeft);

        /// <summary>
        /// Creates new ellipse shaped sub-path.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgEllipse(NVGcontext* ctx, float cx, float cy, float rx, float ry);

        /// <summary>
        /// Creates new circle shaped sub-path.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgCircle(NVGcontext* ctx, float cx, float cy, float r);

        /// <summary>
        /// Fills the current path with current fill style.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgFill(NVGcontext* ctx);

        /// <summary>
        /// Fills the current path with current stroke style.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgStroke(NVGcontext* ctx);

        /// <summary>
        /// Creates font by loading it from the disk from specified file name. <br/>
        /// Returns handle to the font.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern int nvgCreateFont(NVGcontext* ctx, string name, string filename);

        /// <summary>
        /// fontIndex specifies which font face to load from a .ttf/.ttc file.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern int nvgCreateFontAtIndex(NVGcontext* ctx, string name, string filename, int fontIndex);

        /// <summary>
        /// Creates font by loading it from the specified memory chunk. <br/>
        /// Returns handle to the font.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern int nvgCreateFontMem(NVGcontext* ctx, string name, byte[] data, int ndata, int freeData);

        /// <summary>
        /// fontIndex specifies which font face to load from a .ttf/.ttc file.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern int nvgCreateFontMemAtIndex(NVGcontext* ctx, string name, byte[] data, int ndata, int freeData, int fontIndex);

        /// <summary>
        /// Finds a loaded font of specified name, and returns handle to it, or -1 if the font is not found.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern int nvgFindFont(NVGcontext* ctx, string name);

        /// <summary>
        /// Adds a fallback font by handle.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern int nvgAddFallbackFontId(NVGcontext* ctx, int baseFont, int fallbackFont);

        /// <summary>
        /// Adds a fallback font by name.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern int nvgAddFallbackFont(NVGcontext* ctx, string baseFont, string fallbackFont);

        /// <summary>
        /// Resets fallback fonts by handle.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgResetFallbackFontsId(NVGcontext* ctx, int baseFont);

        /// <summary>
        /// Resets fallback fonts by name.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgResetFallbackFonts(NVGcontext* ctx, string baseFont);

        /// <summary>
        /// Sets the font size of current text style.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgFontSize(NVGcontext* ctx, float size);

        /// <summary>
        /// Sets the blur of current text style.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgFontBlur(NVGcontext* ctx, float blur);

        /// <summary>
        /// Sets the letter spacing of current text style.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTextLetterSpacing(NVGcontext* ctx, float spacing);

        /// <summary>
        /// Sets the proportional line height of current text style. The line height is specified as multiple of font size.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTextLineHeight(NVGcontext* ctx, float lineHeight);

        /// <summary>
        /// Sets the text align of current text style, see NVGalign for options.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTextAlign(NVGcontext* ctx, int align);

        /// <summary>
        /// Sets the font face based on specified id of current text style.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgFontFaceId(NVGcontext* ctx, int font);

        /// <summary>
        /// Sets the font face based on specified name of current text style.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgFontFace(NVGcontext* ctx, string font);

        /// <summary>
        /// Draws text string at specified location. If end is specified only the sub-string up to the end is drawn.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern float nvgText(NVGcontext* ctx, float x, float y, string @string, string end);

        /// <summary>
        /// Draws multi-line text string at specified location wrapped at the specified width. If end is specified only the sub-string up to the end is drawn. <br/>
        /// White space is stripped at the beginning of the rows, the text is split at word boundaries or when new-line characters are encountered. <br/>
        /// Words longer than the max width are slit at nearest character (i.e. no hyphenation).
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTextBox(NVGcontext* ctx, float x, float y, float breakRowWidth, string @string, string end);

        /// <summary>
        /// Measures the specified text string. Parameter bounds should be a pointer to float[4], <br/>
        /// if the bounding box of the text should be returned. The bounds value are [xmin,ymin, xmax,ymax] <br/>
        /// Returns the horizontal advance of the measured text (i.e. where the next character should drawn). <br/>
        /// Measured values are returned in local coordinate space.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern float nvgTextBounds(NVGcontext* ctx, float x, float y, string @string, string end, float* bounds);

        /// <summary>
        /// Measures the specified multi-text string. Parameter bounds should be a pointer to float[4], <br/>
        /// if the bounding box of the text should be returned. The bounds value are [xmin,ymin, xmax,ymax] <br/>
        /// Measured values are returned in local coordinate space.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTextBoxBounds(NVGcontext* ctx, float x, float y, float breakRowWidth, string @string, string end, float* bounds);

        /// <summary>
        /// Calculates the glyph x positions of the specified text. If end is specified only the sub-string will be used. <br/>
        /// Measured values are returned in local coordinate space.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern int nvgTextGlyphPositions(NVGcontext* ctx, float x, float y, string @string, string end, NVGglyphPosition* positions, int maxPositions);

        /// <summary>
        /// Returns the vertical metrics based on the current text style. <br/>
        /// Measured values are returned in local coordinate space.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgTextMetrics(NVGcontext* ctx, float* ascender, float* descender, float* lineh);

        /// <summary>
        /// Breaks the specified text into lines. If end is specified only the sub-string will be used. <br/>
        /// White space is stripped at the beginning of the rows, the text is split at word boundaries or when new-line characters are encountered. <br/>
        /// Words longer than the max width are slit at nearest character (i.e. no hyphenation).
        /// </summary>
        [DllImport(LibraryName)]
        public static extern int nvgTextBreakLines(NVGcontext* ctx, string @string, string end, float breakRowWidth, NVGtextRow* rows, int maxRows);

        /// <summary>
        /// Constructor and destructor, called by the render back-end.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern NVGcontext* nvgCreateInternal(NVGparams* @params);

        [DllImport(LibraryName)]
        public static extern void nvgDeleteInternal(NVGcontext* ctx);

        [DllImport(LibraryName)]
        public static extern NVGparams* nvgInternalParams(NVGcontext* ctx);

        /// <summary>
        /// Debug function to dump cached path data.
        /// </summary>
        [DllImport(LibraryName)]
        public static extern void nvgDebugDumpPathCache(NVGcontext* ctx);

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

        public struct NVGcompositeOperationState
        {
            public int srcRGB;
            public int dstRGB;
            public int srcAlpha;
            public int dstAlpha;
        }

        public struct NVGglyphPosition
        {
            public string str;
            public float x;
            public float minx, maxx;
        }

        public struct NVGtextRow
        {
            public string start;
            public string end;
            public string next;
            public float width;
            public float minx, maxx;
        }

        public unsafe struct NVGscissor
        {
            public fixed float xform[6];
            public fixed float extent[2];
        }

        public struct NVGvertex
        {
            public float x,y,u,v;
        }

        public struct NVGpath
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

        public struct NVGparams
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

    }
}
