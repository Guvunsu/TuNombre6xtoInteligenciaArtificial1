using UnityEngine;

public class AEat : GOAPAction
{
    private Animator animator;

    private void Awake()
    {

        animator = GetComponent<Animator>();
        duration = 3.33f;

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
        if (animator != null) animator.SetBool("IsEating", true);
    }

    protected override void OnTick(WorldState state, float t01, float elapsed)
    {
        // opcional: algo durante, sonido, UI, etc.
        // Debug.Log($"AEat progress {t01:0.00}");
    }

    protected override void OnComplete(WorldState state)
    {
        Debug.Log("AEat: complete");
        if (animator != null) animator.SetBool("IsEating", false);

        state["IsFull"] = true;
        state["HasFood"] = false;
        state["IsTired"] = false;
    }
}