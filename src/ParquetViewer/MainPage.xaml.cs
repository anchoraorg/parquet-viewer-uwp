using System;
using ParquetViewer.Model;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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

         await LoadAndDisplayAsync(ParquetUwp.GetFromFileAssociationAsync(e));
      }

      private async Task FileDraggedIn(StorageFile file)
      {
         await LoadAndDisplayAsync(file);
      }

      private async Task LoadAndDisplayAsync(StorageFile file)
      {
         HamburgerMenu.IsPaneOpen = false;

         if (file == null) return;

         LoadingControl.IsLoading = true;

         try
         {
            await DisplayAsync(file);
         }
         finally
         {
            LoadingControl.IsLoading = false;
         }

      }

      public async Task DisplayAsync(StorageFile file)
      {
         await DisplayArea.DisplayAsync(file);

         FrontEmptyArea.Visibility = Visibility.Collapsed;
         DisplayArea.Visibility = Visibility.Visible;
      }

      private async void HamburgerMenu_ItemClick(object sender, ItemClickEventArgs e)
      {
         MenuItem mi = e.ClickedItem as MenuItem;
         if (mi == null) return;

         if (mi.Action == "open")
         {
            await LoadAndDisplayAsync(await ParquetUwp.GetFromFilePickerAsync());
         }
         else if(mi.Action == "issue")
         {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://github.com/aloneguid/parquet-viewer-uwp/issues/new"));
         }
         else if(mi.Action == "about")
         {
            await Windows.System.Launcher.LaunchUriAsync(new Uri("https://github.com/aloneguid/parquet-viewer-uwp"));
         }

         HamburgerMenu.SelectedItem = null;
      }
   }
}
