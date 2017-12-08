using Parquet.Data;

namespace ParquetViewer.Model
{
   public class TableRowView
   {
      private readonly Row _parquetRow;

      public TableRowView(Row row)
      {
         this._parquetRow = row;
      }

      public object this[int i]
      {
         get
         {
            return _parquetRow[i];
         }
      }
   }
}
