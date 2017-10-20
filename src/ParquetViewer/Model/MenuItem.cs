using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace ParquetViewer.Model
{
   public class MenuItem
   {
      public string Glyph { get; set; }
      public string Label { get; set; }
      public string Action { get; set; }

      public static List<MenuItem> GetMainItems()
      {
         var items = new List<MenuItem>();
         items.Add(new MenuItem() { Glyph = "\uE8DA", Label = "Open File", Action = "open" });
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