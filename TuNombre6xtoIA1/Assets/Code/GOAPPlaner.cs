using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GOAPPlaner
{
    public Queue<GOAPAction> Plan(WorldState worldState, List<GOAPAction> avaibleActions, GOAPGoal goal)
    {

        List<GOAPAction> plan = new List<GOAPAction>();
        WorldState currentState = new WorldState();
        foreach (var kv in worldState)
            currentState[kv.Key] = kv.Value;

        int safety = 0;//modificar despues aqui para evitar bucle infinitos

        while (!GoalSatisfied(currentState, goal.DesiredState) && safety < 10)
        {
            safety++;
            // buscara las mejores acciones 
            GOAPAction bestAction = null;
            bool foundImprovingAction = false;

            foreach (var action in avaibleActions)
            {
                //1) Revisamos que se cumplan las precondiciones en el estado actual
                if (!action.ArePreconditionsMet(currentState))
                    continue;

                //2) Debemos cambiar el estado (si no cambia nada, lo ignoramos)
                bool changeState = false;
                foreach (var effects in action.Effects)
                {
                    //tengo un efecto que se llama "hacer del baño"
                    //para seguirla tomando en cuenta su efecto 
                    // a) si no tiene que existir en el estado actual 
                    // o
                    // b) si existe tiene que tener un valor distinto 
                    if (!currentState.ContainsKey(effects.Key) || !currentState[effects.Key].Equals(effects.Value))
                    {
                        changeState = true;
                        break;
                    }
                }
                if (changeState) { continue; }
                //3) Mejora el goal? pone le value en el valor deseado 
                bool improvesGoal = false;

                foreach (var effect in action.Effects)
                {
                    //si el goal es "apagar el incendio" uno de los estados pdoria ser "animalsAreSafe = true;"
                    // Action "putDownFire [GastarAgua, GastarEnergy, animalsAreSafe = true]
                    if (goal.DesiredState.ContainsKey(effect.Key) && effect.Value.Equals(goal.DesiredState[effect.Key]))
                    {
                        improvesGoal = true;
                        break;
                    }
                }
                // 4) seleccion:
                //si encontramos acciones que mejoran el goal, las priorisamos

                if (improvesGoal)
                {
                    if (!foundImprovingAction || bestAction == null || action.cost < bestAction.cost)
                    {
                        bestAction = action;
                        foundImprovingAction = true;
                    }
                } else if (!foundImprovingAction)
                {
                    if (bestAction == null || action.cost < bestAction.cost)
                    {
                        bestAction = action;
                    }
                }
            }
            if (bestAction == null)
            {
                Debug.LogWarning("Planner:No encontre una ninguna accion aplicable.");
                return null;
            }
            foreach (var effect in bestAction.Effects)
            {
                currentState[effect.Key] = effect.Value;
            }
            plan.Add(bestAction);
        }
        if (!GoalSatisfied(currentState, goal.DesiredState))
        {
            Debug.LogWarning("PLanner:No se puede alcanzar el goal");
            return null;
        }

        Debug.LogWarning("PLanner: Plan generado con " + plan.Count + "acciones");
        return new Queue<GOAPAction>(plan);
    }
    private bool GoalSatisfied(WorldState state, Dictionary<string, object> goalState)
    {
        foreach (var goal in goalState)
        {
            // goalState: isForrestBurning == false
            // debere primero preguntar primero si existe isForrestBurning
            // state (world): isForrestBurning != null
            if (!state.ContainsKey(goal.Key)) return false;
            if (!state[goal.Key].Equals(goal.Value)) return false;
        }
        return true;
    }
}
