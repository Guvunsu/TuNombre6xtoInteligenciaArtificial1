using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//sera la clase base para todas las acciones del GOAP
//Cada accion tiene: coste, prediccion, y efectos
public class GOAPAction : MonoBehaviour
{
    public float cost = 1f;
    protected Dictionary<string, object> preconditions = new Dictionary<string, object>();
    protected Dictionary<string, object> effects = new Dictionary<string, object>();

    public Dictionary<string, object> Preconditions => preconditions;
    public Dictionary<string, object> Effects => effects;

    protected void AddPrecondition(string key, object value)
    {
        preconditions[key] = value;
    }
    protected void AddEffects(string key, object value)
    {
        effects[key] = value;
    }
    public virtual bool ArePreconditionsMet(WorldState state)
    {
        foreach (var precondition in preconditions)
        {
            if (!state.ContainsKey(precondition.Key)) return false;
            if (!state[precondition.Key].Equals(precondition.Value)) return false;
        }
        return true;
    }
    public abstract bool Perform(WorldState state);
    public abstract bool IsDone();
}
