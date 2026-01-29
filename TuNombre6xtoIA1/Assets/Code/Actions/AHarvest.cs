using UnityEngine;

public class AHarvest : GOAPAction
{
    private bool done = false;

    private void Awake()
    {
        AddPrecondition("AgentIsClose", true);
        AddPrecondition("IsFull", true);

        AddEffect("GetMoney", true);
        AddEffect("IsFull", false);

        cost = 1f;
    }

    public override bool Perform(WorldState state)
    {
        if (!done)
        {
            Debug.Log("SIM: NPC goes to harvest");
            state["GetMoney"] = true;
            state["IsFull"] = false;
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
