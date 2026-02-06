using UnityEngine;

public class ASleep : GOAPAction
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        duration = 15f; // ⏱ tiempo de dormir

        // Preconditions
        AddPrecondition("IsTired", true);
        AddPrecondition("AgentIsClose", true); // cama / casa

        // Effects
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

    protected override void OnStart(WorldState state)
    {
        Debug.Log("ASleep: start");

        if (animator == null) return;

        state["IsTired"] = false;
        state["AgentIsClose"] = false;
        ResetAnimations();
        animator.SetBool("IsSleeping", true);
    }

    protected override void OnComplete(WorldState state)
    {
        Debug.Log("ASleep: complete");

        if (animator != null)
            animator.SetBool("IsSleeping", false);

  
        state["AgentIsClose"] = true; 
        state["IsTired"] = true;
        //state["IsRested"] = true;
    }
}
