using NetBox.Extensions;
using Parquet.Data;
using ParquetViewer.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ParquetViewer
{
    public class FileViewModel
    {
        private ObservableCollection<TableRowView> _rows = new ObservableCollection<TableRowView>();

        public ObservableCollection<TableRowView> Rows => _rows;

        public FileViewModel()
        {

        }
    }

    public sealed partial class ParquetView : UserControl
    {
        public ParquetView()
        {
            this.InitializeComponent();

            ViewModel = new FileViewModel();

        }

        public FileViewModel ViewModel { get; set; }

        public async Task DisplayAsync(StorageFile file)
        {
            FileGrid.Columns.Clear();
        }
    }
}