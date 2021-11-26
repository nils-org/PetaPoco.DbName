using System;
using JetBrains.Annotations;
using PetaPoco.Providers;
using Shouldly;
using Xunit;

namespace PetaPoco.DbName.Tests;

public class TableNameTests : IDisposable
{
    private readonly IDatabase db;

    public TableNameTests()
    {
        db = DatabaseConfiguration
            .Build()
            .WithAutoSelect()
            .UsingProvider<SQLiteDatabaseProvider>()
            .UsingConnectionString(":memory:")
            .Create();
    }

    [Fact]
    public void Should_Return_TableName_For_Unattributed_Class()
    {
        var actual = db.GetTableName<TestTable1>();

        actual.ShouldBe(db.Provider.EscapeTableName("TestTable1"));
    }

    [Fact]
    public void Should_Return_Renamed_TableName_For_Attributed_Class()
    {
        var actual = db.GetTableName<RenamedTable>();

        actual.ShouldBe(db.Provider.EscapeTableName("TestTable2"));
    }

    public void Dispose()
    {
        db.Dispose();
    }

    // ReSharper disable UnusedMember.Local
    [PrimaryKey("Id")]
    [UsedImplicitly]
    private class TestTable1
    {
        public long Id { get; set; }
    }

    [TableName("TestTable2")]
    [PrimaryKey("Id")]
    [UsedImplicitly]
    private class RenamedTable
    {
        public long Id { get; set; }
    }
    // ReSharper enable UnusedMember.Local
}
