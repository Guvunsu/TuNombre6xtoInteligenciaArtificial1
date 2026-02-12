using UnityEngine;

public class AAcceptTrade : GOAPAction
{
    private GOADAgent self;

    private void Awake()
    {
        self = GetComponent<GOADAgent>();
        duration = 0.2f;

        AddPrecondition("HasIncomingTradeRequest", true);
        AddNumericPrecondition("Food", CompareOp.GreaterOrEqual, 1);

        AddEffect("TradeAccepted", true);
        cost = 1f;
    }

    protected override void OnStart(WorldState state) { }

    protected override void OnComplete(WorldState state)
    {
        if (self == null) return;

        string partnerId = state.ContainsKey("TradePartnerId") ? state["TradePartnerId"]?.ToString() : null;
        if (string.IsNullOrEmpty(partnerId)) return;

        SocialBoard.Instance.Send(new SocialMessage
        {
            type = SocialMessageType.TradeAccepted,
            fromAgentId = self.agentId,
            toAgentId = partnerId
        });
    }
}