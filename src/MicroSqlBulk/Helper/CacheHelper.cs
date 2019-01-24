using System.Collections.Concurrent;

namespace MicroSqlBulk.Helper
{
    public static class CacheHelper
    {
        private static ConcurrentDictionary<string, SqlBulkConfiguration> _mapperCache = new ConcurrentDictionary<string, SqlBulkConfiguration>();

        public static SqlBulkConfiguration GetConfiguration<TEntity>()
        {
            SqlBulkConfiguration sqlBulkEntityConfiguration;
            var nameOfT = typeof(TEntity).Name;

            if (!_mapperCache.TryGetValue(nameOfT, out sqlBulkEntityConfiguration))
            {
                var columns = ColumnHelper.GetFildesInfo<TEntity>();
                TableHelper.GetTableNameAndSchema<TEntity>(out string tableName, out string schema);
             
                sqlBulkEntityConfiguration = new SqlBulkConfiguration(columns, tableName, schema);
                _mapperCache.TryAdd(nameOfT, sqlBulkEntityConfiguration);
            }

            return sqlBulkEntityConfiguration;
        }
    }
}
