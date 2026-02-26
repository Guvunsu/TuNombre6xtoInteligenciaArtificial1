using UnityEngine;
using UnityEngine.AI;

public class APray : GOAPAction
{
    private Animator animator;
    private UICanvasEmotions script_UICanvasEmotions;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        script_UICanvasEmotions = GetComponentInChildren<UICanvasEmotions>();
        duration = 6.66f;
        AddEffect("IsPray", true);

        AddPrecondition("AgentIsClose", true);

        AddEffect("Sit&Talking", false);
        AddEffect("IsFull", false);
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
        animator.SetBool("IsEating", false);
        animator.SetBool("Sit&Talking", false);
        animator.SetBool("IsPray", true);
    }
    protected override void OnStart(WorldState state)
    {
        state["AgentIsClose"] = false;
        Debug.Log("IsPray: start");
        ResetAnimations();
        if (animator != null)
        {
            animator.SetBool("IsPray", true);
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
        Debug.Log("IsPray: complete");
        if (animator != null)
        {
            animator.SetBool("IsPray", false);
            //script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.HAPPYNESS);
        }
        state["AgentIsClose"] = true;
        state["IsPray"] = true;                                 
        state["Sit&Talking"] = false;
        state["GetMoney"] = false;
        state["IsFull"] = false;
        state["IsTired"] = false;
    }
}
