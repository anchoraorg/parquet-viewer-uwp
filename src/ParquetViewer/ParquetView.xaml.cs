using Microsoft.Toolkit.Uwp.UI.Controls;
using NetBox.Extensions;
using Parquet.Data;
using Parquet.Data.Rows;
using ParquetViewer.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ParquetViewer
{
   public sealed partial class ParquetView : UserControl
   {
      public ParquetView()
      {
         this.InitializeComponent();
      }

      public async Task DisplayAsync(StorageFile file)
      {
         FileGrid.Columns.Clear();

         Table table = await ParquetUwp.LoadAsync(file);

         int i = 0;
         foreach(Field f in table.Schema.Fields)
         {
            FileGrid.Columns.Add(new DataGridTextColumn
            {
               Header = f.Name,
               Width = DataGridLength.SizeToCells,
               Binding = new Binding
               {
                  Path = new PropertyPath("[" + i++ + "]")
               }
            });
         }

         FileGrid.ItemsSource = table.Select(r => new TableRowView(r)).ToList();

         StatusText.Text = $"showing first {table.Count} records.";
      }
   }
}