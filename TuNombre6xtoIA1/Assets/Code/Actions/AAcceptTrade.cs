using UnityEngine;
using static UICanvasEmotions;

public class AAcceptTrade : GOAPAction
{
    private GOAPAgent self;
    private Animator animator;

    [SerializeField] private UICanvasEmotions script_UICanvasEmotions;

    private void Awake()
    {
        self = GetComponent<GOAPAgent>();
        animator = GetComponent<Animator>();

        // Si no fue asignado desde Inspector, lo busca automáticamente
        if (script_UICanvasEmotions == null)
            script_UICanvasEmotions = GetComponentInChildren<UICanvasEmotions>();

        duration = 0.2f;
        cost = 1f;

        AddPrecondition("HasIncomingTradeRequest", true);
        AddNumericPrecondition("Food", CompareOp.GreaterOrEqual, 1);

        AddEffect("TradeAccepted", true);
    }

    protected override void OnStart(WorldState state)
    {
        if (animator != null)
            animator.SetBool("IsSearching", true);

        if (script_UICanvasEmotions != null)
            script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.BUSY);
    }

    protected override void OnComplete(WorldState state)
    {
        if (animator != null)
            animator.SetBool("IsTrading", true);

        if (script_UICanvasEmotions != null)
            script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.HAPPYNESS);

        state["TradeAccepted"] = true;

        SendAcceptanceMessage(state);
    }

    private void SendAcceptanceMessage(WorldState state)
    {
        string partnerId = state.ContainsKey("TradePartnerId")
            ? state["TradePartnerId"]?.ToString()
            : null;

        if (string.IsNullOrEmpty(partnerId)) return;
        if (SocialBoard.Instance == null) return;

        SocialBoard.Instance.Send(new SocialMessage
        {
            type = SocialMessageType.TradeAccepted,
            fromAgentId = self != null ? self.agentId : "",
            toAgentId = partnerId
        });
    } 
}