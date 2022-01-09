using System;
using System.Text;
using System.Runtime.InteropServices;

namespace NanoVG
{
    public unsafe static partial class NVG
    {
        public const string LibraryName = "NanoVG";

        [DllImport(LibraryName)]
        public static extern void nvgBeginFrame(NVGcontext* ctx, float windowWidth, float windowHeight, float devicePixelRatio);

        [DllImport(LibraryName)]
        public static extern void nvgCancelFrame(NVGcontext* ctx);

        [DllImport(LibraryName)]
        public static extern void nvgEndFrame(NVGcontext* ctx);

        [DllImport(LibraryName)]
        public static extern void nvgGlobalCompositeOperation(NVGcontext* ctx, int op);

        [DllImport(LibraryName)]
        public static extern void nvgGlobalCompositeBlendFunc(NVGcontext* ctx, int sfactor, int dfactor);

        [DllImport(LibraryName)]
        public static extern void nvgGlobalCompositeBlendFuncSeparate(NVGcontext* ctx, int srcRGB, int dstRGB, int srcAlpha, int dstAlpha);

        [DllImport(LibraryName)]
        public static extern NVGcolor nvgRGB(byte r, byte g, byte b);

        [DllImport(LibraryName)]
        public static extern NVGcolor nvgRGBf(float r, float g, float b);

        [DllImport(LibraryName)]
        public static extern NVGcolor nvgRGBA(byte r, byte g, byte b, byte a);

        [DllImport(LibraryName)]
        public static extern NVGcolor nvgRGBAf(float r, float g, float b, float a);

        [DllImport(LibraryName)]
        public static extern NVGcolor nvgLerpRGBA(NVGcolor c0, NVGcolor c1, float u);

        [DllImport(LibraryName)]
        public static extern NVGcolor nvgTransRGBA(NVGcolor c0, byte a);

        [DllImport(LibraryName)]
        public static extern NVGcolor nvgTransRGBAf(NVGcolor c0, float a);

        [DllImport(LibraryName)]
        public static extern NVGcolor nvgHSL(float h, float s, float l);

        [DllImport(LibraryName)]
        public static extern NVGcolor nvgHSLA(float h, float s, float l, byte a);

        [DllImport(LibraryName)]
        public static extern void nvgSave(NVGcontext* ctx);

        [DllImport(LibraryName)]
        public static extern void nvgRestore(NVGcontext* ctx);

        [DllImport(LibraryName)]
        public static extern void nvgReset(NVGcontext* ctx);

        [DllImport(LibraryName)]
        public static extern void nvgShapeAntiAlias(NVGcontext* ctx, int enabled);

        [DllImport(LibraryName)]
        public static extern void nvgStrokeColor(NVGcontext* ctx, NVGcolor color);

        [DllImport(LibraryName)]
        public static extern void nvgStrokePaint(NVGcontext* ctx, NVGpaint paint);

        [DllImport(LibraryName)]
        public static extern void nvgFillColor(NVGcontext* ctx, NVGcolor color);

        [DllImport(LibraryName)]
        public static extern void nvgFillPaint(NVGcontext* ctx, NVGpaint paint);

        [DllImport(LibraryName)]
        public static extern void nvgMiterLimit(NVGcontext* ctx, float limit);

        [DllImport(LibraryName)]
        public static extern void nvgStrokeWidth(NVGcontext* ctx, float size);

        [DllImport(LibraryName)]
        public static extern void nvgLineCap(NVGcontext* ctx, int cap);

        [DllImport(LibraryName)]
        public static extern void nvgLineJoin(NVGcontext* ctx, int join);

        [DllImport(LibraryName)]
        public static extern void nvgGlobalAlpha(NVGcontext* ctx, float alpha);

        [DllImport(LibraryName)]
        public static extern void nvgResetTransform(NVGcontext* ctx);

        [DllImport(LibraryName)]
        public static extern void nvgTransform(NVGcontext* ctx, float a, float b, float c, float d, float e, float f);

        [DllImport(LibraryName)]
        public static extern void nvgTranslate(NVGcontext* ctx, float x, float y);

        [DllImport(LibraryName)]
        public static extern void nvgRotate(NVGcontext* ctx, float angle);

        [DllImport(LibraryName)]
        public static extern void nvgSkewX(NVGcontext* ctx, float angle);

        [DllImport(LibraryName)]
        public static extern void nvgSkewY(NVGcontext* ctx, float angle);

        [DllImport(LibraryName)]
        public static extern void nvgScale(NVGcontext* ctx, float x, float y);

        [DllImport(LibraryName)]
        public static extern void nvgCurrentTransform(NVGcontext* ctx, float* xform);

        [DllImport(LibraryName)]
        public static extern void nvgTransformIdentity(float* dst);

        [DllImport(LibraryName)]
        public static extern void nvgTransformTranslate(float* dst, float tx, float ty);

        [DllImport(LibraryName)]
        public static extern void nvgTransformScale(float* dst, float sx, float sy);

        [DllImport(LibraryName)]
        public static extern void nvgTransformRotate(float* dst, float a);

        [DllImport(LibraryName)]
        public static extern void nvgTransformSkewX(float* dst, float a);

        [DllImport(LibraryName)]
        public static extern void nvgTransformSkewY(float* dst, float a);

        [DllImport(LibraryName)]
        public static extern void nvgTransformMultiply(float* dst, float* src);

        [DllImport(LibraryName)]
        public static extern void nvgTransformPremultiply(float* dst, float* src);

        [DllImport(LibraryName)]
        public static extern int nvgTransformInverse(float* dst, float* src);

        [DllImport(LibraryName)]
        public static extern void nvgTransformPoint(float* dstx, float* dsty, float* xform, float srcx, float srcy);

        [DllImport(LibraryName)]
        public static extern float nvgDegToRad(float deg);

        [DllImport(LibraryName)]
        public static extern float nvgRadToDeg(float rad);

        [DllImport(LibraryName)]
        public static extern int nvgCreateImage(NVGcontext* ctx, string filename, int imageFlags);

        [DllImport(LibraryName)]
        public static extern int nvgCreateImageMem(NVGcontext* ctx, int imageFlags, byte[] data, int ndata);

        [DllImport(LibraryName)]
        public static extern int nvgCreateImageRGBA(NVGcontext* ctx, int w, int h, int imageFlags, byte[] data);

        [DllImport(LibraryName)]
        public static extern void nvgUpdateImage(NVGcontext* ctx, int image, byte[] data);

        [DllImport(LibraryName)]
        public static extern void nvgImageSize(NVGcontext* ctx, int image, int* w, int* h);

        [DllImport(LibraryName)]
        public static extern void nvgDeleteImage(NVGcontext* ctx, int image);

        [DllImport(LibraryName)]
        public static extern NVGpaint nvgLinearGradient(NVGcontext* ctx, float sx, float sy, float ex, float ey, NVGcolor icol, NVGcolor ocol);

        [DllImport(LibraryName)]
        public static extern NVGpaint nvgBoxGradient(NVGcontext* ctx, float x, float y, float w, float h, float r, float f, NVGcolor icol, NVGcolor ocol);

        [DllImport(LibraryName)]
        public static extern NVGpaint nvgRadialGradient(NVGcontext* ctx, float cx, float cy, float inr, float outr, NVGcolor icol, NVGcolor ocol);

        [DllImport(LibraryName)]
        public static extern NVGpaint nvgImagePattern(NVGcontext* ctx, float ox, float oy, float ex, float ey, float angle, int image, float alpha);

        [DllImport(LibraryName)]
        public static extern void nvgScissor(NVGcontext* ctx, float x, float y, float w, float h);

        [DllImport(LibraryName)]
        public static extern void nvgIntersectScissor(NVGcontext* ctx, float x, float y, float w, float h);

        [DllImport(LibraryName)]
        public static extern void nvgResetScissor(NVGcontext* ctx);

        [DllImport(LibraryName)]
        public static extern void nvgBeginPath(NVGcontext* ctx);

        [DllImport(LibraryName)]
        public static extern void nvgMoveTo(NVGcontext* ctx, float x, float y);

        [DllImport(LibraryName)]
        public static extern void nvgLineTo(NVGcontext* ctx, float x, float y);

        [DllImport(LibraryName)]
        public static extern void nvgBezierTo(NVGcontext* ctx, float c1x, float c1y, float c2x, float c2y, float x, float y);

        [DllImport(LibraryName)]
        public static extern void nvgQuadTo(NVGcontext* ctx, float cx, float cy, float x, float y);

        [DllImport(LibraryName)]
        public static extern void nvgArcTo(NVGcontext* ctx, float x1, float y1, float x2, float y2, float radius);

        [DllImport(LibraryName)]
        public static extern void nvgClosePath(NVGcontext* ctx);

        [DllImport(LibraryName)]
        public static extern void nvgPathWinding(NVGcontext* ctx, int dir);

        [DllImport(LibraryName)]
        public static extern void nvgArc(NVGcontext* ctx, float cx, float cy, float r, float a0, float a1, int dir);

        [DllImport(LibraryName)]
        public static extern void nvgRect(NVGcontext* ctx, float x, float y, float w, float h);

        [DllImport(LibraryName)]
        public static extern void nvgRoundedRect(NVGcontext* ctx, float x, float y, float w, float h, float r);

        [DllImport(LibraryName)]
        public static extern void nvgRoundedRectVarying(NVGcontext* ctx, float x, float y, float w, float h, float radTopLeft, float radTopRight, float radBottomRight, float radBottomLeft);

        [DllImport(LibraryName)]
        public static extern void nvgEllipse(NVGcontext* ctx, float cx, float cy, float rx, float ry);

        [DllImport(LibraryName)]
        public static extern void nvgCircle(NVGcontext* ctx, float cx, float cy, float r);

        [DllImport(LibraryName)]
        public static extern void nvgFill(NVGcontext* ctx);

        [DllImport(LibraryName)]
        public static extern void nvgStroke(NVGcontext* ctx);

        [DllImport(LibraryName)]
        public static extern int nvgCreateFont(NVGcontext* ctx, string name, string filename);

        [DllImport(LibraryName)]
        public static extern int nvgCreateFontAtIndex(NVGcontext* ctx, string name, string filename, int fontIndex);

        [DllImport(LibraryName)]
        public static extern int nvgCreateFontMem(NVGcontext* ctx, string name, byte[] data, int ndata, int freeData);

        [DllImport(LibraryName)]
        public static extern int nvgCreateFontMemAtIndex(NVGcontext* ctx, string name, byte[] data, int ndata, int freeData, int fontIndex);

        [DllImport(LibraryName)]
        public static extern int nvgFindFont(NVGcontext* ctx, string name);

        [DllImport(LibraryName)]
        public static extern int nvgAddFallbackFontId(NVGcontext* ctx, int baseFont, int fallbackFont);

        [DllImport(LibraryName)]
        public static extern int nvgAddFallbackFont(NVGcontext* ctx, string baseFont, string fallbackFont);

        [DllImport(LibraryName)]
        public static extern void nvgResetFallbackFontsId(NVGcontext* ctx, int baseFont);

        [DllImport(LibraryName)]
        public static extern void nvgResetFallbackFonts(NVGcontext* ctx, string baseFont);

        [DllImport(LibraryName)]
        public static extern void nvgFontSize(NVGcontext* ctx, float size);

        [DllImport(LibraryName)]
        public static extern void nvgFontBlur(NVGcontext* ctx, float blur);

        [DllImport(LibraryName)]
        public static extern void nvgTextLetterSpacing(NVGcontext* ctx, float spacing);

        [DllImport(LibraryName)]
        public static extern void nvgTextLineHeight(NVGcontext* ctx, float lineHeight);

        [DllImport(LibraryName)]
        public static extern void nvgTextAlign(NVGcontext* ctx, int align);

        [DllImport(LibraryName)]
        public static extern void nvgFontFaceId(NVGcontext* ctx, int font);

        [DllImport(LibraryName)]
        public static extern void nvgFontFace(NVGcontext* ctx, string font);

        [DllImport(LibraryName)]
        public static extern float nvgText(NVGcontext* ctx, float x, float y, string @string, string end);

        [DllImport(LibraryName)]
        public static extern void nvgTextBox(NVGcontext* ctx, float x, float y, float breakRowWidth, string @string, string end);

        [DllImport(LibraryName)]
        public static extern float nvgTextBounds(NVGcontext* ctx, float x, float y, string @string, string end, float* bounds);

        [DllImport(LibraryName)]
        public static extern void nvgTextBoxBounds(NVGcontext* ctx, float x, float y, float breakRowWidth, string @string, string end, float* bounds);

        [DllImport(LibraryName)]
        public static extern int nvgTextGlyphPositions(NVGcontext* ctx, float x, float y, string @string, string end, NVGglyphPosition* positions, int maxPositions);

        [DllImport(LibraryName)]
        public static extern void nvgTextMetrics(NVGcontext* ctx, float* ascender, float* descender, float* lineh);

        [DllImport(LibraryName)]
        public static extern int nvgTextBreakLines(NVGcontext* ctx, string @string, string end, float breakRowWidth, NVGtextRow* rows, int maxRows);

        [DllImport(LibraryName)]
        public static extern NVGcontext* nvgCreateInternal(NVGparams* @params);

        [DllImport(LibraryName)]
        public static extern void nvgDeleteInternal(NVGcontext* ctx);

        [DllImport(LibraryName)]
        public static extern NVGparams* nvgInternalParams(NVGcontext* ctx);

        [DllImport(LibraryName)]
        public static extern void nvgDebugDumpPathCache(NVGcontext* ctx);

    }
}
