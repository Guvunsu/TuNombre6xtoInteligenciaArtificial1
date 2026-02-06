using UnityEngine;

public class AHarvest : GOAPAction
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
    protected override void OnStart(WorldState state)
    {
        // Al iniciar movimiento, ya no estamos "close"
        state["AgentIsClose"] = false;

        Debug.Log("AHarvest: start");
        //tarea 09 02 2026
        ResetAnimations();
        if (animator != null)
            animator.SetBool("IsWorking", true);
    }

    protected override void OnComplete(WorldState state)
    {
        Debug.Log("AHarvest: complete");

        //tarea 09 02 2026
        if (animator != null)
            animator.SetBool("IsWorking", false);

        // aplica efectos reales
        state["AgentIsClose"] = true;
        state["GetMoney"] = true;
        state["IsFull"] = false;
        state["IsTired"] = true;
    }

}