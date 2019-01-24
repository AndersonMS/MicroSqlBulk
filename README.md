## MicroSqlBulk 

MicroSqlBulk  or **µ**SqlBulk is an IDbConnection extension for bulk operations.

### Download
>**µ**SqlBulk is available [here](https://www.nuget.org/packages/MicroSqlBulk/) as a Nuget Package. You can install it using the Nuget Package Manager Console:

```PM> Install-Package MicroSqlBulk -Version 1.1.3```

### usage
*1. Attributes*
 
Attributes is a powerful feature in the .NET that can add metadata information to your assemblies.
The current version of **µ**SqlBulk only supports metadata attributes. However, we're working to build the fluent version.
Attributes can be applied on your entity class or properties as the example below shows:


    [Table("TABLE_NAME", "SCHEMA")]
    public class Foo
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("XPTO")]
        public int Xpto { get; set; }

        [Ignore]
        public bool Ignored { get; set; }
    }  

*2. Extending the IDbConnection*

Extension methods is another powerful feature in the .NET and it allows you to extend the functionality of existing types with no need of modification them or creation of sub types out of them. 
Here's how simple it is to extend IDbConnection, look at the example below:

    List<Entity> data = DataBuilder();
    
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.BulkInsert(data);                
    }
   
To insert or bulk update, you can use this way:
    
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        connection.BulkInsertOrUpdate(dataListToUpdate);
    }

*Default settings:*

* Open Connection (true);
* Close connection (true);
* 30 seconds of timeout.

```void BulkInsert<TEntity>(this IDbConnection dbConnection, List<TEntity> data, int timeout = 30, bool openConnection = true, bool closeConnection = true);```

### License
You can read the terms of use [here](https://github.com/wgamagomes/MicroSqlBulk/blob/master/LICENSE).
