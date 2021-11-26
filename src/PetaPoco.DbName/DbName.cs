using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;

namespace PetaPoco;

public static class DbName
{
    private static readonly ConcurrentDictionary<string, string> Cache = new ConcurrentDictionary<string, string>();

    public static string GetColumnName<TSource>(this IDatabase db, Expression<Func<TSource, object?>> property)
    {
        var propName = GetName(property);
        var pocoType = typeof(TSource);
        var key = $"{db.Provider.GetType().Name}/{pocoType.FullName}.{propName}";
        return Cache.GetOrAdd(key, FindColumnName(db, propName, pocoType));
    }

    private static string FindColumnName(IDatabase db, string propName, Type pocoType)
    {
        var dbName = propName;

        var colProp = pocoType.GetProperties().FirstOrDefault(x => x.Name == propName);
        if (colProp == null)
        {
            throw new ArgumentException(
                $"Could not access the property named '{propName}'",
                nameof(propName));
        }

        var attr = colProp.GetCustomAttributes(true).OfType<ColumnAttribute>().FirstOrDefault();
        if (attr != null)
        {
            var nonStandardName = attr.Name;
            if (!string.IsNullOrEmpty(nonStandardName))
            {
                dbName = nonStandardName;
            }
        }

        dbName = db.Provider.EscapeSqlIdentifier(dbName);
        return dbName;
    }

    public static string GetTableName<TSource>(this IDatabase db)
    {
        var pocoType = typeof(TSource);
        var pocoName = pocoType.Name;
        var key = $"{db.Provider.GetType().Name}/{pocoType.FullName}.<TABLE>";
        return Cache.GetOrAdd(key, FindTableName(db, pocoName, pocoType));
    }

    private static string FindTableName(IDatabase db, string pocoName, Type pocoType)
    {
        var dbName = pocoName;

        var attr = pocoType.GetCustomAttributes(true).OfType<TableNameAttribute>().FirstOrDefault();
        if (attr != null)
        {
            var nonStandardName = attr.Value;
            if (!string.IsNullOrEmpty(nonStandardName))
            {
                dbName = nonStandardName;
            }
        }

        dbName = db.Provider.EscapeTableName(dbName);
        return dbName;
    }

    private static string GetName<TSource, TField>(Expression<Func<TSource, TField>> field)
    {
        if (Equals(field, null))
        {
            throw new NullReferenceException("Field is required");
        }

        MemberExpression expr;
        if (field.Body is MemberExpression memberExpr)
        {
            expr = memberExpr;
        }
        else if (field.Body is UnaryExpression unaryExpr)
        {
            expr = (MemberExpression)unaryExpr.Operand;
        }
        else
        {
            throw new ArgumentException($"Expression '{field}' not supported.", nameof(field));
        }

        return expr.Member.Name;
    }
}
