using MicroSqlBulk.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace MicroSqlBulk
{
    [Obsolete("This class is deprecated, use MicroSqlBulkExtension instead.")]
    public static class BulkInsertExtension
    {
        [Obsolete("This method is deprecated, use MicroSqlBulkExtension.BulkInsert instead.")]
        public static void BulkInsert<TEntity>(this IDbConnection dbConnection, List<TEntity> data, int timeout = 30, bool openConnection = true, bool closeConnection = true)
        {
            try
            {
                var datatable = DataTableHelper.ConvertToDatatable(data);

                SqlBulkCopy bulkCopy =
                        new SqlBulkCopy
                        (
                            (SqlConnection)dbConnection,
                            SqlBulkCopyOptions.TableLock |
                            SqlBulkCopyOptions.FireTriggers |
                            SqlBulkCopyOptions.UseInternalTransaction,
                            null
                        );

                bulkCopy.DestinationTableName = datatable.TableName;
                bulkCopy.BulkCopyTimeout = timeout;

                if (openConnection)
                    dbConnection.Open();

                bulkCopy.WriteToServer(datatable);
            }
            finally
            {

                if (closeConnection && dbConnection.State == ConnectionState.Open)
                    dbConnection.Close();
            } 
        }
    }
}
