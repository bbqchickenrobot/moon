using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using Mono.Moonlight.UnitTesting;

namespace MoonTest.System.Windows
{
	[TestClass]
	public class ResourceDictionaryTest
	{

		[TestMethod]
		public void AddDouble ()
		{
			Button b = new Button();
			ResourceDictionary rd = b.Resources;

			rd.Add ("hi", 5.0);
		}

		[TestMethod]
		public void AddObject ()
		{
			Button b = new Button();
			ResourceDictionary rd = b.Resources;

			rd.Add ("hi", new object());
		}

		[TestMethod]
		public void AddDuplicate ()
		{
			Button b = new Button();
			ResourceDictionary rd = b.Resources;

			rd.Add ("hi", new object());
			Assert.Throws (delegate { rd.Add ("hi", new object()); 
				},
				typeof (ArgumentException));
		}

		[TestMethod]
		public void TestParseColor ()
		{
			Canvas b = (Canvas)
				XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><Color x:Key=""color"">#ffffffff</Color></Canvas.Resources></Canvas>");

			Assert.IsNotNull (b.Resources["color"], "1");

			Color c = (Color)b.Resources["color"];

			Assert.AreEqual (Color.FromArgb (0xff, 0xff, 0xff, 0xff), b.Resources["color"], "2");
		}

		[TestMethod]
		public void TestParseDouble ()
		{
			Assert.Throws (delegate {
					XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><Double x:Key=""double"">5.0</Double></Canvas.Resources></Canvas>");
				},
				typeof (XamlParseException));
		}

		[TestMethod]
		public void Parse_MissingxKey ()
		{
			Assert.Throws (delegate { 
					XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><Color>#ffffffff</Color></Canvas.Resources></Canvas>");
				},
				typeof (XamlParseException));
		}

		[TestMethod]
		public void Parse_MissingxKey_WithxName ()
		{
			Canvas b = (Canvas)
				XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><Color x:Name=""color"">#ffffffff</Color></Canvas.Resources></Canvas>");


			Color c = (Color)b.Resources["color"];

			Assert.AreEqual (Color.FromArgb (0xff, 0xff, 0xff, 0xff), b.Resources["color"]);

		}

		[TestMethod]
		public void Parse_BothxKeyAndxName ()
		{
			Assert.Throws (delegate { XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><Color x:Key=""keycolor"" x:Name=""color"">#ffffffff</Color></Canvas.Resources></Canvas>");
				},
				typeof (XamlParseException));
		}

		[TestMethod]
		public void TestIntegerIndex ()
		{
			Canvas b = (Canvas)
				XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><Color x:Key=""color"">#ffffffff</Color></Canvas.Resources></Canvas>");

			Assert.Throws (delegate { Color c = (Color)b.Resources[0]; },
				       typeof (ArgumentException));
		}

		[TestMethod]
		public void RemoveTest ()
		{
			Canvas b;

			b = (Canvas)XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><Color x:Key=""color"">#ffffffff</Color></Canvas.Resources></Canvas>");

			IDictionary<object, object> oo = (IDictionary<object, object>)b.Resources;
			Assert.IsTrue (oo.Remove ("color"), "1");
			Assert.IsFalse (oo.Remove ("color"), "2");

			b = (Canvas)XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><Color x:Key=""color"">#ffffffff</Color></Canvas.Resources></Canvas>");

			ICollection<KeyValuePair<object, object>> koo = (ICollection<KeyValuePair<object, object>>)b.Resources;
			Assert.IsFalse (oo.Remove ("color"), "3");
			Assert.IsFalse (oo.Remove ("color"), "4");

			b = (Canvas)XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><Color x:Key=""color"">#ffffffff</Color></Canvas.Resources></Canvas>");

			b.Resources.Remove ("color");
			b.Resources.Remove ("color");
		}

		[TestMethod]
		public void ContainsTest ()
		{
			Canvas b;

			b = (Canvas)XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><Color x:Key=""color"">#ffffffff</Color></Canvas.Resources></Canvas>");

			Assert.IsTrue (b.Resources.Contains ("color"));
			Assert.IsFalse (b.Resources.Contains (new object()));
		}

		[TestMethod]
		public void TestFindName ()
		{
			Canvas b = (Canvas)
				XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><Color x:Key=""color"">#ffffffff</Color><Storyboard x:Key=""sb""/><Storyboard x:Name=""anothersb""/></Canvas.Resources></Canvas>");

			Assert.IsNull (b.FindName ("color"));
			Assert.IsNull (b.FindName ("sb"));
			Assert.IsNotNull (b.FindName ("anothersb"));
			Assert.IsNotNull (b.Resources ["anothersb"]);

			Assert.AreSame (b.FindName ("anothersb"), b.Resources["anothersb"]);
		}

		[TestMethod]
		public void TestNameAndKey ()
		{
			Assert.Throws (delegate {
					XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><Storyboard x:Name=""sb""/><Storyboard x:Key=""sb""/></Canvas.Resources></Canvas>");
				},
				typeof (XamlParseException));
		}

		[TestMethod]
		public void TestCount ()
		{
			Canvas b = (Canvas)
				XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><Color x:Key=""color"">#ffffffff</Color></Canvas.Resources></Canvas>");

			Assert.AreEqual (0, b.Resources.Count);

			b = (Canvas)
				XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><Color x:Name=""color"">#ffffffff</Color></Canvas.Resources></Canvas>");

			Assert.AreEqual (0, b.Resources.Count);
		}

		[TestMethod]
		public void TestxKeyOutsideDictionary ()
		{
			Canvas b = (Canvas)
				XamlReader.Load (@"<Canvas x:Key=""foo"" xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" />");
		}

		[TestMethod]
		public void TestStaticResourceSameElement ()
		{
			Assert.Throws (delegate { XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"" Background=""{StaticResource BackgroundBrush}""><Canvas.Resources><Color x:Key=""color"">#ffffffff</Color><SolidColorBrush x:Key=""BackgroundBrush"" Color=""Black""/></Canvas.Resources></Canvas>");
				},
				typeof (XamlParseException));
		}

		[TestMethod]
		public void TestStaticResourceParentElement_Element ()
		{
			Canvas top = (Canvas)XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><SolidColorBrush x:Key=""FillBrush"" Color=""Black""/></Canvas.Resources><Rectangle x:Name=""child"" Fill=""{StaticResource FillBrush}""/></Canvas>");

			Rectangle child = (Rectangle)top.FindName ("child");
			Assert.AreEqual (Color.FromArgb (0xff, 0x00, 0x00, 0x00), ((SolidColorBrush)child.Fill).Color);
		}

		[TestMethod]
		[KnownFailure]
		public void TestStaticResourceParentElement_Property ()
		{
			Canvas top = (Canvas)XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><Color x:Key=""color"">#ffffffff</Color></Canvas.Resources><Rectangle x:Name=""child""><Rectangle.Stroke><SolidColorBrush Color=""{StaticResource color}""/></Rectangle.Stroke></Rectangle></Canvas>");

			Rectangle child = (Rectangle)top.FindName ("child");
			Assert.AreEqual (Color.FromArgb (0xff, 0xff, 0xff, 0xff), ((SolidColorBrush)child.Stroke).Color);
		}

		[TestMethod]
		public void TestStaticResourceMissingResource ()
		{
			Assert.Throws (delegate { XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><SolidColorBrush x:Key=""FillBrush"" Color=""Black""/></Canvas.Resources><Rectangle x:Name=""child"" Fill=""{StaticResource FillBrush}""><Rectangle.Stroke><SolidColorBrush Color=""{StaticResource color}""/></Rectangle.Stroke></Rectangle></Canvas>");
				},
				typeof (XamlParseException));
		}

		[TestMethod]
		public void TestStaticResourceSyntax ()
		{
			XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><SolidColorBrush x:Key=""FillBrush"" Color=""Black""/></Canvas.Resources><Rectangle x:Name=""child"" Fill=""{ StaticResource    FillBrush }""/></Canvas>");

			Assert.Throws (delegate { XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><SolidColorBrush x:Key=""FillBrush"" Color=""Black""/></Canvas.Resources><Rectangle x:Name=""child"" Fill=""{StaticResource}""/></Canvas>"); },
				       typeof (XamlParseException), "1");

			Assert.Throws (delegate { XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><SolidColorBrush x:Key=""FillBrush"" Color=""Black""/></Canvas.Resources><Rectangle x:Name=""child"" Fill=""{StaticResource }""/></Canvas>"); },
				       typeof (XamlParseException), "2");

			Assert.Throws (delegate { XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><SolidColorBrush x:Key=""FillBrush"" Color=""Black""/></Canvas.Resources><Rectangle x:Name=""child"" Fill="" {StaticResource FillBrush}""/></Canvas>"); },
				       typeof (XamlParseException), "3");

			Assert.Throws (delegate { XamlReader.Load (@"<Canvas xmlns=""http://schemas.microsoft.com/client/2007"" xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""><Canvas.Resources><SolidColorBrush x:Key=""FillBrush"" Color=""Black""/></Canvas.Resources><Rectangle x:Name=""child"" Fill=""{StaticResource FillBrush} ""/></Canvas>"); },
				       typeof (XamlParseException), "4");
		}

	}
}