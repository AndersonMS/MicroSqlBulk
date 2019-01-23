﻿using MicroSqlBulk.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MicroSqlBulk
{
    public static partial class MicroSqlBulkExtension
    {
        public static void BulkUpdate<TEntity>(this IDbConnection dbConnection, IEnumerable<TEntity> data, int timeout = 30, bool openConnection = true, bool closeConnection = true)
        {
            DataTable dataTable = DataTableHelper.ConvertToDatatable(data.ToList());

            string tempTableName = $"#{dataTable.TableName}_TEMP";

            SqlConnection conn = (SqlConnection)dbConnection;

            using (SqlCommand command = new SqlCommand("", conn))
            {
                try
                {
                    if (openConnection)
                        conn.Open();

                    command.CommandText = TableHelper.GenerateLocalTempTableScript<TEntity>();
                    command.ExecuteNonQuery();


                    SqlBulkCopy bulkCopy =
                         new SqlBulkCopy
                         (
                             (SqlConnection)dbConnection,
                             SqlBulkCopyOptions.TableLock |
                             SqlBulkCopyOptions.FireTriggers |
                             SqlBulkCopyOptions.UseInternalTransaction,
                             null
                         );

                    bulkCopy.BulkCopyTimeout = timeout;
                    bulkCopy.DestinationTableName = tempTableName;
                    bulkCopy.WriteToServer(dataTable);
                    bulkCopy.Close();

                    string setUpdate = TableHelper.GenerateSetUpdate<TEntity>();
                    string onJoin = TableHelper.GenerateOnJoin<TEntity>();
                    string valuesUpdate = TableHelper.GenerateValuesUpdate<TEntity>();
                    string columnsInsert = TableHelper.GenerateColumnsInsert<TEntity>();

                    command.CommandTimeout = timeout;
                    command.CommandText = $@"MERGE INTO {dataTable.TableName} 
                                            WITH(HOLDLOCK) USING {tempTableName}
                                            {onJoin}
                                            WHEN MATCHED THEN UPDATE SET {setUpdate}
                                            WHEN NOT MATCHED BY TARGET THEN INSERT({columnsInsert}) values({valuesUpdate});
                                            DROP TABLE {tempTableName};";
                    command.ExecuteNonQuery();
                }
                finally
                {
                    if (closeConnection)
                        conn.Close();
                }
            }
        }
    }
}
