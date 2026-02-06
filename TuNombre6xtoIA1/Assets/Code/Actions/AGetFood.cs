using UnityEngine;

public class AGetFood : GOAPAction
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

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

    protected override void OnStart(WorldState state)
    {
        Debug.Log("AGetFood: start");

        if (animator == null) return;

        state["AgentIsClose"] = false;
        ResetAnimations();
        animator.SetBool("IsGetFood", true);
    }

    protected override void OnComplete(WorldState state)
    {
        Debug.Log("AGetFood: complete");

        if (animator != null)
            animator.SetBool("IsGetFood", false);

        state["AgentIsClose"] = true;
        state["HasFood"] = true;
    }
}
