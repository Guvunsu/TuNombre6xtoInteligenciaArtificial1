//using UnityEngine;
//using System.Collections.Generic;
//using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;

//public class GOAPAgent : MonoBehaviour
//{
//    [Header("Configuration")]
//    [SerializeField]
//    GOADAgentConfiguration configuration;

//    public WorldState worldState = new WorldState();

//    private List<GOAPAction> actions = new List<GOAPAction>();
//    private List<GOAPGoal> goals = new List<GOAPGoal>();

//    private GOAPPlaner planner = new GOAPPlaner();
//    private Queue<GOAPAction> currentPlan;

//    //public bool autoReplan = false;
//    void ApplyConfiguration()
//    {
//        if (configuration == null)
//        {
//            Debug.LogWarning("There is no GOADConfiguration assigned");
//            return;
//        }
//        //definimos el estado inicial
//        worldState.Clear();
//        foreach (var element in configuration.initialBools)
//        {
//            worldState[element.key] = element.value;
//        }

//        //definimos los goals
//        goals.Clear();
//        foreach (var elementGoal in configuration.goals)
//        {
//            var g = new GOAPGoal(elementGoal.name, elementGoal.priority);
//            foreach (var kv in elementGoal.desireBools)
//            {
//                goals.Add(g);
//            }
//        }

//    }
//    private void Awake()
//    {
//        actions.AddRange(GetComponents<GOAPAction>());
//        ApplyConfiguration();
//    }

//    private void SetPlan()
//    {

//        if (goals.Count == 0)
//        {
//            Debug.LogWarning("no goals set");
//            return;
//        }
//        //reset acciones
//        foreach (var action in actions)
//            action.ResetAction();

//        var goal = ChooseGoal();

//        if (goal == null)
//        {
//            currentPlan = null;
//            Debug.Log("Agent: todos los goals ya fueron completados");
//            return;
//        }
//        currentPlan = planner.Plan(worldState, actions, goal); //creo que es goapPlanner.Plan

//        if (currentPlan == null || currentPlan.Count == 0)
//        {
//            currentPlan = null;
//            Debug.Log("Agent: No se encontro plan para " + goal.Name);
//            return;
//        }
//        Debug.Log("Agent: plan creado para " + goal.Name);
//    }
//    private void Update()
//    {
//        if (currentPlan == null || currentPlan.Count == 0)
//        {
//            SetPlan();
//            if (currentPlan == null || currentPlan.Count == 0)
//            {
//                return;
//            }
//        }
//        GOAPAction action = currentPlan.Peek();

//        // Ejecuta la acción. Debe devolver true cuando termina.
//        bool done = action.Perform(worldState);

//        if (done)
//        {
//            Debug.Log("GoapAgent: acción completada -> " + action.GetType().Name);
//            currentPlan.Dequeue();
//        }
//    }
//    private GOAPGoal ChooseGoal()
//    {
//        //ordenamos por prioridad 
//        goals.Sort((a, b) => b.Priority.CompareTo(a.Priority));

//        //agarramos el primer goal que NO esta satisfecho 
//        //esto porque puede que el de mayor prioridad, ya esra satisfied
//        foreach (var g in goals)
//        {
//            if (!GoalIsSatisfied(worldState, g.DesiredState)) return g;
//        }
//        return null;
//    }
//    private bool GoalIsSatisfied(WorldState state, Dictionary<string, object> goalState)
//    {
//        foreach (var g in goalState)
//        {
//            if (!state.ContainsKey(g.Key)) return false;
//            if (!state[g.Key].Equals(g.Value)) return false;
//        }
//        return true;
//    }
//}
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Video;

public class GOAPAgent : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private GOADAgentConfiguration config;

    public WorldState worldState = new WorldState();

    private List<GOAPAction> actions = new List<GOAPAction>();
    private List<GOAPGoal> goals = new List<GOAPGoal>();

    private GOAPPlanner planner = new GOAPPlanner();
    private Queue<GOAPAction> currentPlan;




    private void ApplyConfig()
    {
        if (config == null)
        {
            Debug.LogWarning("There is no GOAPConfig assigned!");
            return;
        }

        // Definimos el estado inicial
        worldState.Clear();
        foreach (var element in config.initialBools)
            worldState[element.key] = element.value;

        // Definimos los Goals
        goals.Clear();
        foreach (var elementGoal in config.goals)
        {
            var g = new GOAPGoal(elementGoal.name, elementGoal.priority);
            foreach (var kv in elementGoal.desireBools)
                g.DesiredState[kv.key] = kv.value;

            goals.Add(g);
        }
    }

    private void Awake()
    {
        actions.AddRange(GetComponents<GOAPAction>());
        ApplyConfig();
    }

    void Start()
    {
        SetPlan();
    }

    private void SetPlan()
    {
        if (goals.Count == 0)
        {
            Debug.LogWarning("No goals set");
            return;
        }

        // Reset de acciones
        foreach (var action in actions)
            action.ResetAction();

        var goal = ChooseGoal();

        if (goal == null)
        {
            currentPlan = null;
            Debug.Log("Agent: todos los goals ya fueron completados");
            return;
        }

        currentPlan = planner.Plan(worldState, actions, goal);

        if (currentPlan == null || currentPlan.Count == 0)
        {
            currentPlan = null;
            Debug.LogError("Agent: plan nulo o vacio para: " + goal.Name);
            return;
        }

        Debug.Log("Agent: Plan creado para " + goal.Name);
    }

    private void Update()
    {
        //un debug rapido para ver que pex con mis acciones
        Debug.Log("WORLD STATE:");
        foreach (var kv in worldState)
        {
            Debug.Log(kv.Key + " = " + kv.Value);
        }


        if (currentPlan == null || currentPlan.Count == 0)
        {
            SetPlan();
            if (currentPlan == null || currentPlan.Count == 0)
                return;
        }


        GOAPAction action = currentPlan.Peek();

        // Ejecuta la acción. Debe devolver true cuando termina.
        bool done = action.Perform(worldState);

        if (done)
        {
            Debug.Log("GoapAgent: acción completada -> " + action.GetType().Name);
            currentPlan.Dequeue();
            //tarea 09 02 2026 loop para replantear el plan cuando este haya acabado con todos
            if (currentPlan.Count == 0)
            {
                Debug.Log("GoapAgent: plan terminado, replanteando...");
                SetPlan();
            }
        }
    }

    private GOAPGoal ChooseGoal()
    {
        // Ordenamos por prioridad
        goals.Sort((a, b) => b.Priority.CompareTo(a.Priority));

        // Agarramos el primer goal que NO esté satisfecho.
        foreach (var g in goals)
        {
            if (!GoalIsSatisfied(worldState, g.DesiredState))
                return g;
        }

        return null;
    }

    private bool GoalIsSatisfied(WorldState state, Dictionary<string, object> goalState)
    {
        foreach (var g in goalState)
        {
            if (!state.ContainsKey(g.Key)) return false;
            if (!state[g.Key].Equals(g.Value)) return false;
        }

        return true;
    }
}
