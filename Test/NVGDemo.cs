using System;
using System.Text;

namespace NanoVG.Test
{
	public class DemoData
	{
		public int fontNormal, fontBold, fontIcons, fontEmoji;
		public int[] images;
	}

	public static unsafe class NVGDemo
    {
		public const int ICON_SEARCH = 0x1F50D;
		public const int ICON_CIRCLED_CROSS = 0x2716;
		public const int ICON_CHEVRON_RIGHT = 0xE75E;
		public const int ICON_CHECK = 0x2713;
		public const int ICON_LOGIN = 0xE740;
		public const int ICON_TRASH = 0xE729;

		static float clampf(float a, float mn, float mx) { return a < mn ? mn : (a > mx ? mx : a); }

		// Returns 1 if col.rgba is 0.0f,0.0f,0.0f,0.0f, 0 otherwise
		static bool isBlack(NVGcolor col)
		{
			if( col.r == 0.0f && col.g == 0.0f && col.b == 0.0f && col.a == 0.0f )
			{
				return true;
			}
			return false;
		}
		
		static string cpToUTF8(int cp)
		{
			byte[] str = new byte[8];
			int n = 0;
			if (cp < 0x80) n = 1;
			else if (cp < 0x800) n = 2;
			else if (cp < 0x10000) n = 3;
			else if (cp < 0x200000) n = 4;
			else if (cp < 0x4000000) n = 5;
			else if (cp <= 0x7fffffff) n = 6;
			str[n] = (byte)'\0';
			switch (n) {
				case 6: str[5] = (byte)(0x80 | (cp & 0x3f)); cp = cp >> 6; cp |= 0x4000000; goto case 5;
				case 5: str[4] = (byte)(0x80 | (cp & 0x3f)); cp = cp >> 6; cp |= 0x200000; goto case 4;
				case 4: str[3] = (byte)(0x80 | (cp & 0x3f)); cp = cp >> 6; cp |= 0x10000; goto case 3;
				case 3: str[2] = (byte)(0x80 | (cp & 0x3f)); cp = cp >> 6; cp |= 0x800; goto case 2;
				case 2: str[1] = (byte)(0x80 | (cp & 0x3f)); cp = cp >> 6; cp |= 0xc0; goto case 1;
				case 1: str[0] = (byte)cp; break;
			}
			return Encoding.UTF8.GetString(str, 0, n);
		}

		public static void drawWindow(NVGcontext vg, string title, float x, float y, float w, float h)
		{
			float cornerRadius = 3.0f;
			NVGpaint shadowPaint;
			NVGpaint headerPaint;

			vg.Save();
		//	vg.ClearState();

			// Window
			vg.BeginPath();
			vg.RoundedRect(x,y, w,h, cornerRadius);
			vg.FillColor(NVG.RGBA(28,30,34,192));
		//	vg.FillColor(NVG.RGBA(0,0,0,128));
			vg.Fill();

			// Drop shadow
			shadowPaint = vg.BoxGradient(x,y+2, w,h, cornerRadius*2, 10, NVG.RGBA(0,0,0,128), NVG.RGBA(0,0,0,0));
			vg.BeginPath();
			vg.Rect(x-10,y-10, w+20,h+30);
			vg.RoundedRect(x,y, w,h, cornerRadius);
			vg.PathWinding((int)NVGsolidity.NVG_HOLE);
			vg.FillPaint(shadowPaint);
			vg.Fill();

			// Header
			headerPaint = vg.LinearGradient(x,y,x,y+15, NVG.RGBA(255,255,255,8), NVG.RGBA(0,0,0,16));
			vg.BeginPath();
			vg.RoundedRect(x+1,y+1, w-2,30, cornerRadius-1);
			vg.FillPaint(headerPaint);
			vg.Fill();
			vg.BeginPath();
			vg.MoveTo(x+0.5f, y+0.5f+30);
			vg.LineTo(x+0.5f+w-1, y+0.5f+30);
			vg.StrokeColor(NVG.RGBA(0,0,0,32));
			vg.Stroke();

			vg.FontSize(15.0f);
			vg.FontFace("sans-bold");
			vg.TextAlign((int)(NVGalign.NVG_ALIGN_CENTER | NVGalign.NVG_ALIGN_MIDDLE));

			vg.FontBlur(2);
			vg.FillColor(NVG.RGBA(0,0,0,128));
			vg.Text(x+w/2,y+16+1, title, null);

			vg.FontBlur(0);
			vg.FillColor(NVG.RGBA(220,220,220,160));
			vg.Text(x+w/2,y+16, title, null);

			vg.Restore();
		}

		public static void drawSearchBox(NVGcontext vg, string text, float x, float y, float w, float h)
		{
			NVGpaint bg;
			float cornerRadius = h/2-1;

			// Edit
			bg = vg.BoxGradient(x,y+1.5f, w,h, h/2,5, NVG.RGBA(0,0,0,16), NVG.RGBA(0,0,0,92));
			vg.BeginPath();
			vg.RoundedRect(x,y, w,h, cornerRadius);
			vg.FillPaint(bg);
			vg.Fill();

		/*	vg.BeginPath();
			vg.RoundedRect(x+0.5f,y+0.5f, w-1,h-1, cornerRadius-0.5f);
			vg.StrokeColor(NVG.RGBA(0,0,0,48));
			vg.Stroke();*/
			
			vg.FontSize(h*1.3f);
			vg.FontFace("icons");
			vg.FillColor(NVG.RGBA(255,255,255,64));
			vg.TextAlign((int)(NVGalign.NVG_ALIGN_CENTER | NVGalign.NVG_ALIGN_MIDDLE));
			vg.Text(x+h*0.55f, y+h*0.55f, cpToUTF8(ICON_SEARCH), null);

			vg.FontSize(17.0f);
			vg.FontFace("sans");
			vg.FillColor(NVG.RGBA(255,255,255,32));

			vg.TextAlign((int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_MIDDLE));
			vg.Text(x+h*1.05f,y+h*0.5f,text, null);

			vg.FontSize(h*1.3f);
			vg.FontFace("icons");
			vg.FillColor(NVG.RGBA(255,255,255,32));
			vg.TextAlign((int)(NVGalign.NVG_ALIGN_CENTER | NVGalign.NVG_ALIGN_MIDDLE));
			vg.Text(x+w-h*0.55f, y+h*0.55f, cpToUTF8(ICON_CIRCLED_CROSS), null);
		}

		public static void drawDropDown(NVGcontext vg, string text, float x, float y, float w, float h)
		{
			NVGpaint bg;
			float cornerRadius = 4.0f;

			bg = vg.LinearGradient(x,y,x,y+h, NVG.RGBA(255,255,255,16), NVG.RGBA(0,0,0,16));
			vg.BeginPath();
			vg.RoundedRect(x+1,y+1, w-2,h-2, cornerRadius-1);
			vg.FillPaint(bg);
			vg.Fill();

			vg.BeginPath();
			vg.RoundedRect(x+0.5f,y+0.5f, w-1,h-1, cornerRadius-0.5f);
			vg.StrokeColor(NVG.RGBA(0,0,0,48));
			vg.Stroke();

			vg.FontSize(17.0f);
			vg.FontFace("sans");
			vg.FillColor(NVG.RGBA(255,255,255,160));
			vg.TextAlign((int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_MIDDLE));
			vg.Text(x+h*0.3f,y+h*0.5f,text, null);

			vg.FontSize(h*1.3f);
			vg.FontFace("icons");
			vg.FillColor(NVG.RGBA(255,255,255,64));
			vg.TextAlign((int)(NVGalign.NVG_ALIGN_CENTER | NVGalign.NVG_ALIGN_MIDDLE));
			vg.Text(x+w-h*0.5f, y+h*0.5f, cpToUTF8(ICON_CHEVRON_RIGHT), null);
		}

		public static void drawLabel(NVGcontext vg, string text, float x, float y, float w, float h)
		{
			vg.FontSize(15.0f);
			vg.FontFace("sans");
			vg.FillColor(NVG.RGBA(255,255,255,128));

			vg.TextAlign((int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_MIDDLE));
			vg.Text(x,y+h*0.5f,text, null);
		}

		public static void drawEditBoxBase(NVGcontext vg, float x, float y, float w, float h)
		{
			NVGpaint bg;
			// Edit
			bg = vg.BoxGradient(x+1,y+1+1.5f, w-2,h-2, 3,4, NVG.RGBA(255,255,255,32), NVG.RGBA(32,32,32,32));
			vg.BeginPath();
			vg.RoundedRect(x+1,y+1, w-2,h-2, 4-1);
			vg.FillPaint(bg);
			vg.Fill();

			vg.BeginPath();
			vg.RoundedRect(x+0.5f,y+0.5f, w-1,h-1, 4-0.5f);
			vg.StrokeColor(NVG.RGBA(0,0,0,48));
			vg.Stroke();
		}

		public static void drawEditBox(NVGcontext vg, string text, float x, float y, float w, float h)
		{

			drawEditBoxBase(vg, x,y, w,h);

			vg.FontSize(17.0f);
			vg.FontFace("sans");
			vg.FillColor(NVG.RGBA(255,255,255,64));
			vg.TextAlign((int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_MIDDLE));
			vg.Text(x+h*0.3f,y+h*0.5f,text, null);
		}

		public static void drawEditBoxNum(NVGcontext vg,
							string text, string units, float x, float y, float w, float h)
		{
			float uw;

			drawEditBoxBase(vg, x,y, w,h);

			uw = vg.TextBounds(0,0, units, null, null);

			vg.FontSize(15.0f);
			vg.FontFace("sans");
			vg.FillColor(NVG.RGBA(255,255,255,64));
			vg.TextAlign((int)(NVGalign.NVG_ALIGN_RIGHT | NVGalign.NVG_ALIGN_MIDDLE));
			vg.Text(x+w-h*0.3f,y+h*0.5f,units, null);

			vg.FontSize(17.0f);
			vg.FontFace("sans");
			vg.FillColor(NVG.RGBA(255,255,255,128));
			vg.TextAlign((int)(NVGalign.NVG_ALIGN_RIGHT | NVGalign.NVG_ALIGN_MIDDLE));
			vg.Text(x+w-uw-h*0.5f,y+h*0.5f,text, null);
		}

		public static void drawCheckBox(NVGcontext vg, string text, float x, float y, float w, float h)
		{
			NVGpaint bg;

			vg.FontSize(15.0f);
			vg.FontFace("sans");
			vg.FillColor(NVG.RGBA(255,255,255,160));

			vg.TextAlign((int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_MIDDLE));
			vg.Text(x+28,y+h*0.5f,text, null);

			bg = vg.BoxGradient(x+1,y+(int)(h*0.5f)-9+1, 18,18, 3,3, NVG.RGBA(0,0,0,32), NVG.RGBA(0,0,0,92));
			vg.BeginPath();
			vg.RoundedRect(x+1,y+(int)(h*0.5f)-9, 18,18, 3);
			vg.FillPaint(bg);
			vg.Fill();

			vg.FontSize(33);
			vg.FontFace("icons");
			vg.FillColor(NVG.RGBA(255,255,255,128));
			vg.TextAlign((int)(NVGalign.NVG_ALIGN_CENTER | NVGalign.NVG_ALIGN_MIDDLE));
			vg.Text(x+9+2, y+h*0.5f, cpToUTF8(ICON_CHECK), null);
		}

		public static void drawButton(NVGcontext vg, int preicon, string text, float x, float y, float w, float h, NVGcolor col)
		{
			NVGpaint bg;
			float cornerRadius = 4.0f;
			float tw = 0, iw = 0;

			bg = vg.LinearGradient(x,y,x,y+h, NVG.RGBA(255,255,255, isBlack(col) ? (byte)16 : (byte)32), NVG.RGBA(0,0,0, isBlack(col) ? (byte)16 : (byte)32));
			vg.BeginPath();
			vg.RoundedRect(x+1,y+1, w-2,h-2, cornerRadius-1);
			if (!isBlack(col)) {
				vg.FillColor(col);
				vg.Fill();
			}
			vg.FillPaint(bg);
			vg.Fill();

			vg.BeginPath();
			vg.RoundedRect(x+0.5f,y+0.5f, w-1,h-1, cornerRadius-0.5f);
			vg.StrokeColor(NVG.RGBA(0,0,0,48));
			vg.Stroke();

			vg.FontSize(17.0f);
			vg.FontFace("sans-bold");
			tw = vg.TextBounds(0,0, text, null, null);
			if (preicon != 0) {
				vg.FontSize(h*1.3f);
				vg.FontFace("icons");
				iw = vg.TextBounds(0,0, cpToUTF8(preicon), null, null);
				iw += h*0.15f;
			}

			if (preicon != 0) {
				vg.FontSize(h*1.3f);
				vg.FontFace("icons");
				vg.FillColor(NVG.RGBA(255,255,255,96));
				vg.TextAlign((int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_MIDDLE));
				vg.Text(x+w*0.5f-tw*0.5f-iw*0.75f, y+h*0.5f, cpToUTF8(preicon), null);
			}

			vg.FontSize(17.0f);
			vg.FontFace("sans-bold");
			vg.TextAlign((int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_MIDDLE));
			vg.FillColor(NVG.RGBA(0,0,0,160));
			vg.Text(x+w*0.5f-tw*0.5f+iw*0.25f,y+h*0.5f-1,text, null);
			vg.FillColor(NVG.RGBA(255,255,255,160));
			vg.Text(x+w*0.5f-tw*0.5f+iw*0.25f,y+h*0.5f,text, null);
		}

		public static void drawSlider(NVGcontext vg, float pos, float x, float y, float w, float h)
		{
			NVGpaint bg, knob;
			float cy = y+(int)(h*0.5f);
			float kr = (int)(h*0.25f);

			vg.Save();
		//	vg.ClearState();

			// Slot
			bg = vg.BoxGradient(x,cy-2+1, w,4, 2,2, NVG.RGBA(0,0,0,32), NVG.RGBA(0,0,0,128));
			vg.BeginPath();
			vg.RoundedRect(x,cy-2, w,4, 2);
			vg.FillPaint(bg);
			vg.Fill();

			// Knob Shadow
			bg = vg.RadialGradient(x+(int)(pos*w),cy+1, kr-3,kr+3, NVG.RGBA(0,0,0,64), NVG.RGBA(0,0,0,0));
			vg.BeginPath();
			vg.Rect(x+(int)(pos*w)-kr-5,cy-kr-5,kr*2+5+5,kr*2+5+5+3);
			vg.Circle(x+(int)(pos*w),cy, kr);
			vg.PathWinding((int)NVGsolidity.NVG_HOLE);
			vg.FillPaint(bg);
			vg.Fill();

			// Knob
			knob = vg.LinearGradient(x,cy-kr,x,cy+kr, NVG.RGBA(255,255,255,16), NVG.RGBA(0,0,0,16));
			vg.BeginPath();
			vg.Circle(x+(int)(pos*w),cy, kr-1);
			vg.FillColor(NVG.RGBA(40,43,48,255));
			vg.Fill();
			vg.FillPaint(knob);
			vg.Fill();

			vg.BeginPath();
			vg.Circle(x+(int)(pos*w),cy, kr-0.5f);
			vg.StrokeColor(NVG.RGBA(0,0,0,92));
			vg.Stroke();

			vg.Restore();
		}

		public static void drawEyes(NVGcontext vg, float x, float y, float w, float h, float mx, float my, float t)
		{
			NVGpaint gloss, bg;
			float ex = w *0.23f;
			float ey = h * 0.5f;
			float lx = x + ex;
			float ly = y + ey;
			float rx = x + w - ex;
			float ry = y + ey;
			float dx,dy,d;
			float br = (ex < ey ? ex : ey) * 0.5f;
			float blink = 1 - MathF.Pow(MathF.Sin(t*0.5f),200)*0.8f;

			bg = vg.LinearGradient(x,y+h*0.5f,x+w*0.1f,y+h, NVG.RGBA(0,0,0,32), NVG.RGBA(0,0,0,16));
			vg.BeginPath();
			vg.Ellipse(lx+3.0f,ly+16.0f, ex,ey);
			vg.Ellipse(rx+3.0f,ry+16.0f, ex,ey);
			vg.FillPaint(bg);
			vg.Fill();

			bg = vg.LinearGradient(x,y+h*0.25f,x+w*0.1f,y+h, NVG.RGBA(220,220,220,255), NVG.RGBA(128,128,128,255));
			vg.BeginPath();
			vg.Ellipse(lx,ly, ex,ey);
			vg.Ellipse(rx,ry, ex,ey);
			vg.FillPaint(bg);
			vg.Fill();

			dx = (mx - rx) / (ex * 10);
			dy = (my - ry) / (ey * 10);
			d = MathF.Sqrt(dx*dx+dy*dy);
			if (d > 1.0f) {
				dx /= d; dy /= d;
			}
			dx *= ex*0.4f;
			dy *= ey*0.5f;
			vg.BeginPath();
			vg.Ellipse(lx+dx,ly+dy+ey*0.25f*(1-blink), br,br*blink);
			vg.FillColor(NVG.RGBA(32,32,32,255));
			vg.Fill();

			dx = (mx - rx) / (ex * 10);
			dy = (my - ry) / (ey * 10);
			d = MathF.Sqrt(dx*dx+dy*dy);
			if (d > 1.0f) {
				dx /= d; dy /= d;
			}
			dx *= ex*0.4f;
			dy *= ey*0.5f;
			vg.BeginPath();
			vg.Ellipse(rx+dx,ry+dy+ey*0.25f*(1-blink), br,br*blink);
			vg.FillColor(NVG.RGBA(32,32,32,255));
			vg.Fill();

			gloss = vg.RadialGradient(lx-ex*0.25f,ly-ey*0.5f, ex*0.1f,ex*0.75f, NVG.RGBA(255,255,255,128), NVG.RGBA(255,255,255,0));
			vg.BeginPath();
			vg.Ellipse(lx,ly, ex,ey);
			vg.FillPaint(gloss);
			vg.Fill();

			gloss = vg.RadialGradient(rx-ex*0.25f,ry-ey*0.5f, ex*0.1f,ex*0.75f, NVG.RGBA(255,255,255,128), NVG.RGBA(255,255,255,0));
			vg.BeginPath();
			vg.Ellipse(rx,ry, ex,ey);
			vg.FillPaint(gloss);
			vg.Fill();
		}

		public static void drawGraph(NVGcontext vg, float x, float y, float w, float h, float t)
		{
			NVGpaint bg;
			float[] samples = new float[6];
			float[] sx = new float[6], sy = new float[6];
			float dx = w/5.0f;
			int i;

			samples[0] = (1+MathF.Sin(t*1.2345f+MathF.Cos(t*0.33457f)*0.44f))*0.5f;
			samples[1] = (1+MathF.Sin(t*0.68363f+MathF.Cos(t*1.3f)*1.55f))*0.5f;
			samples[2] = (1+MathF.Sin(t*1.1642f+MathF.Cos(t*0.33457f)*1.24f))*0.5f;
			samples[3] = (1+MathF.Sin(t*0.56345f+MathF.Cos(t*1.63f)*0.14f))*0.5f;
			samples[4] = (1+MathF.Sin(t*1.6245f+MathF.Cos(t*0.254f)*0.3f))*0.5f;
			samples[5] = (1+MathF.Sin(t*0.345f+MathF.Cos(t*0.03f)*0.6f))*0.5f;

			for (i = 0; i < 6; i++) {
				sx[i] = x+i*dx;
				sy[i] = y+h*samples[i]*0.8f;
			}

			// Graph background
			bg = vg.LinearGradient(x,y,x,y+h, NVG.RGBA(0,160,192,0), NVG.RGBA(0,160,192,64));
			vg.BeginPath();
			vg.MoveTo(sx[0], sy[0]);
			for (i = 1; i < 6; i++)
				vg.BezierTo(sx[i-1]+dx*0.5f,sy[i-1], sx[i]-dx*0.5f,sy[i], sx[i],sy[i]);
			vg.LineTo(x+w, y+h);
			vg.LineTo(x, y+h);
			vg.FillPaint(bg);
			vg.Fill();

			// Graph line
			vg.BeginPath();
			vg.MoveTo(sx[0], sy[0]+2);
			for (i = 1; i < 6; i++)
				vg.BezierTo(sx[i-1]+dx*0.5f,sy[i-1]+2, sx[i]-dx*0.5f,sy[i]+2, sx[i],sy[i]+2);
			vg.StrokeColor(NVG.RGBA(0,0,0,32));
			vg.StrokeWidth(3.0f);
			vg.Stroke();

			vg.BeginPath();
			vg.MoveTo(sx[0], sy[0]);
			for (i = 1; i < 6; i++)
				vg.BezierTo(sx[i-1]+dx*0.5f,sy[i-1], sx[i]-dx*0.5f,sy[i], sx[i],sy[i]);
			vg.StrokeColor(NVG.RGBA(0,160,192,255));
			vg.StrokeWidth(3.0f);
			vg.Stroke();

			// Graph sample pos
			for (i = 0; i < 6; i++) {
				bg = vg.RadialGradient(sx[i],sy[i]+2, 3.0f,8.0f, NVG.RGBA(0,0,0,32), NVG.RGBA(0,0,0,0));
				vg.BeginPath();
				vg.Rect(sx[i]-10, sy[i]-10+2, 20,20);
				vg.FillPaint(bg);
				vg.Fill();
			}

			vg.BeginPath();
			for (i = 0; i < 6; i++)
				vg.Circle(sx[i], sy[i], 4.0f);
			vg.FillColor(NVG.RGBA(0,160,192,255));
			vg.Fill();
			vg.BeginPath();
			for (i = 0; i < 6; i++)
				vg.Circle(sx[i], sy[i], 2.0f);
			vg.FillColor(NVG.RGBA(220,220,220,255));
			vg.Fill();

			vg.StrokeWidth(1.0f);
		}

		public static void drawSpinner(NVGcontext vg, float cx, float cy, float r, float t)
		{
			float a0 = 0.0f + t*6;
			float a1 = MathF.PI + t*6;
			float r0 = r;
			float r1 = r * 0.75f;
			float ax,ay, bx,by;
			NVGpaint paint;

			vg.Save();

			vg.BeginPath();
			vg.Arc(cx,cy, r0, a0, a1, (int)NVGwinding.NVG_CW);
			vg.Arc(cx,cy, r1, a1, a0, (int)NVGwinding.NVG_CCW);
			vg.ClosePath();
			ax = cx + MathF.Cos(a0) * (r0+r1)*0.5f;
			ay = cy + MathF.Sin(a0) * (r0+r1)*0.5f;
			bx = cx + MathF.Cos(a1) * (r0+r1)*0.5f;
			by = cy + MathF.Sin(a1) * (r0+r1)*0.5f;
			paint = vg.LinearGradient(ax,ay, bx,by, NVG.RGBA(0,0,0,0), NVG.RGBA(0,0,0,128));
			vg.FillPaint(paint);
			vg.Fill();

			vg.Restore();
		}

		public static void drawThumbnails(NVGcontext vg, float x, float y, float w, float h, int[] images, int nimages, float t)
		{
			float cornerRadius = 3.0f;
			NVGpaint shadowPaint, imgPaint, fadePaint;
			float ix,iy,iw,ih;
			float thumb = 60.0f;
			float arry = 30.5f;
			int imgw = 0, imgh = 0;
			float stackh = (nimages/2) * (thumb+10) + 10;
			int i;
			float u = (1+MathF.Cos(t*0.5f))*0.5f;
			float u2 = (1-MathF.Cos(t*0.2f))*0.5f;
			float scrollh, dv;

			vg.Save();
		//	vg.ClearState();

			// Drop shadow
			shadowPaint = vg.BoxGradient(x,y+4, w,h, cornerRadius*2, 20, NVG.RGBA(0,0,0,128), NVG.RGBA(0,0,0,0));
			vg.BeginPath();
			vg.Rect(x-10,y-10, w+20,h+30);
			vg.RoundedRect(x,y, w,h, cornerRadius);
			vg.PathWinding((int)NVGsolidity.NVG_HOLE);
			vg.FillPaint(shadowPaint);
			vg.Fill();

			// Window
			vg.BeginPath();
			vg.RoundedRect(x,y, w,h, cornerRadius);
			vg.MoveTo(x-10,y+arry);
			vg.LineTo(x+1,y+arry-11);
			vg.LineTo(x+1,y+arry+11);
			vg.FillColor(NVG.RGBA(200,200,200,255));
			vg.Fill();

			vg.Save();
			vg.Scissor(x,y,w,h);
			vg.Translate(0, -(stackh - h)*u);

			dv = 1.0f / (float)(nimages-1);

			for (i = 0; i < nimages; i++) {
				float tx, ty, v, a;
				tx = x+10;
				ty = y+10;
				tx += (i%2) * (thumb+10);
				ty += (i/2) * (thumb+10);
				vg.ImageSize(images[i], ref imgw, ref imgh);
				if (imgw < imgh) {
					iw = thumb;
					ih = iw * (float)imgh/(float)imgw;
					ix = 0;
					iy = -(ih-thumb)*0.5f;
				} else {
					ih = thumb;
					iw = ih * (float)imgw/(float)imgh;
					ix = -(iw-thumb)*0.5f;
					iy = 0;
				}

				v = i * dv;
				a = clampf((u2-v) / dv, 0, 1);

				if (a < 1.0f)
					drawSpinner(vg, tx+thumb/2,ty+thumb/2, thumb*0.25f, t);

				imgPaint = vg.ImagePattern(tx+ix, ty+iy, iw,ih, 0.0f/180.0f*MathF.PI, images[i], a);
				vg.BeginPath();
				vg.RoundedRect(tx,ty, thumb,thumb, 5);
				vg.FillPaint(imgPaint);
				vg.Fill();

				shadowPaint = vg.BoxGradient(tx-1,ty, thumb+2,thumb+2, 5, 3, NVG.RGBA(0,0,0,128), NVG.RGBA(0,0,0,0));
				vg.BeginPath();
				vg.Rect(tx-5,ty-5, thumb+10,thumb+10);
				vg.RoundedRect(tx,ty, thumb,thumb, 6);
				vg.PathWinding((int)NVGsolidity.NVG_HOLE);
				vg.FillPaint(shadowPaint);
				vg.Fill();

				vg.BeginPath();
				vg.RoundedRect(tx+0.5f,ty+0.5f, thumb-1,thumb-1, 4-0.5f);
				vg.StrokeWidth(1.0f);
				vg.StrokeColor(NVG.RGBA(255,255,255,192));
				vg.Stroke();
			}
			vg.Restore();

			// Hide fades
			fadePaint = vg.LinearGradient(x,y,x,y+6, NVG.RGBA(200,200,200,255), NVG.RGBA(200,200,200,0));
			vg.BeginPath();
			vg.Rect(x+4,y,w-8,6);
			vg.FillPaint(fadePaint);
			vg.Fill();

			fadePaint = vg.LinearGradient(x,y+h,x,y+h-6, NVG.RGBA(200,200,200,255), NVG.RGBA(200,200,200,0));
			vg.BeginPath();
			vg.Rect(x+4,y+h-6,w-8,6);
			vg.FillPaint(fadePaint);
			vg.Fill();

			// Scroll bar
			shadowPaint = vg.BoxGradient(x+w-12+1,y+4+1, 8,h-8, 3,4, NVG.RGBA(0,0,0,32), NVG.RGBA(0,0,0,92));
			vg.BeginPath();
			vg.RoundedRect(x+w-12,y+4, 8,h-8, 3);
			vg.FillPaint(shadowPaint);
		//	vg.FillColor(NVG.RGBA(255,0,0,128));
			vg.Fill();

			scrollh = (h/stackh) * (h-8);
			shadowPaint = vg.BoxGradient(x+w-12-1,y+4+(h-8-scrollh)*u-1, 8,scrollh, 3,4, NVG.RGBA(220,220,220,255), NVG.RGBA(128,128,128,255));
			vg.BeginPath();
			vg.RoundedRect(x+w-12+1,y+4+1 + (h-8-scrollh)*u, 8-2,scrollh-2, 2);
			vg.FillPaint(shadowPaint);
		//	vg.FillColor(NVG.RGBA(0,0,0,128));
			vg.Fill();

			vg.Restore();
		}

		public static void drawColorwheel(NVGcontext vg, float x, float y, float w, float h, float t)
		{
			int i;
			float r0, r1, ax,ay, bx,by, cx,cy, aeps, r;
			float hue = MathF.Sin(t * 0.12f);
			NVGpaint paint;

			vg.Save();

		/*	vg.BeginPath();
			vg.Rect(x,y,w,h);
			vg.FillColor(NVG.RGBA(255,0,0,128));
			vg.Fill();*/

			cx = x + w*0.5f;
			cy = y + h*0.5f;
			r1 = (w < h ? w : h) * 0.5f - 5.0f;
			r0 = r1 - 20.0f;
			aeps = 0.5f / r1;	// half a pixel arc length in radians (2pi cancels out).

			for (i = 0; i < 6; i++) {
				float a0 = (float)i / 6.0f * MathF.PI * 2.0f - aeps;
				float a1 = (float)(i+1.0f) / 6.0f * MathF.PI * 2.0f + aeps;
				vg.BeginPath();
				vg.Arc(cx,cy, r0, a0, a1, (int)NVGwinding.NVG_CW);
				vg.Arc(cx,cy, r1, a1, a0, (int)NVGwinding.NVG_CCW);
				vg.ClosePath();
				ax = cx + MathF.Cos(a0) * (r0+r1)*0.5f;
				ay = cy + MathF.Sin(a0) * (r0+r1)*0.5f;
				bx = cx + MathF.Cos(a1) * (r0+r1)*0.5f;
				by = cy + MathF.Sin(a1) * (r0+r1)*0.5f;
				paint = vg.LinearGradient(ax,ay, bx,by, NVG.HSLA(a0/(MathF.PI*2),1.0f,0.55f,255), NVG.HSLA(a1/(MathF.PI*2),1.0f,0.55f,255));
				vg.FillPaint(paint);
				vg.Fill();
			}

			vg.BeginPath();
			vg.Circle(cx,cy, r0-0.5f);
			vg.Circle(cx,cy, r1+0.5f);
			vg.StrokeColor(NVG.RGBA(0,0,0,64));
			vg.StrokeWidth(1.0f);
			vg.Stroke();

			// Selector
			vg.Save();
			vg.Translate(cx,cy);
			vg.Rotate(hue*MathF.PI*2);

			// Marker on
			vg.StrokeWidth(2.0f);
			vg.BeginPath();
			vg.Rect(r0-1,-3,r1-r0+2,6);
			vg.StrokeColor(NVG.RGBA(255,255,255,192));
			vg.Stroke();

			paint = vg.BoxGradient(r0-3,-5,r1-r0+6,10, 2,4, NVG.RGBA(0,0,0,128), NVG.RGBA(0,0,0,0));
			vg.BeginPath();
			vg.Rect(r0-2-10,-4-10,r1-r0+4+20,8+20);
			vg.Rect(r0-2,-4,r1-r0+4,8);
			vg.PathWinding((int)NVGsolidity.NVG_HOLE);
			vg.FillPaint(paint);
			vg.Fill();

			// Center triangle
			r = r0 - 6;
			ax = MathF.Cos(120.0f/180.0f*MathF.PI) * r;
			ay = MathF.Sin(120.0f/180.0f*MathF.PI) * r;
			bx = MathF.Cos(-120.0f/180.0f*MathF.PI) * r;
			by = MathF.Sin(-120.0f/180.0f*MathF.PI) * r;
			vg.BeginPath();
			vg.MoveTo(r,0);
			vg.LineTo(ax,ay);
			vg.LineTo(bx,by);
			vg.ClosePath();
			paint = vg.LinearGradient(r,0, ax,ay, NVG.HSLA(hue,1.0f,0.5f,255), NVG.RGBA(255,255,255,255));
			vg.FillPaint(paint);
			vg.Fill();
			paint = vg.LinearGradient((r+ax)*0.5f,(0+ay)*0.5f, bx,by, NVG.RGBA(0,0,0,0), NVG.RGBA(0,0,0,255));
			vg.FillPaint(paint);
			vg.Fill();
			vg.StrokeColor(NVG.RGBA(0,0,0,64));
			vg.Stroke();

			// Select circle on triangle
			ax = MathF.Cos(120.0f/180.0f*MathF.PI) * r*0.3f;
			ay = MathF.Sin(120.0f/180.0f*MathF.PI) * r*0.4f;
			vg.StrokeWidth(2.0f);
			vg.BeginPath();
			vg.Circle(ax,ay,5);
			vg.StrokeColor(NVG.RGBA(255,255,255,192));
			vg.Stroke();

			paint = vg.RadialGradient(ax,ay, 7,9, NVG.RGBA(0,0,0,64), NVG.RGBA(0,0,0,0));
			vg.BeginPath();
			vg.Rect(ax-20,ay-20,40,40);
			vg.Circle(ax,ay,7);
			vg.PathWinding((int)NVGsolidity.NVG_HOLE);
			vg.FillPaint(paint);
			vg.Fill();

			vg.Restore();

			vg.Restore();
		}

		public static void drawLines(NVGcontext vg, float x, float y, float w, float h, float t)
		{
			int i, j;
			float pad = 5.0f, s = w/9.0f - pad*2;
			float[] pts = new float[4*2];
			float fx, fy;
			int[] joins = new int[] { (int)NVGlineCap.NVG_MITER, (int)NVGlineCap.NVG_ROUND, (int)NVGlineCap.NVG_BEVEL };
			int[] caps = new int[] { (int)NVGlineCap.NVG_BUTT, (int)NVGlineCap.NVG_ROUND, (int)NVGlineCap.NVG_SQUARE };

			vg.Save();
			pts[0] = -s*0.25f + MathF.Cos(t*0.3f) * s*0.5f;
			pts[1] = MathF.Sin(t*0.3f) * s*0.5f;
			pts[2] = -s*0.25f;
			pts[3] = 0;
			pts[4] = s*0.25f;
			pts[5] = 0;
			pts[6] = s*0.25f + MathF.Cos(-t*0.3f) * s*0.5f;
			pts[7] = MathF.Sin(-t*0.3f) * s*0.5f;

			for (i = 0; i < 3; i++) {
				for (j = 0; j < 3; j++) {
					fx = x + s*0.5f + (i*3+j)/9.0f*w + pad;
					fy = y - s*0.5f + pad;

					vg.LineCap(caps[i]);
					vg.LineJoin(joins[j]);

					vg.StrokeWidth(s*0.3f);
					vg.StrokeColor(NVG.RGBA(0,0,0,160));
					vg.BeginPath();
					vg.MoveTo(fx+pts[0], fy+pts[1]);
					vg.LineTo(fx+pts[2], fy+pts[3]);
					vg.LineTo(fx+pts[4], fy+pts[5]);
					vg.LineTo(fx+pts[6], fy+pts[7]);
					vg.Stroke();

					vg.LineCap((int)NVGlineCap.NVG_BUTT);
					vg.LineJoin((int)NVGlineCap.NVG_BEVEL);

					vg.StrokeWidth(1.0f);
					vg.StrokeColor(NVG.RGBA(0,192,255,255));
					vg.BeginPath();
					vg.MoveTo(fx+pts[0], fy+pts[1]);
					vg.LineTo(fx+pts[2], fy+pts[3]);
					vg.LineTo(fx+pts[4], fy+pts[5]);
					vg.LineTo(fx+pts[6], fy+pts[7]);
					vg.Stroke();
				}
			}


			vg.Restore();
		}

		public static DemoData loadDemoData(NVGcontext vg)
		{
			int i;

			if (vg.Handle == IntPtr.Zero)
				return null;

			DemoData data = new DemoData();
			data.images = new int[12];

			for (i = 0; i < data.images.Length; i++) {;
				string file = $"./example/images/image{i+1}.jpg";
				data.images[i] = vg.CreateImage(file, 0);

				if (data.images[i] == 0) {
					Console.WriteLine("Could not load %s.\n", file);
					return null;
				}
			}

			data.fontIcons = vg.CreateFont("icons", "./example/entypo.ttf");
			if (data.fontIcons == -1) {
				Console.WriteLine("Could not add font icons.\n");
				return null;
			}
			data.fontNormal = vg.CreateFont("sans", "./example/Roboto-Regular.ttf");
			if (data.fontNormal == -1) {
				Console.WriteLine("Could not add font italic.\n");
				return null;
			}
			data.fontBold = vg.CreateFont("sans-bold", "./example/Roboto-Bold.ttf");
			if (data.fontBold == -1) {
				Console.WriteLine("Could not add font bold.\n");
				return null;
			}
			data.fontEmoji = vg.CreateFont("emoji", "./example/NotoEmoji-Regular.ttf");
			if (data.fontEmoji == -1) {
				Console.WriteLine("Could not add font emoji.\n");
				return null;
			}

			vg.AddFallbackFontId(data.fontNormal, data.fontEmoji);
			vg.AddFallbackFontId(data.fontBold, data.fontEmoji);

			return data;
		}

		public static void freeDemoData(NVGcontext vg, DemoData data)
		{
			int i;

			if (vg.Handle == IntPtr.Zero)
				return;

			for (i = 0; i < 12; i++)
				vg.DeleteImage(data.images[i]);
		}

		public static void drawParagraph(NVGcontext vg, float x, float y, float width, float height, float mx, float my)
		{
			/*NVGtextRow[] rows = new NVGtextRow[3];
			NVGglyphPosition[] glyphs = new NVGglyphPosition[100];
			string text = "This is longer chunk of text.\n  \n  Would have used lorem ipsum but she    was busy jumping over the lazy dog with the fox and all the men who came to the aid of the party.🎉";
			string start;
			string end;
			int nrows, i, nglyphs, j, lnum = 0;
			float lineh;
			float caretx, px;
			float[] bounds = new float[4];
			float a;
			string hoverText = "Hover your mouse over the text to see calculated caret position.";
			float gx,gy;
			int gutter = 0;
			string boxText = "Testing\nsome multiline\ntext.";

			vg.Save();

			vg.FontSize(15.0f);
			vg.FontFace("sans");
			vg.TextAlign((int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_TOP));
			vg.TextMetrics(null, null, &lineh);

			// The text break API can be used to fill a large buffer of rows,
			// or to iterate over the text just few lines (or just one) at a time.
			// The "next" variable of the last returned item tells where to continue.
			start = text;
			end = text + strlen(text);
			while ((nrows = vg.TextBreakLines(start, end, width, rows, 3))) {
				for (i = 0; i < nrows; i++) {
					NVGtextRow* row = &rows[i];
					int hit = mx > x && mx < (x+width) && my >= y && my < (y+lineh);

					vg.BeginPath();
					vg.FillColor(NVG.RGBA(255,255,255,hit?64:16));
					vg.Rect(x + row->minx, y, row->maxx - row->minx, lineh);
					vg.Fill();

					vg.FillColor(NVG.RGBA(255,255,255,255));
					vg.Text(x, y, row->start, row->end);

					if (hit) {
						caretx = (mx < x+row->width/2) ? x : x+row->width;
						px = x;
						nglyphs = vg.TextGlyphPositions(x, y, row->start, row->end, glyphs, 100);
						for (j = 0; j < nglyphs; j++) {
							float x0 = glyphs[j].x;
							float x1 = (j+1 < nglyphs) ? glyphs[j+1].x : x+row->width;
							float gx = x0 * 0.3f + x1 * 0.7f;
							if (mx >= px && mx < gx)
								caretx = glyphs[j].x;
							px = gx;
						}
						vg.BeginPath();
						vg.FillColor(NVG.RGBA(255,192,0,255));
						vg.Rect(caretx, y, 1, lineh);
						vg.Fill();

						gutter = lnum+1;
						gx = x - 10;
						gy = y + lineh/2;
					}
					lnum++;
					y += lineh;
				}
				// Keep going...
				start = rows[nrows-1].next;
			}

			if (gutter != 0) {
				char txt[16];
				snprintf(txt, sizeof(txt), "%d", gutter);
				vg.FontSize(12.0f);
				vg.TextAlign((int)(NVGalign.NVG_ALIGN_RIGHT | NVGalign.NVG_ALIGN_MIDDLE));

				vg.TextBounds(gx,gy, txt, null, bounds);

				vg.BeginPath();
				vg.FillColor(NVG.RGBA(255,192,0,255));
				vg.RoundedRect((int)bounds[0]-4,(int)bounds[1]-2, (int)(bounds[2]-bounds[0])+8, (int)(bounds[3]-bounds[1])+4, ((int)(bounds[3]-bounds[1])+4)/2-1);
				vg.Fill();

				vg.FillColor(NVG.RGBA(32,32,32,255));
				vg.Text(gx,gy, txt, null);
			}

			y += 20.0f;

			vg.FontSize(11.0f);
			vg.TextAlign((int)(NVGalign.NVG_ALIGN_LEFT | NVGalign.NVG_ALIGN_TOP));
			vg.TextLineHeight(1.2f);

			vg.TextBoxBounds(x,y, 150, hoverText, null, bounds);

			// Fade the tooltip out when close to it.
			gx = clampf(mx, bounds[0], bounds[2]) - mx;
			gy = clampf(my, bounds[1], bounds[3]) - my;
			a = MathF.Sqrt(gx*gx + gy*gy) / 30.0f;
			a = clampf(a, 0, 1);
			vg.GlobalAlpha(a);

			vg.BeginPath();
			vg.FillColor(NVG.RGBA(220,220,220,255));
			vg.RoundedRect(bounds[0]-2,bounds[1]-2, (int)(bounds[2]-bounds[0])+4, (int)(bounds[3]-bounds[1])+4, 3);
			px = (int)((bounds[2]+bounds[0])/2);
			vg.MoveTo(px,bounds[1] - 10);
			vg.LineTo(px+7,bounds[1]+1);
			vg.LineTo(px-7,bounds[1]+1);
			vg.Fill();

			vg.FillColor(NVG.RGBA(0,0,0,220));
			vg.TextBox(x,y, 150, hoverText, null);

			vg.Restore();*/
		}

		public static void drawWidths(NVGcontext vg, float x, float y, float width)
		{
			int i;

			vg.Save();

			vg.StrokeColor(NVG.RGBA(0,0,0,255));

			for (i = 0; i < 20; i++) {
				float w = (i+0.5f)*0.1f;
				vg.StrokeWidth(w);
				vg.BeginPath();
				vg.MoveTo(x,y);
				vg.LineTo(x+width,y+width*0.3f);
				vg.Stroke();
				y += 10;
			}

			vg.Restore();
		}

		public static void drawCaps(NVGcontext vg, float x, float y, float width)
		{
			int i;
			int[] caps = new int[] { (int)NVGlineCap.NVG_BUTT, (int)NVGlineCap.NVG_ROUND, (int)NVGlineCap.NVG_SQUARE };
			float lineWidth = 8.0f;

			vg.Save();

			vg.BeginPath();
			vg.Rect(x-lineWidth/2, y, width+lineWidth, 40);
			vg.FillColor(NVG.RGBA(255,255,255,32));
			vg.Fill();

			vg.BeginPath();
			vg.Rect(x, y, width, 40);
			vg.FillColor(NVG.RGBA(255,255,255,32));
			vg.Fill();

			vg.StrokeWidth(lineWidth);
			for (i = 0; i < 3; i++) {
				vg.LineCap(caps[i]);
				vg.StrokeColor(NVG.RGBA(0,0,0,255));
				vg.BeginPath();
				vg.MoveTo(x, y + i*10 + 5);
				vg.LineTo(x+width, y + i*10 + 5);
				vg.Stroke();
			}

			vg.Restore();
		}

		public static void drawScissor(NVGcontext vg, float x, float y, float t)
		{
			vg.Save();

			// Draw first rect and set scissor to it's area.
			vg.Translate(x, y);
			vg.Rotate(NVG.DegToRad(5));
			vg.BeginPath();
			vg.Rect(-20,-20,60,40);
			vg.FillColor(NVG.RGBA(255,0,0,255));
			vg.Fill();
			vg.Scissor(-20,-20,60,40);

			// Draw second rectangle with offset and rotation.
			vg.Translate(40,0);
			vg.Rotate(t);

			// Draw the intended second rectangle without any scissoring.
			vg.Save();
			vg.ResetScissor();
			vg.BeginPath();
			vg.Rect(-20,-10,60,30);
			vg.FillColor(NVG.RGBA(255,128,0,64));
			vg.Fill();
			vg.Restore();

			// Draw second rectangle with combined scissoring.
			vg.IntersectScissor(-20,-10,60,30);
			vg.BeginPath();
			vg.Rect(-20,-10,60,30);
			vg.FillColor(NVG.RGBA(255,128,0,255));
			vg.Fill();

			vg.Restore();
		}

		public static void renderDemo(NVGcontext vg, float mx, float my, float width, float height, float t, bool blowup, DemoData data)
		{
			float x,y,popy;

			drawEyes(vg, width - 250, 50, 150, 100, mx, my, t);
			drawParagraph(vg, width - 450, 50, 150, 100, mx, my);
			drawGraph(vg, 0, height/2, width, height/2, t);
			drawColorwheel(vg, width - 300, height - 300, 250.0f, 250.0f, t);

			// Line joints
			drawLines(vg, 120, height-50, 600, 50, t);

			// Line caps
			drawWidths(vg, 10, 50, 30);

			// Line caps
			drawCaps(vg, 10, 300, 30);

			drawScissor(vg, 50, height-80, t);

			vg.Save();
			if (blowup) {
				vg.Rotate(MathF.Sin(t*0.3f)*5.0f/180.0f*MathF.PI);
				vg.Scale(2.0f, 2.0f);
			}

			// Widgets
			drawWindow(vg, "Widgets `n Stuff", 50, 50, 300, 400);
			x = 60; y = 95;
			drawSearchBox(vg, "Search", x,y,280,25);
			y += 40;
			drawDropDown(vg, "Effects", x,y,280,28);
			popy = y + 14;
			y += 45;

			// Form
			drawLabel(vg, "Login", x,y, 280,20);
			y += 25;
			drawEditBox(vg, "Email",  x,y, 280,28);
			y += 35;
			drawEditBox(vg, "Password", x,y, 280,28);
			y += 38;
			drawCheckBox(vg, "Remember me", x,y, 140,28);
			drawButton(vg, ICON_LOGIN, "Sign in", x+138, y, 140, 28, NVG.RGBA(0,96,128,255));
			y += 45;

			// Slider
			drawLabel(vg, "Diameter", x,y, 280,20);
			y += 25;
			drawEditBoxNum(vg, "123.00", "px", x+180,y, 100,28);
			drawSlider(vg, 0.4f, x,y, 170,28);
			y += 55;

			drawButton(vg, ICON_TRASH, "Delete", x, y, 160, 28, NVG.RGBA(128,16,8,255));
			drawButton(vg, 0, "Cancel", x+170, y, 110, 28, NVG.RGBA(0,0,0,0));

			// Thumbnails box
			drawThumbnails(vg, 365, popy-30, 160, 300, data.images, 12, t);

			vg.Restore();
		}

		static int mini(int a, int b) { return a < b ? a : b; }

		/*static void unpremultiplyAlpha(unsigned char* image, int w, int h, int stride)
		{
			int x,y;

			// Unpremultiply
			for (y = 0; y < h; y++) {
				unsigned char *row = &image[y*stride];
				for (x = 0; x < w; x++) {
					int r = row[0], g = row[1], b = row[2], a = row[3];
					if (a != 0) {
						row[0] = (int)mini(r*255/a, 255);
						row[1] = (int)mini(g*255/a, 255);
						row[2] = (int)mini(b*255/a, 255);
					}
					row += 4;
				}
			}

			// Defringe
			for (y = 0; y < h; y++) {
				unsigned char *row = &image[y*stride];
				for (x = 0; x < w; x++) {
					int r = 0, g = 0, b = 0, a = row[3], n = 0;
					if (a == 0) {
						if (x-1 > 0 && row[-1] != 0) {
							r += row[-4];
							g += row[-3];
							b += row[-2];
							n++;
						}
						if (x+1 < w && row[7] != 0) {
							r += row[4];
							g += row[5];
							b += row[6];
							n++;
						}
						if (y-1 > 0 && row[-stride+3] != 0) {
							r += row[-stride];
							g += row[-stride+1];
							b += row[-stride+2];
							n++;
						}
						if (y+1 < h && row[stride+3] != 0) {
							r += row[stride];
							g += row[stride+1];
							b += row[stride+2];
							n++;
						}
						if (n > 0) {
							row[0] = r/n;
							row[1] = g/n;
							row[2] = b/n;
						}
					}
					row += 4;
				}
			}
		}

		static void setAlpha(unsigned char* image, int w, int h, int stride, unsigned char a)
		{
			int x, y;
			for (y = 0; y < h; y++) {
				unsigned char* row = &image[y*stride];
				for (x = 0; x < w; x++)
					row[x*4+3] = a;
			}
		}

		static void flipHorizontal(unsigned char* image, int w, int h, int stride)
		{
			int i = 0, j = h-1, k;
			while (i < j) {
				unsigned char* ri = &image[i * stride];
				unsigned char* rj = &image[j * stride];
				for (k = 0; k < w*4; k++) {
					unsigned char t = ri[k];
					ri[k] = rj[k];
					rj[k] = t;
				}
				i++;
				j--;
			}
		}

		public static void saveScreenShot(int w, int h, int premult, string name)
		{
			unsigned char* image = (unsigned char*)malloc(w*h*4);
			if (image == null)
				return;
			glReadPixels(0, 0, w, h, GL_RGBA, GL_UNSIGNED_BYTE, image);
			if (premult)
				unpremultiplyAlpha(image, w, h, w*4);
			else
				setAlpha(image, w, h, w*4, 255);
			flipHorizontal(image, w, h, w*4);
 			stbi_write_png(name, w, h, 4, image, w*4);
 			free(image);
		}*/
    }
}
