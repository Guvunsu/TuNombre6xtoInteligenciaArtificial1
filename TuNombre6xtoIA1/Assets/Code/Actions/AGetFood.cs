using UnityEngine;

public class AGetFood : GOAPAction
{
    private bool done = false;

    private void Awake()
    {
        AddPrecondition("AgentIsClose", true);
        AddEffect("HasFood", true);
        cost = 1f;
    }

    public override bool Perform(WorldState state)
    {
        if (!done)
        {
            Debug.Log("SIM: NPC goes to get food");
            state["HasFood"] = true;
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
