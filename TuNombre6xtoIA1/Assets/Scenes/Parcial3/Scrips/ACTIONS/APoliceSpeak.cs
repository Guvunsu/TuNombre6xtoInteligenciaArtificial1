using UnityEngine;
using static UICanvasEmotions;

public class APoliceSpeak : GOAPAction
{
    private GOAPAgent self;
    private Animator animator;
    [SerializeField] UICanvasEmotions script_UICanvasEmotions;

    private void Awake()
    {
        self = GetComponent<GOAPAgent>();
        animator = GetComponent<Animator>();
        script_UICanvasEmotions = GetComponentInChildren<UICanvasEmotions>();

        duration = 1.0f; // tiempo de conversación
        cost = 1f;

        // Requiere estar cerca del otro agente
        AddPrecondition("AgentIsClose", true);

        // Requiere que el otro agente permita hablar (el citizen debe tener su precondición equivalente)
        AddPrecondition("CitizenWillingToTalk", true);

        // Efectos: obtiene dinero (marca) y limpia handshake
        AddEffect("GetMoney", true);
        AddEffect("HasSpokenWithCitizen", true);

        // Consumir el permiso para evitar loops
        AddEffect("CitizenWillingToTalk", false);
        AddEffect("AgentIsClose", false);
    }

    protected override void OnStart(WorldState state)
    {
        if (animator != null)
        {
            animator.SetBool("IsTalking", true);
            if (script_UICanvasEmotions != null)
                script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.BUSY);
        }
    }

    protected override bool CheckComplete(WorldState state, float t01, float elapsed)
    {
        // Simple: termina por duración
        return elapsed >= duration;
    }

    protected override void OnComplete(WorldState state)
    {
        if (animator != null)
        {
            animator.SetBool("IsTalking", false);
            if (script_UICanvasEmotions != null)
                script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.HAPPYNESS);
        }

        // Efecto ya agregado en Awake, pero asegurar estado final
        state["GetMoney"] = true;
        state["HasSpokenWithCitizen"] = true;

        // Limpieza de flags de interacción
        state["CitizenWillingToTalk"] = false;
        state["AgentIsClose"] = false;

        // Si quieres, aquí puedes invocar SocialBoard para transferencias económicas
        // Ejemplo (opcional): SocialBoard.Instance.TransferMoney(self.agentId, citizenId, amount);
    }
}
