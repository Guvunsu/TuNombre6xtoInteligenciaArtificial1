using UnityEngine;
using System.Collections.Generic;

public class GOADAgent : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private GOADAgentConfiguration config;

    [Header("Social")]
    public string agentId = "AgentA";

    public WorldState worldState = new WorldState();

    private readonly List<GOAPAction> actions = new();
    private readonly List<GOAPGoal> goals = new();

    private readonly GOAPPlanner planner = new GOAPPlanner();
    private Queue<GOAPAction> currentPlan;

    private void Awake()
    {
        actions.AddRange(GetComponents<GOAPAction>());
        ApplyConfig();

        SocialBoard.Instance.Register(this);
    }

    private void Start()
    {
        SetPlan();
    }

    private void Update()
    {
        // 1) primero procesa mensajes entrantes (convierte eventos -> estado)
        SocialBoard.Instance.PumpInboxIntoWorldState(this);

        // 2) ejecuta GOAP
        if (currentPlan == null || currentPlan.Count == 0)
        {
            SetPlan();
            if (currentPlan == null || currentPlan.Count == 0)
                return;
        }

        GOAPAction action = currentPlan.Peek();
        bool done = action.Perform(worldState);

        if (done)
        {
            currentPlan.Dequeue();
        }
    }

    private void ApplyConfig()
    {
        if (config == null)
        {
            Debug.LogWarning("There is no GOAPConfig assigned!");
            return;
        }

        worldState.Clear();

        foreach (var b in config.initialBools)
            worldState[b.key] = b.value;

        foreach (var i in config.initialInts)
            worldState[i.key] = i.value;

        goals.Clear();
        foreach (var gdef in config.goals)
        {
            var g = new GOAPGoal(gdef.name, gdef.priority);
            foreach (var kv in gdef.desiredBools)
                g.DesiredState[kv.key] = kv.value;
            goals.Add(g);
        }
    }

    private void SetPlan()
    {
        if (goals.Count == 0)
        {
            currentPlan = null;
            return;
        }

        foreach (var a in actions)
            a.ResetAction();

        GOAPGoal goal = ChooseGoal();
        if (goal == null)
        {
            currentPlan = null;
            return;
        }

        currentPlan = planner.Plan(worldState, actions, goal);

        if (currentPlan == null || currentPlan.Count == 0)
        {
            currentPlan = null;
            return;
        }
    }

    private GOAPGoal ChooseGoal()
    {
        goals.Sort((a, b) => b.Priority.CompareTo(a.Priority));

        foreach (var g in goals)
        {
            if (!GoalSatisfied(worldState, g.DesiredState))
                return g;
        }
        return null;
    }

    private bool GoalSatisfied(WorldState state, Dictionary<string, object> goalState)
    {
        foreach (var goal in goalState)
        {
            if (!state.ContainsKey(goal.Key)) return false;
            if (!state[goal.Key].Equals(goal.Value)) return false;
        }
        return true;
    }
}