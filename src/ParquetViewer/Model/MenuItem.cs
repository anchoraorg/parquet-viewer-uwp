using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace ParquetViewer.Model
{
   //https://docs.microsoft.com/en-gb/windows/uwp/design/style/segoe-ui-symbol-font
   public class MenuItem
   {
      public string Glyph { get; set; }
      public string Label { get; set; }
      public string Action { get; set; }

      public static List<MenuItem> GetMainItems()
      {
         var items = new List<MenuItem>();
         items.Add(new MenuItem() { Glyph = "\uE8DA", Label = "Open File", Action = "open" });
         items.Add(new MenuItem() { Glyph = "\uE8A7", Label = "Report Issue...", Action = "issue" });
         return items;
      }

      public static List<MenuItem> GetOptionsItems()
      {
         var items = new List<MenuItem>();
         items.Add(new MenuItem() { Glyph = "\uE897", Label = "About", Action = "about" });
         return items;
      }
   }
}