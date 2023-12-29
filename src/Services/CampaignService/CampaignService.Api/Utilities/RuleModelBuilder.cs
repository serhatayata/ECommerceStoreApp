using CampaignService.Api.Models.Enums;
using CampaignService.Api.Models.Rules;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace CampaignService.Api.Utilities;

public static class RuleModelBuilder
{
    public static RuleModel GetModelRule<T>()
    {
		try
		{
            var ruleModel = new RuleModel();
            var type = typeof(T);

            ruleModel.Conditions = GetConditions();
            ruleModel.Operators = GetOperators();
            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                       .Where(p => !typeof(IEnumerable).IsAssignableFrom(p.PropertyType)));

            var ruleItems = new List<RuleItemModel>();
            foreach (var prop in props)
            {
                var propertyName = prop.Name;
                var type = GetTypeName(prop.PropertyType);
            }
        }
        catch (Exception ex)
		{
            //Log
            return new RuleModel();
		}
    }

    private static List<RuleConditionModel> GetConditions()
    {
        string In = nameof(In).ToLowerInvariant();
        string And = nameof(And).ToLowerInvariant();

        return new List<RuleConditionModel>()
        {
            new RuleConditionModel()
            {
                Name = In,
                Symbol = "|"
            },
            new RuleConditionModel()
            {
                Name = And,
                Symbol = "&"
            }
        };
    }

    private static List<RuleOperatorModel> GetOperators()
    {
        var operators = Enum.GetValues(typeof(ComparisonOperator))
                            .Cast<ComparisonOperator>()
                            .ToList();

        var result = new List<RuleOperatorModel>();
        operators.ForEach(op =>
        {
            var symbol = op switch
            {
                ComparisonOperator.equal => "=",
                ComparisonOperator.notequal => "!=",
                ComparisonOperator.greater => ">",
                ComparisonOperator.greaterorequal => ">=",
                ComparisonOperator.less => "<",
                ComparisonOperator.lessorequal => "<="
            };
            var model = new RuleOperatorModel(op.ToString(), symbol);
            result.Add(model);
        });

        return result;
    }

    private static string GetTypeName(Type type)
    {
        var code = Type.GetTypeCode(type) switch
        {
            case TypeCode.Byte:
            case TypeCode.SByte:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }
}
