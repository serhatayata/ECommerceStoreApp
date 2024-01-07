using CampaignService.Api.Models.Enums;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace CampaignService.Api.Helpers;

public static class ExpressionHelper
{
    private static MethodInfo methodContains = typeof(Enumerable).GetMethods(
               BindingFlags.Static | BindingFlags.Public)
               .Single(m => m.Name == nameof(Enumerable.Contains)
               && m.GetParameters().Length == 2);

    public static Expression GetMemberProperty(
    ParameterExpression param,
    string field)
    {
        if (!field.Contains("."))
        {
            return Expression.Property(param, field);
        }
        else
        {
            Expression member = param;
            foreach (var namePart in field.Split('.'))
                member = Expression.Property(member, namePart);

            return member;
        }
    }

    public static MethodCallExpression CreateMethodExpression(
    Expression prop,
    JsonElement val,
    string type,
    string @operator
    )
    {
        if (@operator == ComparisonOperator.contains.ToString())
        {
            object value = GetTypeValue(type, val);
            return Expression.Call(
                   prop,
                   nameof(string.Contains),
                   Type.EmptyTypes,
                   Expression.Constant(value));
        }
        else if (@operator == ComparisonOperator.@in.ToString())
        {
            object value = GetTypeEnumeratedValue(val, type);
            return Expression.Call(
                   methodContains.MakeGenericMethod(GetType(type)),
                   Expression.Constant(value),
                   prop);
        }
        else if (@operator == ComparisonOperator.starts_with.ToString())
        {
            object value = GetTypeValue(type, val);
            return Expression.Call(
                   prop,
                   nameof(string.StartsWith),
                   Type.EmptyTypes,
                   Expression.Constant(value));
        }

        return null;
    }

    public static BinaryExpression? GetExpressionComparison(
    Expression property,
    object val,
    string @operator)
    {
        var comparisonParse = Enum.TryParse(@operator, out ComparisonOperator comparisonOperator);
        if (!comparisonParse)
            throw new ArgumentException("Comparison parse failed");

        var toCompare = Expression.Constant(val);
        if (property.Type.IsEnum)
        {
            toCompare = Expression.Constant(Enum.ToObject(property.Type, toCompare.Value));
            return Expression.Equal(property, toCompare);
        }
        else
        {
            return comparisonOperator switch
            {
                ComparisonOperator.equal => Expression.Equal(property, toCompare),
                ComparisonOperator.not_equal => Expression.NotEqual(property, toCompare),
                ComparisonOperator.greater => Expression.GreaterThan(property, toCompare),
                ComparisonOperator.greater_or_equal => Expression.GreaterThanOrEqual(property, toCompare),
                ComparisonOperator.less => Expression.LessThan(property, toCompare),
                ComparisonOperator.less_or_equal => Expression.LessThanOrEqual(property, toCompare)
            };
        }
    }

    public static object? GetTypeValue(
    string type,
    JsonElement value)
    {
        var valueType = GetType(type);
        TypeCode typeCode = Type.GetTypeCode(valueType);

        return typeCode switch
        {
            TypeCode.String => value.GetString(),
            TypeCode.Boolean => value.GetBoolean(),
            TypeCode.Int32 => value.GetInt32(),
            TypeCode.Int16 => value.GetInt16(),
            TypeCode.Int64 => value.GetInt64(),
            TypeCode.Byte => value.GetByte(),
            TypeCode.Single => value.GetSingle(),
            TypeCode.Decimal => value.GetDecimal(),
            TypeCode.DateTime => value.GetDateTime(),
            _ => TypeCode.Empty
        };
    }

    public static object GetTypeEnumeratedValue(
    JsonElement val,
    string type)
    {
        var valueType = GetType(type);
        TypeCode typeCode = Type.GetTypeCode(valueType);

        var enumeratedArray = val.EnumerateArray();
        return typeCode switch
        {
            TypeCode.String => enumeratedArray.Select(s => s.GetString()),
            TypeCode.Int32 => enumeratedArray.Select(s => s.GetInt32()),
            TypeCode.Int16 => enumeratedArray.Select(s => s.GetInt16()),
            TypeCode.Single => enumeratedArray.Select(s => s.GetSingle()),
            TypeCode.Decimal => enumeratedArray.Select(s => s.GetDecimal()),
        };
    }

    public static Type GetType(string type)
    {
        string StringStr = TypeCode.String.ToString().ToLowerInvariant();
        string BooleanStr = TypeCode.Boolean.ToString().ToLowerInvariant();
        string IntStr = TypeCode.Int32.ToString().Substring(0, 3).ToLowerInvariant();
        string DecimalStr = TypeCode.Decimal.ToString().ToLowerInvariant();
        string DateTimeStr = TypeCode.DateTime.ToString().ToLowerInvariant();

        if (type == StringStr)
            return typeof(string);
        else if (type == BooleanStr)
            return typeof(bool);
        else if (type == IntStr)
            return typeof(int);
        else if (type == DecimalStr)
            return typeof(decimal);
        else if (type == DateTimeStr)
            return typeof(DateTime);

        return null;
    }
}