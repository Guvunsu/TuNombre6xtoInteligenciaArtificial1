// ===============================
// AAcceptTrade.cs (CORREGIDO)
// - Al aceptar, envía mensaje de aceptación al partner.
// - Pone TradeAccepted=true (sirve si el vendedor tiene goal "TradeAccepted=true").
// ===============================
using UnityEngine;

public class AAcceptTrade : GOAPAction
{
    private GOAPAgent self;

    private void Awake()
    {
        self = GetComponent<GOAPAgent>();

        duration = 0.2f;
        cost = 1f;

        AddPrecondition("HasIncomingTradeRequest", true);
        AddNumericPrecondition("Food", CompareOp.GreaterOrEqual, 1);

        AddEffect("TradeAccepted", true);
    }

    protected override void OnStart(WorldState state) { }

    protected override void OnComplete(WorldState state)
    {
        state["TradeAccepted"] = true;

        string partnerId = state.ContainsKey("TradePartnerId") ? state["TradePartnerId"]?.ToString() : null;
        if (string.IsNullOrEmpty(partnerId)) return;

        if (SocialBoard.Instance != null)
        {
            SocialBoard.Instance.Send(new SocialMessage
            {
                type = SocialMessageType.TradeAccepted,
                fromAgentId = self != null ? self.agentId : "",
                toAgentId = partnerId
            });
        }
    }
}
