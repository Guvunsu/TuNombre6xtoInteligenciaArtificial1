using UnityEngine;
using UnityEngine.AI;
public class ASitTalking : GOAPAction
{
    private Animator animator;
    private UICanvasEmotions script_UICanvasEmotions;

    public float sitTalkingDuration = 13f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        script_UICanvasEmotions = GetComponentInChildren<UICanvasEmotions>();
        duration = sitTalkingDuration;

        AddPrecondition("AgentIsClose", true);

        AddEffect("Sit&Talking", true);
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
        animator.SetBool("Sit&Talking", true);
    }
    protected override void OnStart(WorldState state)
    {
        state["AgentIsClose"] = false;
        Debug.Log("Sit&Talking: start");
        ResetAnimations();
        if (animator != null)
        {
            animator.SetBool("Sit&Talking", true);
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
        Debug.Log("Sit&Talking: complete");
        if (animator != null)
        {
            animator.SetBool("Sit&Talking", false);
            //script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.HAPPYNESS);
        }
        state["Sit&Talking"] = true;
        state["AgentIsClose"] = true;
        state["GetMoney"] = false;
        state["IsFull"] = false;
        state["IsTired"] = false;
    }
}
