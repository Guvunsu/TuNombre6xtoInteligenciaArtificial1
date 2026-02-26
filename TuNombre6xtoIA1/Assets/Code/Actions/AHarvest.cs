using UnityEngine;
using static UICanvasEmotions;

public class AHarvest : GOAPAction
{
    private Animator animator;
    private UICanvasEmotions script_UICanvasEmotions;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        script_UICanvasEmotions = GetComponentInChildren<UICanvasEmotions>();
        duration = 10f;
        AddPrecondition("AgentIsClose", true);

        AddEffect("GetMoney", true);
        AddEffect("IsFull", false);
        AddEffect("IsTired", true);

        cost = 1f;
    }
    void ResetAnimations()
    {
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsEating", false);
        animator.SetBool("IsSleeping", false);
        animator.SetBool("IsGetFood", false);
        animator.SetBool("IsWorking", true);
    }
    protected override void OnTick(WorldState state, float t01, float elapsed)
    {
        // opcional: algo durante, sonido, UI, etc.
        // Debug.Log($"AEat progress {t01:0.00}");
    }
    protected override void OnStart(WorldState state)
    {
        // Al iniciar movimiento, ya no estamos "close"
        state["AgentIsClose"] = false;

        Debug.Log("AHarvest: start");
        //tarea 09 02 2026
        ResetAnimations();
        if (animator != null)
        {
            animator.SetBool("IsWorking", true);
            script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.BUSY);
        }
    }

    protected override void OnComplete(WorldState state)
    {
        Debug.Log("AHarvest: complete");

        //tarea 09 02 2026
        if (animator != null)
        {
            animator.SetBool("IsWorking", false);
            script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.SADNESS);
        }
        state["AgentIsClose"] = true;
        state["GetMoney"] = true;
        state["IsFull"] = false;
        state["IsTired"] = true;
    }
}