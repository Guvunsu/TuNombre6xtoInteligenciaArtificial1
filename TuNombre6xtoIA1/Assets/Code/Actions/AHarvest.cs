using UnityEngine;

public class AHarvest : GOAPAction
{
    private bool done = false;
    private void Awake()
    {
        AddPrecondition("AgentIsClose", true);
        AddPrecondition("IsFull", true);
        AddEffects("GetMoney", true);
        AddPrecondition("IsFull", false);
        cost = 1f;
    }
    public override bool Perform(WorldState state)
    {
        //throw new System.NotImplementedException();
        if (!done)
        {
            Debug.Log("SIM: NPC goes to get Harvesyt");
            //Aqui se pondria la logica de la accion
            //ej: mover el personaje, abrir objeto, gastar dinero,animacion,vfx,etc

            state["GetMoney"] = true;
            done = true;
        }
        return done;
    }
    public override bool IsDone()
    {
        //throw new System.NotImplementedException();
        return done;
    }
}
