using UnityEngine;

public class AEat : GOAPAction
{
    private bool done = false;
    private Animator animator;
    private bool anim_started = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();

        AddPrecondition("HasFood", true);
        AddPrecondition("IsFull", false);

        AddEffect("IsFull", true);
        AddEffect("HasFood", false);

        cost = 1f;
    }

    public override bool Perform(WorldState state)
    {
        if (!anim_started)
        {
            Debug.Log("Comiendo");
            animator.SetBool("IsEating", true);
            anim_started = true;
        }

        if (!done)
        {
            Debug.Log("SIM: NPC is eating");
            state["IsFull"] = true;
            state["HasFood"] = false;

            done = true;
        }

        return done;
    }

    public override void ResetAction()
    {
        done = false;
    }

    public override bool IsDone()
    {
        return done;
    }
}
