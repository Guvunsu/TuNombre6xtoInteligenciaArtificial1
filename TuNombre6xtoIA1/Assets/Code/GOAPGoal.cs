using System.Collections.Generic;


/// Define un objetivo (goal) que el agente quiere alcanzar.
/// Tiene un nombre, una prioridad y un estado deseado (otra "foto" del mundo).

public class GOAPGoal
{
    public string Name;
    public Dictionary<string, object> DesiredState;
    public float Priority; // Para futuro: decidir entre varios goals

    public GOAPGoal(string name, float priority)
    {
        Name = name;
        Priority = priority;
        DesiredState = new Dictionary<string, object>();
    }
}
