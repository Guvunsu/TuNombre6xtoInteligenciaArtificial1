// ===============================
// AExecuteTrade.cs (CORREGIDO)
// - Se ejecuta cuando estás cerca y el partner ya aceptó.
// - Ejecuta el trade en SocialBoard y limpia flags para evitar quedarse pegado.
// ===============================
using UnityEngine;

public class AExecuteTrade : GOAPAction
{
    private GOAPAgent self;

    private void Awake()
    {
        self = GetComponent<GOAPAgent>();

        duration = 0.2f;
        cost = 1f;

        AddPrecondition("IsNearPartner", true);
        AddPrecondition("TradeAcceptedByPartner", true);

        // Marcador
        AddEffect("TradeCompleted", true);

        // Para que el planner pueda ver que obtienes comida
        AddNumericEffect("Food", EffectOp.Add, 1);

        // CLAVE: “consumir” el handshake para evitar loops en el planner
        AddEffect("TradeAcceptedByPartner", false);
        AddEffect("IsNearPartner", false);
        AddEffect("TradeRequested", false);
    }


    protected override void OnStart(WorldState state) { }

    protected override void OnComplete(WorldState state)
    {
        string partnerId = state.ContainsKey("TradePartnerId") ? state["TradePartnerId"]?.ToString() : null;
        int price = state.GetInt("TradePrice", 5);
        int foodAmount = state.GetInt("TradeFoodAmount", 1);

        bool ok = false;

        if (SocialBoard.Instance != null && self != null && !string.IsNullOrEmpty(partnerId))
        {
            ok = SocialBoard.Instance.ExecuteTrade(self.agentId, partnerId, price, foodAmount);
        }

        state["TradeCompleted"] = ok;

        // Limpieza
        state["TradeAcceptedByPartner"] = false;
        state["HasIncomingTradeRequest"] = false;
        state["IsNearPartner"] = false;
        state["TradeRequested"] = false;


    }
}
