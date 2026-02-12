using UnityEngine;

public class AExecuteTrade : GOAPAction
{
    private GOADAgent self;

    private void Awake()
    {
        self = GetComponent<GOADAgent>();
        duration = 0.2f;

        AddPrecondition("IsNearPartner", true);
        AddPrecondition("TradeAcceptedByPartner", true);

        // Esto es solo “señal”, el cambio real lo hace SocialBoard.ExecuteTrade
        AddEffect("TradeCompleted", true);
        cost = 1f;
    }

    protected override void OnStart(WorldState state) { }

    protected override void OnComplete(WorldState state)
    {
        if (self == null) return;

        string partnerId = state.ContainsKey("TradePartnerId") ? state["TradePartnerId"]?.ToString() : null;
        int price = state.GetInt("TradePrice", 5);
        int foodAmount = state.GetInt("TradeFoodAmount", 1);

        if (string.IsNullOrEmpty(partnerId)) return;

        // self = buyer, partner = seller (para este ejemplo)
        bool ok = SocialBoard.Instance.ExecuteTrade(self.agentId, partnerId, price, foodAmount);

        // limpia flags locales
        state["TradeAcceptedByPartner"] = false;
        state["TradeRequested"] = false;

        // (opcional) si el trade falló, podrías setear algún flag de error
        if (!ok)
            state["TradeFailed"] = true;
    }
}