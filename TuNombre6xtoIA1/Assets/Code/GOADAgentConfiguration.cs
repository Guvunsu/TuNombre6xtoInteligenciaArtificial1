using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class BoolStateEntry
{
    public string key;
    public bool value;
}
[Serializable]
public class GoalDefinition
{
    public string name;
    [Range(0f, 10f)] public float priority = 1f;
    public List<BoolStateEntry> desireBools = new();
}
[CreateAssetMenu(fileName = "GOADAgentConfiguration", menuName = "Scriptable Objects/GOADAgentConfiguration")]
public class GOADAgentConfiguration : ScriptableObject
{
    [Header("Initial State")]
    public List<BoolStateEntry> initialBools = new();

    [Header("Goals")]
    public List<GoalDefinition> goals = new();
}
