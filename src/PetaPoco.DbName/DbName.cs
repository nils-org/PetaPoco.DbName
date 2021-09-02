using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

// ReSharper disable once CheckNamespace
namespace PetaPoco
{
    public static class DbName
    {
        private static readonly Dictionary<string, string> Cache = new Dictionary<string, string>();

        public static string GetColumnName<TSource>(this IDatabase db, Expression<Func<TSource, object>> property)
        {
            var propName = GetName(property);
            var pocoType = typeof(TSource);
            var key = $"{db.Provider.GetType().Name}/{pocoType.FullName}.{propName}";
            if (Cache.TryGetValue(key, out var dbName))
            {
                return dbName;
            }

            dbName = propName;

            var colProp = pocoType.GetProperties().FirstOrDefault(x => x.Name == propName);
            if (colProp == null)
            {
                throw new ArgumentException(
                    $"Could not find access the property named '{propName}'",
                    nameof(property));
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
            Cache[key] = dbName;
            return dbName;
        }

        public static string GetTableName<TSource>(this IDatabase db)
        {
            var pocoType = typeof(TSource);
            var pocoName = pocoType.Name;
            var key = $"{db.Provider.GetType().Name}/{pocoType.FullName}.<TABLE>";
            if (Cache.TryGetValue(key, out var dbName))
            {
                return dbName;
            }

            dbName = pocoName;

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
            Cache[key] = dbName;
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
}