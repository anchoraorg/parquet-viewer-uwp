using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Parquet.Data;
using ParquetViewer.Model;
using Syncfusion.Data;
using Syncfusion.UI.Xaml.Grid;
using Windows.ApplicationModel.DataTransfer;
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
using System.Threading.Tasks;

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
         FrontEmptyArea.Visibility = Visibility.Visible;

         HamburgerMenu.ItemsSource = MenuItem.GetMainItems();
         HamburgerMenu.OptionsItemsSource = MenuItem.GetOptionsItems();
         FrontEmptyArea.FileDragged = FileDraggedIn;
      }

      protected override async void OnNavigatedTo(NavigationEventArgs e)
      {
         base.OnNavigatedTo(e);

         await LoadAndDisplay(ParquetUwp.GetFromFileAssociationAsync(e));
      }

      private async Task FileDraggedIn(StorageFile file)
      {
         await LoadAndDisplay(file);
      }

      private async Task LoadAndDisplay(StorageFile file)
      {
         HamburgerMenu.IsPaneOpen = false;

         if (file == null) return;

         LoadingControl.IsLoading = true;

         try
         {
            DataSet ds = await ParquetUwp.LoadAsync(file);

            Display(ds);
         }
         finally
         {
            LoadingControl.IsLoading = false;
         }

      }

      public void Display(DataSet ds)
      {
         SfGrid.Columns.Clear();

         if (ds == null)
         {

            return;
         }

         FrontEmptyArea.Visibility = Visibility.Collapsed;
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

      private async void HamburgerMenu_ItemClick(object sender, ItemClickEventArgs e)
      {
         MenuItem mi = e.ClickedItem as MenuItem;
         if (mi == null) return;

         if (mi.Action == "open")
         {
            await LoadAndDisplay(await ParquetUwp.GetFromFilePickerAsync());
         }

         HamburgerMenu.SelectedItem = null;
      }
   }
}
