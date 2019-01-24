using FluentAssertions;
using MicroSqlBulk.Attributes;
using MicroSqlBulk.Helper;
using NUnit.Framework;
using System;
using System.ComponentModel;
using IgnoreAttribute = MicroSqlBulk.Attributes.IgnoreAttribute;
namespace MicroSqlBulk.Test.Attribute
{
    public class AttributeTest
    {
        private PropertyDescriptorCollection props;

        [SetUp]
        public void SetUp()
        {
            props = TypeDescriptor.GetProperties(typeof(TableDummy0));
        }

        [Test]
        public void ShouldBeTrueForColumnAttribute()
        {
            Assert.AreEqual(true, AttributeHelper.TryGetCustomAttribute(props[0], out ColumnAttribute columnAttribute));
        }

        [Test]
        public void ShouldBeTrueForIgnoreAttribute()
        {
            Assert.AreEqual(true, AttributeHelper.TryGetCustomAttribute(props[2], out IgnoreAttribute columnAttribute));
        }

        [Test]
        public void ShouldBeFalseForIgnoreAttribute()
        {
            Assert.AreEqual(false, AttributeHelper.TryGetCustomAttribute(props[1], out IgnoreAttribute columnAttribute));
        }


        [Test]
        public void ShouldThrowExceptionWhenTheClassHasNoIgnoreOrColumnAttribute()
        {
            Action act = () => { ColumnHelper.GetFildesInfo<TableDummy1>(); };

            act.Should()
             .Throw<InvalidOperationException>();
        }

        [Test]
        public void ShouldThrowExceptionWhenTheClassHasNoTableAttribute()
        {
            Action act = () => { TableHelper.GetTableNameAndSchema<TableDummy2>(out string tableName, out string schema); };

            act.Should()
             .Throw<InvalidOperationException>();
        }

        [Test]
        public void ShouldReturnTheTableName()
        {
            TableHelper.GetTableNameAndSchema<TableDummy3>(out string tableName, out string schema);

            tableName.Should()
                .Be("TABLE_DUMMY_3");

            schema.Should()
             .Be(string.Empty);
        }

        [Test]
        public void ShouldThrowExceptionWhenTheEntityIsDefinedWithMoreThanOnePrimaryKey()
        {
            Assert.Throws<InvalidOperationException>(() => ColumnHelper.GetFildesInfo<TableDummy4>());
        }

        [Test]
        public void ShouldThrowExceptionWhenItDoesNotContainPrimaryKeyInTheEntityWhenUsingUpdate()
        {
            Assert.Throws<MissingFieldException>(() => TableHelper.GenerateOnJoin<TableDummy3>());
        }
    }

    [Table("TABLE_DUMMY_0", "SCHEMA")]
    class TableDummy0
    {
        [Column("Id")]
        public int Id { get; set; }

        public int Xpto { get; set; }

        [Ignore]
        public bool Ignored { get; set; }
    }


    [Table("TABLE_DUMMY_1")]
    public class TableDummy1
    {
        public int Id { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }
    }

    public class TableDummy2
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }
    }

    [Table("TABLE_DUMMY_3")]
    public class TableDummy3
    {
        [Column("ID")]
        public int Id { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }
    }

    [Table("TABLE_DUMMY_4")]
    public class TableDummy4
    {
        [Column("ID", true)]
        public int Id { get; set; }

        [Column("ID2", true)]
        public int Id2 { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }
    }
}
