using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BoolStateEntry
{
    public string key;
    public bool value;
}

[Serializable]
public class IntStateEntry
{
    public string key;
    public int value;
}

[Serializable]
public class GoalDefinition
{
    public string name;
    [Range(0f, 10f)] public float priority = 1f;
    public List<BoolStateEntry> desiredBools = new();
}

[CreateAssetMenu(fileName = "GOAPAgentConfig", menuName = "Scriptable Objects/GOAPAgentConfig")]
public class GOADAgentConfiguration : ScriptableObject
{
    [Header("Initial State (Bools)")]
    public List<BoolStateEntry> initialBools = new();

    [Header("Initial State (Ints)")]
    public List<IntStateEntry> initialInts = new();

    [Header("Goals")]
    public List<GoalDefinition> goals = new();
}
