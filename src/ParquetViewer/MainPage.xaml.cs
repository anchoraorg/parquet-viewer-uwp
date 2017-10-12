using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Parquet.Data;
using ParquetViewer.Model;
using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ParquetViewer
{
   /// <summary>
   /// An empty page that can be used on its own or navigated to within a Frame.
   /// </summary>
   public sealed partial class MainPage : Page
   {
      public MainPage()
      {
         this.InitializeComponent();

         DisplayArea.Visibility = Visibility.Collapsed;
         DropArea.Visibility = Visibility.Visible;
      }

      protected override async void OnNavigatedTo(NavigationEventArgs e)
      {
         base.OnNavigatedTo(e);

         DataSet ds = await ParquetUwp.OpenFromFileAssociationAsync(e);

         Display(ds);
      }

      private void DropArea_DragOver(object sender, DragEventArgs e)
      {
         e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
      }

      private async void DropArea_Drop(object sender, DragEventArgs e)
      {
         DataSet ds = await ParquetUwp.OpenFromDragDropAsync(e);

         Display(ds);
      }

      private async void OpenFileButton_Click(object sender, RoutedEventArgs e)
      {
         DataSet ds = await ParquetUwp.OpenFromFilePickerAsync();

         Display(ds);
      }


      public void Display(DataSet ds)
      {
         SfGrid.Columns.Clear();

         if (ds == null) return;

         DropArea.Visibility = Visibility.Collapsed;
         DisplayArea.Visibility = Visibility.Visible;

         for (int i = 0; i < ds.ColumnCount; i++)
         {
            SfGrid.Columns.Add(CreateSfColumn(ds.Schema[i], i));
         }

         SfGrid.ItemsSource = Enumerable
            .Range(0, ds.RowCount)
            .Select(rn => new TableRowView(ds[rn]));


         if(ds.TotalRowCount > ParquetUwp.MaxRows)
         {
            StatusText.Text = $"First {ParquetUwp.MaxRows} of {ds.TotalRowCount} are shown";
         }
         else
         {
            StatusText.Text = $"All {ds.TotalRowCount} rows are shown";
         }

      }

      private GridColumn CreateSfColumn(SchemaElement se, int i)
      {
         GridColumn result;

         if (se.ElementType == typeof(int) ||
            se.ElementType == typeof(float) ||
            se.ElementType == typeof(double) ||
            se.ElementType == typeof(decimal))
         {
            result = new GridNumericColumn();
         }
         else if (se.ElementType == typeof(DateTime) ||
            se.ElementType == typeof(DateTimeOffset))
         {
            result = new GridDateTimeColumn();
         }
         else if (se.ElementType == typeof(bool))
         {
            result = new GridCheckBoxColumn();
         }
         else
         {
            result = new GridTextColumn();
         }

         result.MappingName = $"[{i}]";
         result.HeaderText = se.Name;
         result.AllowFiltering = false;
         result.AllowFocus = true;
         result.AllowResizing = true;
         result.AllowSorting = true;
         result.FilterBehavior = FilterBehavior.StronglyTyped;
         result.AllowEditing = true;

         return result;
      }
   }
}
