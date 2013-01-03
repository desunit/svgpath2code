// Authors:
//	Sebastien Pouliot  <sebastien@xamarin.com>
//
// Copyright 2012 Xamarin Inc.
//
// Licensed under the GNU LGPL 2 license only (no "later versions")

using System;
using System.Collections.Generic;
using System.IO;
using Poupou.SvgPathConverter;

// This sample shows how you can use the library to converts every SVG path inside FontAwesome into a MonoTouch.Dialog
// based application to show them all. Since MonoTouch uses C# and iOS is CoreGraphics based then the parameters (and
// the extra code generation) are hardcoded inside the sample.

class Program {

	static void Usage (string error, params string[] values)
	{
		Console.WriteLine ("Usage: convert-font-awesome <font-directory> <monotouch|android> [generated-file.cs]");

		if (error != null)
			Console.WriteLine (error, values);
		Environment.Exit (1);
	}

	static ISourceFormatter CreateFormatter (string type, TextWriter writer)
	{
		switch (type) {
		case "monotouch":
			return new CSharpCoreGraphicsFormatter(writer);
		case "android":
			return new AndroidFormatter(writer);
		default:
			Usage("Supported types: monotouch, android.");
			return null;
		}
	}

	public static int Main (string[] args)
	{
		if (args.Length < 1)
			Usage ("error: Path to FontAwesome directory required");

		string font_dir = args [0];
		string css_file = Path.Combine (font_dir, "css/font-awesome.css");
		if (!File.Exists (css_file))
			Usage ("error: Missing '{0}' file.", css_file);

		string svg_file = Path.Combine (font_dir, "font/fontawesome-webfont.svg");
		if (!File.Exists (svg_file))
			Usage ("error: Missing '{0}' file.", svg_file);

		if (args.Length < 2)
			Usage ("error: Specify formatter");


		TextWriter writer = (args.Length < 3) ? Console.Out : new StreamWriter (args [2]);
		var code = CreateFormatter(args[1], writer);

		var parser = new SvgPathParser () {
			Formatter = code
		};

		code.Header();

		Console.WriteLine("Parsing icons");

		Dictionary<string,string> names = new Dictionary<string,string> ();
		foreach (string line in File.ReadLines (css_file)) {
			if (!line.StartsWith (".icon-", StringComparison.Ordinal))
				continue;
			int p = line.IndexOf (':');
			if (p == -1) 
				continue;
			string name = line.Substring (1, p - 1).Replace ('-', '_');
			p = line.IndexOf ("content: \"\\", StringComparison.Ordinal);
			if (p == -1)
				continue;
			string value = line.Substring (p + 11, 4);
			code.NewElement (name, value);
			names.Add (value, name);
		}

		code.ElementStats(names.Count);
		Console.WriteLine("Parsing glyphs");

		foreach (string line in File.ReadLines (svg_file)) {
			if (!line.StartsWith ("<glyph unicode=\"&#x", StringComparison.Ordinal))
				continue;
			string id = line.Substring (19, 4);
			string name;
			if (!names.TryGetValue (id, out name))
				continue;
			int p = line.IndexOf (" d=\"") + 4;
			int e = line.LastIndexOf ('"');
			string data = line.Substring (p, e - p);
			parser.Parse (data, name);
		}

		code.Footer();

		return 0;
	}
}