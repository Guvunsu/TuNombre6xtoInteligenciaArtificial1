using UnityEngine;
using System.Collections.Generic;

public class GOADAgent : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private GOADAgentConfiguration config;

    [Header("Identity")]
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

        Debug.Log($"[{agentId}] Awake. Acciones encontradas: {actions.Count}");
    }

    private void Start()
    {
        if (SocialBoard.Instance != null)
        {
            SocialBoard.Instance.Register(this);
            Debug.Log($"[{agentId}] Registrado en SocialBoard");
        } else
        {
            Debug.LogError($"[{agentId}] SocialBoard.Instance es NULL");
        }

        SetPlan();
    }

    private void Update()
    {
        if (SocialBoard.Instance != null)
            SocialBoard.Instance.PumpInboxIntoWorldState(this);

        if (currentPlan == null || currentPlan.Count == 0)
        {
            Debug.Log($"[{agentId}] No tengo plan. Intentando replanear...");
            SetPlan();
            if (currentPlan == null || currentPlan.Count == 0)
                return;
        }

        GOAPAction action = currentPlan.Peek();
        Debug.Log($"[{agentId}] Ejecutando acci�n: {action.GetType().Name}");

        bool done = action.Perform(worldState);

        if (done)
        {
            Debug.Log($"[{agentId}] Acci�n completada: {action.GetType().Name}");
            currentPlan.Dequeue();
        }
    }

    private void ApplyConfig()
    {
        worldState.Clear();

        foreach (var b in config.initialBools)
        {
            worldState[b.key] = b.value;
            Debug.Log($"[{agentId}] Init Bool: {b.key} = {b.value}");
        }

        foreach (var i in config.initialInts)
        {
            worldState[i.key] = i.value;
            Debug.Log($"[{agentId}] Init Int: {i.key} = {i.value}");
        }

        goals.Clear();
        foreach (var gdef in config.goals)
        {
            var g = new GOAPGoal(gdef.name, gdef.priority);
            foreach (var kv in gdef.desiredBools)
                g.DesiredState[kv.key] = kv.value;

            goals.Add(g);

            Debug.Log($"[{agentId}] Goal cargado: {gdef.name}");
        }

        // Defaults para claves de interacci�n social (evita ContainsKey missing)
        if (!worldState.ContainsKey("TradePartnerId")) worldState["TradePartnerId"] = "";
        if (!worldState.ContainsKey("TradePrice")) worldState["TradePrice"] = 0;
        if (!worldState.ContainsKey("TradeFoodAmount")) worldState["TradeFoodAmount"] = 0;

        // Tambi�n es buena idea asegurar flags usados por precondiciones
        if (!worldState.ContainsKey("TradeRequested")) worldState["TradeRequested"] = false;
        if (!worldState.ContainsKey("TradeAcceptedByPartner")) worldState["TradeAcceptedByPartner"] = false;
        if (!worldState.ContainsKey("IsNearPartner")) worldState["IsNearPartner"] = false;
        if (!worldState.ContainsKey("HasIncomingTradeRequest")) worldState["HasIncomingTradeRequest"] = false;

    }

    private void SetPlan()
    {
        foreach (var a in actions)
            a.ResetAction();

        GOAPGoal goal = ChooseGoal();
        if (goal == null)
        {
            Debug.LogWarning($"[{agentId}] SetPlan: No goal activo (�ya satisfechos o no hay goals?)");
            currentPlan = null;
            return;
        }

        Debug.Log($"[{agentId}] SetPlan: Planeando goal={goal.Name} con {actions.Count} acciones...");

        currentPlan = planner.Plan(worldState, actions, goal, agentId);

        if (currentPlan == null)
        {
            Debug.LogWarning($"[{agentId}] SetPlan: planner devolvi� NULL");
            return;
        }

        Debug.Log($"[{agentId}] SetPlan: plan tiene {currentPlan.Count} pasos");
        foreach (var a in currentPlan)
            Debug.Log($"[{agentId}]  -> {a.GetType().Name}");
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
