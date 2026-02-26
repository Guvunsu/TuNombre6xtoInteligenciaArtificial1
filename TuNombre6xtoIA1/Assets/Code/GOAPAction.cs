// ===============================
// GOAPAction.cs (CORREGIDO)
// - Agrega allowNoStateChange para permitir acciones tipo "Wait" que no cambian el estado.
// ===============================
using System.Collections.Generic;
using UnityEngine;

public abstract class GOAPAction : MonoBehaviour
{
    public float cost = 1f;

    [Header("Timing")]
    [Tooltip("0 = termina solo por condición (CheckComplete). >0 = también puede terminar por tiempo.")]
    public float duration = 0f;

    [Header("Planner")]
    [Tooltip("Si es true, el planner puede elegir esta acción aunque no cambie el estado (útil para Wait/Listen).")]
    public bool allowNoStateChange = false;

    protected Dictionary<string, object> preconditions = new();
    protected Dictionary<string, object> effects = new();

    protected readonly List<GOAPCondition> numericPreconditions = new();
    protected readonly List<GOAPNumericEffect> numericEffects = new();

    public Dictionary<string, object> Preconditions => preconditions;
    public Dictionary<string, object> Effects => effects;
    public IReadOnlyList<GOAPCondition> NumericPreconditions => numericPreconditions;
    public IReadOnlyList<GOAPNumericEffect> NumericEffects => numericEffects;

    private bool running = false;
    private float startTime = -1f;
    private bool completed = false;

    protected void AddPrecondition(string key, object value) => preconditions[key] = value;
    protected void AddEffect(string key, object value) => effects[key] = value;

    protected void AddNumericPrecondition(string key, CompareOp op, int value)
        => numericPreconditions.Add(new GOAPCondition { key = key, op = op, intValue = value });

    protected void AddNumericEffect(string key, EffectOp op, int value)
        => numericEffects.Add(new GOAPNumericEffect { key = key, op = op, intValue = value });

    public virtual bool ArePreconditionsMet(WorldState state)
    {
        foreach (var p in preconditions)
        {
            if (!state.ContainsKey(p.Key)) return false;
            if (!state[p.Key].Equals(p.Value)) return false;
        }

        foreach (var c in numericPreconditions)
        {
            if (!c.Evaluate(state))
                return false;
        }

        return true;
    }

    public virtual void ResetAction()
    {
        running = false;
        startTime = -1f;
        completed = false;
        OnReset();
    }

    protected virtual void OnReset() { }

    public bool Perform(WorldState state)
    {
        if (completed) return true;

        if (!running)
        {
            running = true;
            startTime = Time.time;
            OnStart(state);
        }

        float elapsed = Time.time - startTime;
        float t01 = (duration > 0f) ? Mathf.Clamp01(elapsed / duration) : 0f;

        OnTick(state, t01, elapsed);

        if (CheckComplete(state, t01, elapsed))
        {
            Finish(state);
            return true;
        }

        if (duration > 0f && elapsed >= duration)
        {
            Finish(state);
            return true;
        }

        return false;
    }

    private void Finish(WorldState state)
    {
        if (completed) return;
        completed = true;
        running = false;
        OnComplete(state);
    }

    protected abstract void OnStart(WorldState state);
    protected virtual void OnTick(WorldState state, float t01, float elapsed) { }
    protected virtual bool CheckComplete(WorldState state, float t01, float elapsed) => false;
    protected abstract void OnComplete(WorldState state);

    public bool IsDone() => completed;
}
