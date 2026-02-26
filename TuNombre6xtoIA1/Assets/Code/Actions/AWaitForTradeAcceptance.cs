using UnityEngine;
using static UICanvasEmotions;

public class AWaitForTradeAcceptance : GOAPAction
{
    private Animator animator;
    [SerializeField] UICanvasEmotions script_UICanvasEmotions;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        script_UICanvasEmotions = GetComponentInChildren<UICanvasEmotions>();
        // No usamos duration para terminar; terminamos por condición.
        duration = 0f;
        cost = 0.1f;

        // Permitimos que el planner la tome aunque su cambio sea "externo"
        // (pero aquí sí cambiamos el estado simulado con un Effect).
        allowNoStateChange = false;

        AddPrecondition("TradeRequested", true);
        AddPrecondition("TradeAcceptedByPartner", false);

        // CLAVE:
        // Esto permite que el planner "avance" el estado simulado
        // y pueda planear ExecuteTrade -> Eat.
        AddEffect("TradeAcceptedByPartner", true);
    }

    protected override void OnStart(WorldState state)
    {
        if (animator != null)
        {
            animator.SetBool("IsTrading", true);
            script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.HAPPYNESS);
        }
    }
    protected override bool CheckComplete(WorldState state, float t01, float elapsed)
    {
        // En runtime no “inventamos” la aceptación.
        // Esperamos hasta que SocialBoard haya puesto esto en true.
        return state.ContainsKey("TradeAcceptedByPartner") &&
               state["TradeAcceptedByPartner"].Equals(true);
    }

    protected override void OnComplete(WorldState state)
    {
        if (animator != null)
        {
            animator.SetBool("IsTrading", false);
            script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.BUSY);
        }
        // No hacemos nada: solo “esperar”.
    }
}
