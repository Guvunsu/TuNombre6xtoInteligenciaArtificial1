using UnityEngine;

public class AGetFood : GOAPAction
{
    private void Awake()
    {
        duration = 1.5f;

        AddPrecondition("AgentIsClose", true);
        AddEffect("HasFood", true);

        cost = 1f;
    }

    protected override void OnStart(WorldState state)
    {
        Debug.Log("AGetFood: start");
    }

    protected override void OnComplete(WorldState state)
    {
        Debug.Log("AGetFood: complete");
        state["HasFood"] = true;
    }
}
