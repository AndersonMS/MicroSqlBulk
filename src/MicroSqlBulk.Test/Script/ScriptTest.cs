using FluentAssertions;
using MicroSqlBulk.Attributes;
using MicroSqlBulk.Helper;
using NUnit.Framework;
using System;

namespace MicroSqlBulk.Test.Script
{
    public class ScriptTest
    {

        [SetUp]
        public void SetUp()
        {

        }

        [Test]
        public void ShouldBeReturnedTheTempTableCreationScript()
        {
            TableHelper
                 .GenerateLocalTempTableScript<TableDummy>()
                 .Should()
                 .Be(
@"CREATE TABLE #TABLE_DUMMY_TEMP
(
	 ID BIGINT NOT NULL,
	 DESCRIPTION NVARCHAR(MAX),
	 DATE DATETIME NOT NULL,
	 FLAG BIT NOT NULL,
	 MONEY DECIMAL(18,0) NOT NULL,
	 Status INT NOT NULL
)
");
        }
    }

    [Table("TABLE_DUMMY")]
    public class TableDummy
    {
        [Column("ID", true)]
        public int Id { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Column("DATE")]
        public DateTime Date { get; set; }

        [Column("FLAG")]
        public bool Flag { get; set; }

        [Column("MONEY")]
        public decimal Money { get; set; }

        [Column("Status")]
        public Status Status { get; set; }
    }

    public enum Status
    {
        Pending = 0,
        Active = 1,
        Inactive = 2,
        Deleted = 3
    }
}
