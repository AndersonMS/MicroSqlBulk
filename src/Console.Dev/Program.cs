
using MicroSqlBulk;
using MicroSqlBulk.Attributes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Console.Dev
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"data source=localhost\SQLEXPRESS;initial catalog=HBMDM;persist security info=True;user id=sa;password=Sa12345;MultipleActiveResultSets=True;App=EntityFramework";
            var list = new List<Persons>();

            for (int i = 1; i <= 1000000; i++)
            {
                list.Add(new Persons
                {
                    FirstName = $"FirstName {i}",
                    LastName = $"LastName {i}",
                    Address = $"Address {i}",
                    City = $"City{i}",
                    PersonID = i

                });
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                MicroSqlBulkExtension.BulkInsertOrUpdate(connection, list, 300);
            }
        }
    }

    [Table("Persons", "dbo")]
    public class Persons
    {
        [Column("PersonID", true)]
        public int PersonID { get; set; }

        [Column("LastName")]
        public string LastName { get; set; }

        [Column("FirstName")]
        public string FirstName { get; set; }

        [Column("Address")]
        public string Address { get; set; }

        [Column("City")]
        public string City { get; set; }

        [Column("BirthDate")]
        public DateTime? BirthDate { get; set; }
    }
}

