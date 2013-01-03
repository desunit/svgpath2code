using System;
using System.Drawing;
using System.IO;

namespace Poupou.SvgPathConverter {

	public class AndroidFormatter : ISourceFormatter {

		TextWriter writer;

		public AndroidFormatter (TextWriter textWriter)
		{
			writer = textWriter;
		}

		public void Header()
		{
			writer.WriteLine ("// note: Generated file - do not modify - use convert-font-awesome to regenerate");
			writer.WriteLine ();

			writer.WriteLine ("package com.desunit;");
			writer.WriteLine ();
			writer.WriteLine ("import android.graphics.Canvas;");
			writer.WriteLine ("import android.graphics.Paint;");
			writer.WriteLine ("import android.graphics.Path;");
			writer.WriteLine ("import android.graphics.Path.FillType;");
			writer.WriteLine ();
			writer.WriteLine ("public class VectorImages {");
		}
		
		public void ElementStats (int count)
		{
			writer.WriteLine ("\t// total: {0}", count);
			writer.WriteLine ();
		}
		
		public void Footer()
		{
			writer.WriteLine ("}");
			writer.Close ();
		}
		
		public void NewElement (string name, string value)
		{
		}
		
		public void Prologue (string name)
		{
			writer.WriteLine ("\tpublic static void {0} (Canvas c, Paint paint)", name);
			writer.WriteLine ("\t{");
			writer.WriteLine ("\t\tPath p = new Path();");

		}
	
		public void Epilogue ()
		{
			writer.WriteLine ("\t\tp.setFillType(FillType.WINDING);");
			writer.WriteLine ("\t\tc.drawPath(p, paint);");
			writer.WriteLine ("\t}");
			writer.WriteLine ();
		}
	
		public void MoveTo (PointF pt)
		{
			writer.WriteLine ("\t\tp.moveTo ({0}f, {1}f);", pt.X, pt.Y);
		}
	
		public void LineTo (PointF pt)
		{
			writer.WriteLine ("\t\tp.lineTo ({0}f, {1}f);", pt.X, pt.Y);
		}
	
		public void ClosePath ()
		{
			writer.WriteLine ("\t\tp.close ();");
		}
	
		public void QuadCurveTo (PointF pt1, PointF pt2)
		{
			writer.WriteLine ("\t\tp.quadTo ({0}f, {1}f, {2}f, {3}f);", pt1.X, pt1.Y, pt2.X, pt2.Y);
		}

		public void CurveTo (PointF pt1, PointF pt2, PointF pt3)
		{
			writer.WriteLine ("\t\tp.cubicTo ({0}f, {1}f, {2}f, {3}f, {4}f, {5}f);", 
				pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y);
		}

		public void ArcTo (PointF size, float angle, bool isLarge, bool sweep, PointF endPoint, PointF startPoint)
		{
			this.ArcHelper (size, angle, isLarge, sweep, endPoint, startPoint);
		}
	}
}