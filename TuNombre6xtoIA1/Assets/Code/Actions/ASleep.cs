using UnityEngine;
using static UICanvasEmotions;

public class ASleep : GOAPAction
{
    private Animator animator;
    private UICanvasEmotions script_UICanvasEmotions;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        script_UICanvasEmotions = GetComponent<UICanvasEmotions>();
        duration = 7;

        AddPrecondition("IsTired", true);
        AddPrecondition("AgentIsClose", true);

        AddEffect("IsTired", false);
        AddEffect("IsRested", true);

        cost = 1f;
    }
    void ResetAnimations()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsWorking", false);
        animator.SetBool("IsEating", false);
        animator.SetBool("IsGetFood", false);
        animator.SetBool("IsSleeping", true);
    }
    protected override void OnTick(WorldState state, float t01, float elapsed)
    {
        // opcional: algo durante, sonido, UI, etc.
        // Debug.Log($"AEat progress {t01:0.00}");
    }
    protected override void OnStart(WorldState state)
    {
        Debug.Log("ASleep: start");

        if (animator == null) return;

        state["IsTired"] = false;
        state["AgentIsClose"] = false;
        ResetAnimations();
        if (animator != null)
        {
            animator.SetBool("IsSleeping", true);
            //script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.SADNESS);
        }
    }

    protected override void OnComplete(WorldState state)
    {
        Debug.Log("ASleep: complete");

        if (animator != null)
        {
            animator.SetBool("IsSleeping", false);
            //script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.HAPPYNESS);
        }
        state["AgentIsClose"] = true;
        state["IsTired"] = true;
    }
}
