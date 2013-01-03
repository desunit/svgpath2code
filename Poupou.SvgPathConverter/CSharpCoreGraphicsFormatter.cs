// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2012 Xamarin Inc.
//
// Licensed under the GNU LGPL 2 license only (no "later versions")

using System;
using System.Drawing;
using System.IO;

namespace Poupou.SvgPathConverter {

	public class CSharpCoreGraphicsFormatter : ISourceFormatter {

		TextWriter writer;

		public CSharpCoreGraphicsFormatter (TextWriter textWriter)
		{
			writer = textWriter;
		}

		public void Header()
		{
			writer.WriteLine ("// note: Generated file - do not modify - use convert-font-awesome to regenerate");
			writer.WriteLine ();
			writer.WriteLine ("using MonoTouch.CoreGraphics;");
			writer.WriteLine ("using MonoTouch.Dialog;");
			writer.WriteLine ("using MonoTouch.Foundation;");
			writer.WriteLine ("using MonoTouch.UIKit;");
			writer.WriteLine ();
			writer.WriteLine ("namespace Poupou.Awesome.Demo {");
			writer.WriteLine ();
			writer.WriteLine ("\t[Preserve]");
			writer.WriteLine ("\tpublic partial class Elements {");
		}

		public void ElementStats (int count)
		{
			writer.WriteLine ("\t\t// total: {0}", count);
			writer.WriteLine ();
		}

		public void Footer()
		{
			writer.WriteLine ("\t}");
			writer.WriteLine ("}");
			writer.Close ();
		}

		public void NewElement (string name, string value)
		{
			writer.WriteLine ("\t\t// {0} : {1}", name, value);
			writer.WriteLine ("\t\tImageStringElement {0}_element = new ImageStringElement (\"{0}\", GetAwesomeIcon ({0}));", name);
			writer.WriteLine ();
		}
		
		public void Prologue (string name)
		{
			writer.WriteLine ("\tstatic void {0} (CGContext c)", name);
			writer.WriteLine ("\t{");
		}
	
		public void Epilogue ()
		{
			writer.WriteLine ("\t\tc.FillPath ();");
			writer.WriteLine ("\t\tc.StrokePath ();");
			writer.WriteLine ("\t}");
			writer.WriteLine ();
		}
	
		public void MoveTo (PointF pt)
		{
			writer.WriteLine ("\t\tc.MoveTo ({0}f, {1}f);", pt.X, pt.Y);
		}
	
		public void LineTo (PointF pt)
		{
			writer.WriteLine ("\t\tc.AddLineToPoint ({0}f, {1}f);", pt.X, pt.Y);
		}
	
		public void ClosePath ()
		{
			writer.WriteLine ("\t\tc.ClosePath ();");
		}
	
		public void QuadCurveTo (PointF pt1, PointF pt2)
		{
			writer.WriteLine ("\t\tc.AddQuadCurveToPoint ({0}f, {1}f, {2}f, {3}f);", pt1.X, pt1.Y, pt2.X, pt2.Y);
		}

		public void CurveTo (PointF pt1, PointF pt2, PointF pt3)
		{
			writer.WriteLine ("\t\tc.AddCurveToPoint ({0}f, {1}f, {2}f, {3}f, {4}f, {5}f);", 
				pt1.X, pt1.Y, pt2.X, pt2.Y, pt3.X, pt3.Y);
		}

		public void ArcTo (PointF size, float angle, bool isLarge, bool sweep, PointF endPoint, PointF startPoint)
		{
			this.ArcHelper (size, angle, isLarge, sweep, endPoint, startPoint);
		}
	}
}