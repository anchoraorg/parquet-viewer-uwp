using Parquet.Data;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace ParquetViewer.Model
{
   public class ParquetIncrementalList : IncrementalList<TableRowView>
   {
      private const int BatchSize = 1000;
      private readonly StorageFile _file;
      private List<Row> _rows = new List<Row>();

      public ParquetIncrementalList(StorageFile file, Func<CancellationToken, uint, int, Task<IList<TableRowView>>> loadMoreItemsFunc) : base(loadMoreItemsFunc)
      {
         _file = file;
      }

      public async Task InitialiseAsync()
      {
         if(Schema == null)
         {
            DataSet ds = await ParquetUwp.LoadAsync(_file, 0, 0);
            Schema = ds.Schema;
            MaxItemCount = (int)ds.TotalRowCount;
         }
      }

      public Schema Schema { get; private set; }

      public int CachedRowsCount => _rows.Count;

      public async Task<IList<TableRowView>> LoadRowsAsync(CancellationToken token, uint count, int baseIndex)
      {
         bool isInWindow = baseIndex + count < _rows.Count;

         if(!isInWindow)
         {
            DataSet ds = await ParquetUwp.LoadAsync(_file, _rows.Count, BatchSize);

            _rows.AddRange(ds);
         }

         var result = new List<TableRowView>((int)count);

         for(int i = baseIndex; i < baseIndex + count; i++)
         {
            Row row = _rows[i];
            result.Add(new TableRowView(row));
         }

         return result;
      }
   }
}
