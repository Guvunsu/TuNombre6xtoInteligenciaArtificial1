using UnityEngine;
using UnityEngine.AI;
using static UICanvasEmotions;

public class ARunning : GOAPAction
{
    private Animator animator;
    private UICanvasEmotions script_UICanvasEmotions;

    public int foodConsumed = 1;
    public float eatDuration = 0;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        script_UICanvasEmotions = GetComponent<UICanvasEmotions>();
        duration = eatDuration;

        AddPrecondition("AgentIsClose", true);

        AddEffect("Sit&Talking", false);
        AddEffect("IsFull", false);
        AddEffect("HasFood", false);
        AddEffect("IsTired", false);
        AddEffect("Running", true);

        cost = 1f;
    }
    void ResetAnimations()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsSleeping", false);
        animator.SetBool("IsWorking", false);
        animator.SetBool("IsGetFood", false);
        animator.SetBool("IsEating", false);
        animator.SetBool("IsRunning", true);
    }
    protected override void OnStart(WorldState state)
    {
        Debug.Log("ARunning: start");
        ResetAnimations();
        if (animator != null)
        {
            animator.SetBool("IsRunning", true);
            script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.HAPPYNESS);
        }
    }
    protected override void OnTick(WorldState state, float t01, float elapsed)
    {
        // opcional: algo durante, sonido, UI, etc.
        // Debug.Log($"AEat progress {t01:0.00}");
    }
    protected override void OnComplete(WorldState state)
    {
        Debug.Log("ARunning: complete");
        if (animator != null)
        {
            animator.SetBool("IsRunning", false);
            script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.BUSY);
        }
        state["IsFull"] = true;
        foreach (var e in NumericEffects)
        {
            e.Apply(state);
        }
    }
}
