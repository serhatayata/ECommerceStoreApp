using CampaignService.Api.Models.Enums;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;

namespace CampaignService.Api.Utilities.Json;

public class JsonExpressionParser
{
    private readonly string In = nameof(In).ToLowerInvariant();
    private readonly string And = nameof(And).ToLowerInvariant();

    private readonly MethodInfo MethodContains = typeof(Enumerable).GetMethods(
                    BindingFlags.Static | BindingFlags.Public)
                    .Single(m => m.Name == nameof(Enumerable.Contains)
                        && m.GetParameters().Length == 2);

    private delegate Expression Binder(Expression left, Expression right);

    private Expression ParseTree<T>(
        JsonElement condition,
        ParameterExpression parm)
    {
        Expression left = null;
        var gate = condition.GetProperty(nameof(condition)).GetString();

        JsonElement rules = condition.GetProperty(nameof(rules));

        Binder binder = gate == And ? (Binder)Expression.And : Expression.Or;

        Expression bind(Expression left, Expression right) =>
            left == null ? right : binder(left, right);

        foreach (var rule in rules.EnumerateArray())
        {
            if (rule.TryGetProperty(nameof(condition), out JsonElement check))
            {
                var right = ParseTree<T>(rule, parm);
                left = bind(left, right);
                continue;
            }

            string @operator = rule.GetProperty(nameof(@operator)).GetString();
            string type = rule.GetProperty(nameof(type)).GetString();
            string field = rule.GetProperty(nameof(field)).GetString();

            JsonElement value = rule.GetProperty(nameof(value));

            var property = Expression.Property(parm, field);

            if (@operator == In)
            {
                var contains = MethodContains.MakeGenericMethod(typeof(string));
                object val = value.EnumerateArray().Select(e => e.GetString())
                    .ToList();
                var right = Expression.Call(
                    contains,
                    Expression.Constant(val),
                    property);
                left = bind(left, right);
            }
            else
            {
                object val = GetTypeValue(type, value);
                var toCompare = Expression.Constant(val);
                var right = GetExpressionComparison(property, toCompare, @operator);
                left = bind(left, right);
            }
        }

        return left;
    }

    BinaryExpression? GetExpressionComparison(
        MemberExpression property,
        ConstantExpression toCompare,
        string @operator)
    {
        var comparisonParse = Enum.TryParse(@operator, out ComparisonOperator comparisonOperator);
        if (!comparisonParse)
            throw new ArgumentException("Comparison parse failed");

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
                ComparisonOperator.notequal => Expression.NotEqual(property, toCompare),
                ComparisonOperator.greater => Expression.GreaterThan(property, toCompare),
                ComparisonOperator.greaterorequal => Expression.GreaterThanOrEqual(property, toCompare),
                ComparisonOperator.less => Expression.LessThan(property, toCompare),
                ComparisonOperator.lessorequal => Expression.LessThanOrEqual(property, toCompare)
            };
        }
    }

    public Expression<Func<T, bool>> ParseExpressionOf<T>(JsonDocument doc)
    {
        var itemExpression = Expression.Parameter(typeof(T));
        var conditions = ParseTree<T>(doc.RootElement, itemExpression);
        if (conditions.CanReduce)
            conditions = conditions.ReduceAndCheck();

        var query = Expression.Lambda<Func<T, bool>>(conditions, itemExpression);
        return query;
    }

    public object GetTypeValue(
        string type,
        JsonElement value)
    {
        string StringStr = nameof(String).ToLowerInvariant();
        string BooleanStr = nameof(Boolean).ToLowerInvariant();
        string Int32Str = nameof(Int32).ToLowerInvariant();
        string DecimalStr = nameof(Decimal).ToLowerInvariant();
        string DateTimeStr = nameof(DateTime).ToLowerInvariant();

        if (type == StringStr)
            return (object)value.GetString();
        else if (type == BooleanStr)
            return (object)value.GetBoolean();
        else if (type == Int32Str)
            return (object)value.GetInt32();
        else if (type == DecimalStr)
            return (object)value.GetDecimal();
        else if (type == DateTimeStr)
            return (object)value.GetDateTime();

        throw new ArgumentException("Type not found");
    }

    public Func<T, bool> ParsePredicateOf<T>(JsonDocument doc)
    {
        var query = ParseExpressionOf<T>(doc);
        return query.Compile();
    }
}
