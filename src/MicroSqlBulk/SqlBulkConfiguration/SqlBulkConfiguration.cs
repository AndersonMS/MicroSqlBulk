using MicroSqlBulk.Interfaces;
using System.Collections.Generic;


namespace MicroSqlBulk
{
    public class SqlBulkConfiguration : IMicroSqlConfiguration
    {
        public IList<Column> Columns { get; }
        public string TableName { get; }
        public string Schema { get; }

        public SqlBulkConfiguration(IList<Column> columns, string tableName, string schema)
        {
            Columns = columns;
            TableName = tableName;
            Schema = schema;
        }

        public string FullTableName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Schema))
                    return $"{Schema}.{TableName}";

                return TableName;

            }
        }

        public string FullTempTableName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Schema))
                    return $"{Schema}.#{TableName}_TEMP";

                return $"#{TableName}_TEMP";
            }
        }
    }
}
