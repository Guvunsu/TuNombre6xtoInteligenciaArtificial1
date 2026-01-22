using UnityEngine;
using System.Collections.Generic;
public class GOAPAgent : MonoBehaviour
{
    public WorldState worldState = new WorldState();
    private List<GOAPAction> actions = new List<GOAPAction>();
    private List<GOAPAction> goals = new List<GOAPAction>();

    private GOAPPlaner goapPlanner = new GOAPPlaner();
    private Queue<GOAPAction> currentPlan;

    void Start()
    {
        //1) estado inicial
        worldState["HasFood"] = false;
        worldState["IsFull"] = false;

        //2) obtengo mis accciones disponibles 
        actions.AddRange(GetComponents<GOAPAction>());

        //3) creamos el goal: isFull = true 
        var eatGoal = new GOAPGoal("Eat", 1f);
        eatGoal.DesiredState["IsFull"] = true;
        goals.Add(eatGoal);

        //4) Planificar
        SetPlan();
    }

    private void SetPlan()
    {
        if (goals.Count == 0)
        {
            Debug.LogWarning("no goals set");
            return;
        }
        GOAPGoal goal = goals[0];
        currentPlan = planner.Plan(worldState, actions, goal);
        if (currentPlan == null)
        {
            Debug.LogError("Agent: No se encontro plan para " + goal.Name);
        } else Debug.LogError("Agent: plan creado para " + goal.Name);
    }
    private void Update()
    {

        GOAPAction action = currentPlan.Peek();

        // Ejecuta la acción. Debe devolver true cuando termina.
        bool done = action.Perform(worldState);

        if (done)
        {
            Debug.Log("GoapAgent: acción completada -> " + action.GetType().Name);
            currentPlan.Dequeue();
        }
    }
}
