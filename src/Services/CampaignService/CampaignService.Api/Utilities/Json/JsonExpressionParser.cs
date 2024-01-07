using CampaignService.Api.Helpers;
using CampaignService.Api.Models.Enums;
using System.Linq.Expressions;
using System.Text.Json;

namespace CampaignService.Api.Utilities.Json;

public class JsonExpressionParser
{
    private readonly string In = nameof(In).ToLowerInvariant();
    private readonly string And = nameof(And).ToLowerInvariant();

    private delegate Expression Binder(Expression left, Expression right);

    public Func<T, bool> ParsePredicateOf<T>(JsonDocument doc)
    {
        var query = ParseExpressionOf<T>(doc);
        return query.Compile();
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
            var property = ExpressionHelper.GetMemberProperty(parm, field);
            if (@operator == ComparisonOperator.@in.ToString() ||
                @operator == ComparisonOperator.contains.ToString() ||
                @operator == ComparisonOperator.starts_with.ToString())
            {
                var right = ExpressionHelper.CreateMethodExpression(property, value, type, @operator);
                left = bind(left, right);
            }
            else
            {
                object val = ExpressionHelper.GetTypeValue(type, value);
                var right = ExpressionHelper.GetExpressionComparison(property, val, @operator);
                left = bind(left, right);
            }
        }

        return left;
    }
}
