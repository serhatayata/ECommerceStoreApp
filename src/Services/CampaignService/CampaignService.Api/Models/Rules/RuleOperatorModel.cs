namespace CampaignService.Api.Models.Rules;

public class RuleOperatorModel
{
    public RuleOperatorModel()
    {
        
    }

    public RuleOperatorModel(string name, string symbol)
    {
        this.Name = name;
        this.Symbol = symbol;
    }

    /// <summary>
    /// Name of the operator
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// Symbol of the operator
    /// </summary>
    public string Symbol { get; set; }
}
