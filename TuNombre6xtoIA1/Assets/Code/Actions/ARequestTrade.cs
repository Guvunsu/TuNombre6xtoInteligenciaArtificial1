using UnityEngine;

public class ARequestTrade : GOAPAction
{
    [Header("Trade Settings")]
    public string sellerId = "AgentB";
    public int price = 10;
    public int foodAmount = 1;

    private GOADAgent self;

    private void Awake()
    {
        self = GetComponent<GOADAgent>();

        duration = 0.2f;
        cost = 1f;

        AddPrecondition("TradeRequested", false);

        AddNumericPrecondition("Food", CompareOp.LessOrEqual, 0);
        AddNumericPrecondition("Money", CompareOp.GreaterOrEqual, price = 10);

        // Para el planner (simulaci�n)
        AddEffect("TradeRequested", true);
        AddEffect("TradePartnerId", sellerId);
        AddEffect("TradePrice", price);
        AddEffect("TradeFoodAmount", foodAmount);
    }

    protected override void OnStart(WorldState state)
    {
        // ESTO ES LO QUE TE FALTABA EN RUNTIME:
        state["TradeRequested"] = true;
        state["TradePartnerId"] = sellerId;
        state["TradePrice"] = price;
        state["TradeFoodAmount"] = foodAmount;

        Debug.Log($"[{self.agentId}] ARequestTrade runtime wrote TradePartnerId={state["TradePartnerId"]}");
    }

    protected override void OnComplete(WorldState state)
    {

        SocialBoard.Instance.Send(new SocialMessage
        {
            type = SocialMessageType.TradeRequest,
            fromAgentId = self.agentId,
            toAgentId = sellerId,
            price = price,
            foodAmount = foodAmount
        });
    }
}
