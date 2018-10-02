using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parquet;
using Parquet.Data;
using Parquet.Data.Rows;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace ParquetViewer
{
   static class ParquetUwp
   {
      public const int MaxRows = 500;

      public static async Task<StorageFile> GetFromFilePickerAsync()
      {
         var picker = new FileOpenPicker
         {
            ViewMode = PickerViewMode.List,
            SuggestedStartLocation = PickerLocationId.DocumentsLibrary
         };
         picker.FileTypeFilter.Add(".parquet");

         return await picker.PickSingleFileAsync();
      }

      public static async Task<StorageFile> GetFirstParquetFileAsync(DragEventArgs e)
      {
         if (e.DataView.Contains(StandardDataFormats.StorageItems))
         {
            IReadOnlyList<IStorageItem> items = await e.DataView.GetStorageItemsAsync();

            if (items.Count > 0)
            {
               var storageFile = items[0] as StorageFile;

               if (storageFile != null && Path.GetExtension(storageFile.Name).ToLower() == ".parquet")
               {
                  return storageFile;
               }
            }
         }

         return null;
      }

      public static StorageFile GetFromFileAssociationAsync(NavigationEventArgs e)
      {
         var args = e.Parameter as Windows.ApplicationModel.Activation.IActivatedEventArgs;
         if (args != null)
         {
            if (args.Kind == Windows.ApplicationModel.Activation.ActivationKind.File)
            {
               var fileArgs = args as Windows.ApplicationModel.Activation.FileActivatedEventArgs;
               string strFilePath = fileArgs.Files[0].Path;
               var file = (StorageFile)fileArgs.Files[0];
               return file;
            }
         }

         return null;
      }

      public static async Task<Table> LoadAsync(StorageFile file, int offset = 0, int count = 100)
      {
         using (IRandomAccessStreamWithContentType uwpStream = await file.OpenReadAsync())
         {
            using (Stream stream = uwpStream.AsStreamForRead())
            {
               var formatOptions = new ParquetOptions
               {
                  TreatByteArrayAsString = true
               };

               try
               {
                  using (var reader = new ParquetReader(stream, formatOptions))
                  {
                     return reader.ReadAsTable();
                  }
               }
               catch (Exception ex)
               {
                  var dialog = new MessageDialog(ex.Message, "Cannot open file");
                  await dialog.ShowAsync();
                  return null;
               }
            }
         }
      }
   }
}