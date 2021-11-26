using System;
using JetBrains.Annotations;
using PetaPoco.Providers;
using Shouldly;
using Xunit;

namespace PetaPoco.DbName.Tests;

public class ColumnNameTests : IDisposable
{
    private readonly IDatabase _db;

    public ColumnNameTests()
    {
        _db = DatabaseConfiguration
            .Build()
            .WithAutoSelect()
            .UsingProvider<SQLiteDatabaseProvider>()
            .UsingConnectionString(":memory:")
            .Create();
    }

    [Fact]
    public void Should_Return_PropertyName_For_Unattributed_Properties()
    {
        var actual = _db.GetColumnName<TestTable>(x => x.Property1);

        actual.ShouldBe(_db.Provider.EscapeSqlIdentifier("Property1"));
    }

    [Fact]
    public void Should_Return_Renamed_PropertyName_For_Attributed_Properties()
    {
        var actual = _db.GetColumnName<TestTable>(x => x.Renamed);

        actual.ShouldBe(_db.Provider.EscapeSqlIdentifier("Property2"));
    }

    public void Dispose()
    {
        _db.Dispose();
    }

    // ReSharper disable UnusedMember.Local
    // ReSharper disable UnusedAutoPropertyAccessor.Local
    [PrimaryKey("Id")]
    [UsedImplicitly]
    private class TestTable
    {
        public long Id { get; set; }

        public string? Property1 { get; set; }

        [Column("Property2")]
        public string? Renamed { get; set; }
    }
    // ReSharper restore UnusedAutoPropertyAccessor.Local
    // ReSharper enable UnusedMember.Local
}
