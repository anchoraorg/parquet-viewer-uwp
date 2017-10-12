using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

      public object this[int i] => _parquetRow[i];
   }
}
