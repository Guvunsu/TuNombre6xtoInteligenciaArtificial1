using UnityEngine;
using static UICanvasEmotions;

public class AEat : GOAPAction
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
        AddNumericPrecondition("Food", CompareOp.GreaterOrEqual, foodConsumed);
        AddEffect("IsFull", true);
        AddNumericEffect("Food", EffectOp.Subtract, foodConsumed);

        AddPrecondition("AgentIsClose", true);
        AddPrecondition("HasFood", true);

        AddEffect("IsFull", true);
        AddEffect("HasFood", false);
        AddEffect("IsTired", false);

        cost = 1f;
    }
    void ResetAnimations()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsSleeping", false);
        animator.SetBool("IsWorking", false);
        animator.SetBool("IsGetFood", false);
        animator.SetBool("IsEating", true);
    }
    protected override void OnStart(WorldState state)
    {
        Debug.Log("AEat: start");
        ResetAnimations();
        if (animator != null)
        {
            animator.SetBool("IsEating", true);
            //script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.HAPPYNESS);
        }
    }
    protected override void OnTick(WorldState state, float t01, float elapsed)
    {
        // opcional: algo durante, sonido, UI, etc.
        // Debug.Log($"AEat progress {t01:0.00}");
    }
    protected override void OnComplete(WorldState state)
    {
        Debug.Log("AEat: complete");
        if (animator != null)
        {
            animator.SetBool("IsEating", false);
            //script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.HAPPYNESS);
        }
        state["IsFull"] = true;
      foreach(var e in NumericEffects)
        {
            e.Apply(state);
        }
    }
}