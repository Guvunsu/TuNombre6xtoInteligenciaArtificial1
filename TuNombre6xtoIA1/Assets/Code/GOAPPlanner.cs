using System.Collections.Generic;
using UnityEngine;

public class GOAPPlanner
{
    private const int MAX_STEPS = 50;

    public Queue<GOAPAction> Plan(
        WorldState worldState,
        List<GOAPAction> availableActions,
        GOAPGoal goal,
        string agentId)
    {
        if (goal == null)
        {
            Debug.LogWarning($"[{agentId}] Planner: goal NULL.");
            return null;
        }

        if (availableActions == null || availableActions.Count == 0)
        {
            Debug.LogWarning($"[{agentId}] Planner: no available actions.");
            return null;
        }

        // Copia del estado para simular
        WorldState currentState = CopyState(worldState);

        // Si ya está satisfecho, plan vacío es válido
        if (GoalSatisfied(currentState, goal.DesiredState))
            return new Queue<GOAPAction>();

        List<GOAPAction> plan = new List<GOAPAction>();

        // Anti-loop: guarda una “firma” del estado para detectar estancamiento
        HashSet<string> visited = new HashSet<string>();

        for (int step = 0; step < MAX_STEPS; step++)
        {
            if (GoalSatisfied(currentState, goal.DesiredState))
                return new Queue<GOAPAction>(plan);

            string signature = StateSignature(currentState);
            if (!visited.Add(signature))
            {
                // Ya vimos este estado -> estamos en loop
                Debug.LogWarning($"[{agentId}] Planner: loop detectado en step={step}. Estado repetido.");
                return null;
            }

            GOAPAction bestAction = null;
            float bestScore = float.NegativeInfinity;

            foreach (var action in availableActions)
            {
                if (!action.ArePreconditionsMet(currentState))
                    continue;

                // Si no cambia nada y no se permite, se ignora
                bool changesState = WouldChangeState(action, currentState);

                if (!changesState && !action.allowNoStateChange)
                    continue;

                // Heurística simple:
                // - preferir acciones que cumplen partes del goal
                // - preferir acciones que cambian estado (progreso real)
                // - preferir costo bajo
                int goalHits = CountGoalHits(action, goal);
                float score =
                    goalHits * 100f +              // prioridad máxima: acercarse al goal
                    (changesState ? 10f : -5f) +    // progreso > espera
                    (-action.cost);                 // menor costo = mejor

                if (bestAction == null || score > bestScore)
                {
                    bestAction = action;
                    bestScore = score;
                }
            }

            if (bestAction == null)
            {
                Debug.LogWarning($"[{agentId}] Planner: No encontré acción aplicable en step={step}.");
                return null;
            }

            // Aplicar efectos simulados
            ApplyEffects(bestAction, currentState);

            plan.Add(bestAction);
        }

        Debug.LogWarning($"[{agentId}] Planner: Safety limit ({MAX_STEPS}) reached. Goal not satisfied.");
        return null;
    }

    // ---------- helpers ----------

    private WorldState CopyState(WorldState src)
    {
        WorldState dst = new WorldState();
        foreach (var kv in src)
            dst[kv.Key] = kv.Value;
        return dst;
    }

    private bool GoalSatisfied(WorldState state, Dictionary<string, object> goalState)
    {
        foreach (var g in goalState)
        {
            if (!state.ContainsKey(g.Key)) return false;
            if (!state[g.Key].Equals(g.Value)) return false;
        }
        return true;
    }

    private bool WouldChangeState(GOAPAction action, WorldState state)
    {
        // bool/object effects
        foreach (var e in action.Effects)
        {
            if (!state.ContainsKey(e.Key) || !state[e.Key].Equals(e.Value))
                return true;
        }

        // numeric effects
        foreach (var ne in action.NumericEffects)
        {
            if (ne.WouldChange(state))
                return true;
        }

        return false;
    }

    private int CountGoalHits(GOAPAction action, GOAPGoal goal)
    {
        int hits = 0;
        foreach (var e in action.Effects)
        {
            if (goal.DesiredState.TryGetValue(e.Key, out var desired) && Equals(desired, e.Value))
                hits++;
        }
        return hits;
    }

    private void ApplyEffects(GOAPAction action, WorldState state)
    {
        foreach (var e in action.Effects)
            state[e.Key] = e.Value;

        foreach (var ne in action.NumericEffects)
            ne.Apply(state);
    }

    private string StateSignature(WorldState state)
    {
        // Firma estable para detectar loops (ordenada por key)
        List<string> parts = new List<string>(state.Count);
        foreach (var kv in state)
            parts.Add($"{kv.Key}={kv.Value}");

        parts.Sort();
        return string.Join("|", parts);
    }
}
