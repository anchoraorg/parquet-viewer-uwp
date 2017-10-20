using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ParquetViewer
{
   public sealed partial class FrontEmpty : UserControl
   {
      public Func<StorageFile, Task> FileDragged;

      public FrontEmpty()
      {
         this.InitializeComponent();
      }

      private void Grid_DragOver(object sender, DragEventArgs e)
      {
         e.AcceptedOperation = DataPackageOperation.Copy;
      }

      private async void Grid_Drop(object sender, DragEventArgs e)
      {
         StorageFile file = await ParquetUwp.GetFirstParquetFileAsync(e);

         if(file != null && FileDragged != null)
         {
            await FileDragged(file);
         }
      }

      private async void OpenFileButton_Click(object sender, RoutedEventArgs e)
      {
         StorageFile file = await ParquetUwp.GetFromFilePickerAsync();

         if(file != null && FileDragged != null)
         {
            await FileDragged(file);
         }
      }
   }
}