using UnityEngine;

public class ARequestTrade : GOAPAction
{
    [Header("Trade Settings")]
    public string sellerId = "AgentB";
    public int price = 5;
    public int foodAmount = 1;

    private GOAPAgent self;

    private void Awake()
    {
        self = GetComponent<GOAPAgent>();

        duration = 0.2f;
        cost = 1f;

        AddPrecondition("TradeRequested", false);

        AddNumericPrecondition("Food", CompareOp.LessOrEqual, 0);
        AddNumericPrecondition("Money", CompareOp.GreaterOrEqual, price);

        // Para el planner (simulación)
        AddEffect("TradeRequested", true);
        AddEffect("TradePartnerId", sellerId);
        AddEffect("TradePrice", price);
        AddEffect("TradeFoodAmount", foodAmount);
    }

    protected override void OnStart(WorldState state) { }

    protected override void OnComplete(WorldState state)
    {
        // ESTO ES LO QUE TE FALTABA EN RUNTIME:
        state["TradeRequested"] = true;
        state["TradePartnerId"] = sellerId;
        state["TradePrice"] = price;
        state["TradeFoodAmount"] = foodAmount;

        Debug.Log($"[{self.agentId}] ARequestTrade runtime wrote TradePartnerId={state["TradePartnerId"]}");

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
