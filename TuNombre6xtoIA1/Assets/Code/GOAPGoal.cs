using System.Collections.Generic;

public class GOAPGoal
{
    public string Name;
    public Dictionary<string, object> DesiredState;
    public float Priority;

    public GOAPGoal(string name, float priority)
    {
        Name = name;
        Priority = priority;
        DesiredState = new Dictionary<string, object>();
    }
}
