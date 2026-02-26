using UnityEngine;
using static UICanvasEmotions;

public class AGetFood : GOAPAction
{
    private Animator animator;
    private UICanvasEmotions script_UICanvasEmotions;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        script_UICanvasEmotions = GetComponentInChildren<UICanvasEmotions>();
        duration = 10;

        AddPrecondition("AgentIsClose", true);

        AddEffect("HasFood", true);

        cost = 1f;
    }
    void ResetAnimations()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsSleeping", false);
        animator.SetBool("IsWorking", false);
        animator.SetBool("IsEating", false);
        animator.SetBool("IsGetFood", true);
    }
    protected override void OnTick(WorldState state, float t01, float elapsed)
    {
        // opcional: algo durante, sonido, UI, etc.
        // Debug.Log($"AEat progress {t01:0.00}");
    }
    protected override void OnStart(WorldState state)
    {
        Debug.Log("AGetFood: start");

        if (animator == null) return;

        state["AgentIsClose"] = false;
        ResetAnimations();
        if (animator != null)
        {
            animator.SetBool("IsGetFood", true);
            //script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.BUSY);
        }
    }
    protected override void OnComplete(WorldState state)
    {
        Debug.Log("AGetFood: complete");

        if (animator != null)
        {
            animator.SetBool("IsGetFood", false);
            //script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.BUSY);
        }

        state["AgentIsClose"] = true;
        state["HasFood"] = true;
    }
}
