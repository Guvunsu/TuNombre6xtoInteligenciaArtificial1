using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;

public class GOAPAgent : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField]
    GOADAgentConfiguration configuration;

    public WorldState worldState = new WorldState();

    private List<GOAPAction> actions = new List<GOAPAction>();
    private List<GOAPGoal> goals = new List<GOAPGoal>();

    private GOAPPlaner planner = new GOAPPlaner();
    private Queue<GOAPAction> currentPlan;

    //public bool autoReplan = false;
    void ApplyConfiguration()
    {
        if (configuration == null)
        {
            Debug.LogWarning("There is no GOADConfiguration assigned");
            return;
        }
        //definimos el estado inicial
        worldState.Clear();
        foreach (var element in configuration.initialBools)
        {
            worldState[element.key] = element.value;
        }

        //definimos los goals
        goals.Clear();
        foreach (var elementGoal in configuration.goals)
        {
            var g = new GOAPGoal(elementGoal.name, elementGoal.priority);
            foreach (var kv in elementGoal.desireBools)
            {
                goals.Add(g);
            }
        }

    }
    private void Awake()
    {
        actions.AddRange(GetComponents<GOAPAction>());
    }
    void Start()
    {
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
        currentPlan = planner.Plan(worldState, actions, goal); //creo que es goapPlanner.Plan

        if (currentPlan == null) { Debug.Log("Agent: No se encontro plan para " + goal.Name); } else Debug.Log("Agent: plan creado para " + goal.Name);
    }
    private void Update()
    {
        if (currentPlan == null || currentPlan.Count == 0)
        {
            //if (autoReplan)
            //{
            //}
                SetPlan();
                return;
        }

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
