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
            TypeCode[] notAllowedTypeCodes = new[] { TypeCode.Empty, TypeCode.Object, TypeCode.DBNull };
            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                       .Where(p => !p.PropertyType.IsAssignableFrom(typeof(IEnumerable)) &&
                                                   !p.PropertyType.IsAbstract &&
                                                   !p.GetAccessors().Any(a => a.IsVirtual) &&
                                                   !notAllowedTypeCodes.Contains(Type.GetTypeCode(p.PropertyType)))
                                       .ToArray();

            var ruleItems = new List<RuleItemModel>();
            foreach (var prop in props)
            {
                var propertyName = prop.Name;
                string typeName = GetTypeName(prop.PropertyType);

                ruleItems.Add(new RuleItemModel(propertyName, typeName));
            }
            ruleModel.Items = ruleItems;
            return ruleModel;
        }
        catch (Exception ex)
		{
            //Log
            return new RuleModel();
		}
    }

    private static List<RuleConditionModel> GetConditions()
    {
        string Or = nameof(Or).ToLowerInvariant();
        string And = nameof(And).ToLowerInvariant();

        return new List<RuleConditionModel>()
        {
            new RuleConditionModel()
            {
                Name = Or,
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
                ComparisonOperator.lessorequal => "<=",
                ComparisonOperator.@in => "in",
            };
            var model = new RuleOperatorModel(op.ToString(), symbol);
            result.Add(model);
        });

        return result;
    }

    private static string GetTypeName(Type type)
    {
        string StringStr = TypeCode.String.ToString().ToLowerInvariant();
        string BooleanStr = TypeCode.Boolean.ToString().ToLowerInvariant();
        string IntStr = TypeCode.Int32.ToString().Substring(0, 3).ToLowerInvariant();
        string DecimalStr = TypeCode.Decimal.ToString().ToLowerInvariant();
        string DoubleStr = TypeCode.Double.ToString().ToLowerInvariant();
        string DateTimeStr = TypeCode.DateTime.ToString().ToLowerInvariant();
        string CharStr = TypeCode.Char.ToString().ToLowerInvariant();
        string FloatStr = TypeCode.Single.ToString().ToLowerInvariant();

        var code = Type.GetTypeCode(type);
        switch (code)
        {
            case TypeCode.Empty: return string.Empty;
            case TypeCode.Object: return string.Empty;
            case TypeCode.DBNull: return string.Empty;
            case TypeCode.Boolean: return BooleanStr;
            case TypeCode.SByte: return BooleanStr;
            case TypeCode.Byte: return BooleanStr;
            case TypeCode.Char: return CharStr;
            case TypeCode.Int16: return IntStr;
            case TypeCode.UInt16: return IntStr;
            case TypeCode.Int32: return IntStr;
            case TypeCode.UInt32: return IntStr;
            case TypeCode.Int64: return IntStr;
            case TypeCode.UInt64: return IntStr;
            case TypeCode.Single: return FloatStr;
            case TypeCode.Double: return DoubleStr;
            case TypeCode.Decimal: return DecimalStr;
            case TypeCode.DateTime: return DateTimeStr;
            case TypeCode.String: return StringStr;
            default: return string.Empty;
        }
    }
}
