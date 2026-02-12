using UnityEngine;

public class ARequestTrade : GOAPAction
{
    public string sellerID = "AgentB";
    public int price = 5;
    public int foodAmount = 1;

    private GOADAgent self;

    private void Awake()
    {
        self = GetComponent<GOADAgent>();
        duration = 1f;

        AddNumericPrecondition("Food", CompareOp.LessOrEqual, 0);
        AddNumericPrecondition("Money", CompareOp.GreaterOrEqual, 0);

        AddEffect("TradeRequested", true);
        AddEffect("TradePartnerId", sellerID);
        AddEffect("TradePrice", price);
        AddEffect("TradeFoodAmount", foodAmount);
        cost = 1f;
    }
    protected override void OnStart(WorldState state)
    {
        
    }
    protected override void OnComplete(WorldState state)
    {
        if (self == null) { return; }

        state["TradePartnerID"] = sellerID;
        state["TradePrice"] = price;
        state["TradeFoodAmount"]=foodAmount;

        SocialBoard.Instance.Send(new SocialMessage
        {
            type = SocialMessageType.TradeRequest,
            fromAgentId = self.agentId,
            toAgentId = sellerID,
            foodAmount = foodAmount
        }); 
    }
}
