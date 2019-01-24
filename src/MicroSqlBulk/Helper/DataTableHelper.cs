using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MicroSqlBulk.Helper
{
    public static class DataTableHelper
    {
        public static DataTable ConvertToDatatable<TEntity>(List<TEntity> data)
        {
            var sqlBulkEntityConfiguration = CacheHelper.GetConfiguration<TEntity>();

            DataTable dataTable = new DataTable(sqlBulkEntityConfiguration.FullTableName);

            var dataColumns = sqlBulkEntityConfiguration.Columns
                                .Select(col => new DataColumn(col.Name, Nullable.GetUnderlyingType(col.PropertyDescriptor.PropertyType) ?? col.PropertyDescriptor.PropertyType))
                                .ToArray();

            dataTable.Columns.AddRange(dataColumns);

            foreach (var row in GetRows(data))
            {
                dataTable.Rows.Add(row.Values);
            }

            return dataTable;
        }
   
        static IEnumerable<Row> GetRows<TEntity>(List<TEntity> data)
        {
            var sqlBulkEntityConfiguration = CacheHelper.GetConfiguration<TEntity>();

            List<object> values = new List<object>();

            foreach (TEntity item in data)
            {
                foreach (var prop in sqlBulkEntityConfiguration.Columns.ToList())
                {
                    values.Add(prop.PropertyDescriptor.GetValue(item));
                }

                var row = new Row(values.ToArray());

                values.Clear();

                yield return row;
            }
        }
    }
}
