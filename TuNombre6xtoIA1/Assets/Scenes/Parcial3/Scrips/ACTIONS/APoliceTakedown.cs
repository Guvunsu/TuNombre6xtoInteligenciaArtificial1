using UnityEngine;
using static UICanvasEmotions;

public class APoliceTakedown : GOAPAction
{
    private GOAPAgent self;
    private Animator animator;
    [SerializeField] UICanvasEmotions script_UICanvasEmotions;

    private void Awake()
    {
        self = GetComponent<GOAPAgent>();
        animator = GetComponent<Animator>();
        script_UICanvasEmotions = GetComponentInChildren<UICanvasEmotions>();

        duration = 0.5f;
        cost = 2f; // más costosa que hablar

        // Requisitos: estar cerca y que el objetivo sea un ladrón (la clave TargetIsThief debe ser puesta por el world/agent)
        AddPrecondition("AgentIsClose", true);
        AddPrecondition("TargetIsThief", true);

        // Efectos: captura y cansancio
        AddEffect("CapturedThief", true);
        AddEffect("IsTired", true);

        // Consumir el estado de cercanía/objetivo para evitar loops
        AddEffect("AgentIsClose", false);
        AddEffect("TargetIsThief", false);
    }

    protected override void OnStart(WorldState state)
    {
        if (animator != null)
        {
            animator.SetTrigger("Takedown");
            if (script_UICanvasEmotions != null)
                script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.BUSY);
        }
    }

    protected override bool CheckComplete(WorldState state, float t01, float elapsed)
    {
        return elapsed >= duration;
    }

    protected override void OnComplete(WorldState state)
    {
        // Marca captura en el estado
        state["CapturedThief"] = true;
        state["IsTired"] = true;

        // Aquí puedes notificar al SocialBoard para que actualice el thief (ej. marcarlo arrestado)
        // if (SocialBoard.Instance != null) SocialBoard.Instance.ReportCapture(self.agentId, targetId);

        // Limpieza
        state["AgentIsClose"] = false;
        state["TargetIsThief"] = false;

        if (animator != null)
        {
            if (script_UICanvasEmotions != null)
                script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.SADNESS);
        }
    }
}
