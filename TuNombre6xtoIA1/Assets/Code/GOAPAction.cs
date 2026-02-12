using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase base para TODAS las acciones GOAP.
/// Controla el flujo:
/// - inicio
/// - ejecución
/// - duración
/// - finalización
///
/// Las acciones hijas SOLO describen comportamiento.
/// </summary>
public abstract class GOAPAction : MonoBehaviour
{
    public float cost = 1f;

    [Header("Timing")]
    // Si duration = 0, la acción termina solo por condición (CheckComplete)
    public float duration = 0f;

    // Precondiciones y efectos booleanos
    protected Dictionary<string, object> preconditions = new();
    protected Dictionary<string, object> effects = new();

    // Precondiciones y efectos numéricos
    protected readonly List<GOAPCondition> numericPreconditions = new();
    protected readonly List<GOAPNumericEffect> numericEffects = new();

    public Dictionary<string, object> Preconditions => preconditions;
    public Dictionary<string, object> Effects => effects;
    public IReadOnlyList<GOAPCondition> NumericPreconditions => numericPreconditions;
    public IReadOnlyList<GOAPNumericEffect> NumericEffects => numericEffects;

    // Estado interno de ejecución
    private bool running = false;
    private float startTime = -1f;
    private bool completed = false;

    protected void AddPrecondition(string key, object value)
        => preconditions[key] = value;

    protected void AddEffect(string key, object value)
        => effects[key] = value;

    protected void AddNumericPrecondition(string key, CompareOp op, int value)
        => numericPreconditions.Add(new GOAPCondition { key = key, op = op, intValue = value });

    protected void AddNumericEffect(string key, EffectOp op, int value)
        => numericEffects.Add(new GOAPNumericEffect { key = key, op = op, intValue = value });

    /// <summary>
    /// Verifica TODAS las precondiciones (bool + numéricas).
    /// </summary>
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

    /// <summary>
    /// Se llama antes de replanear para poder reutilizar la acción.
    /// </summary>
    public virtual void ResetAction()
    {
        running = false;
        startTime = -1f;
        completed = false;
        OnReset();
    }

    protected virtual void OnReset() { }

    /// <summary>
    /// ESTE método lo llama el GOAPAgent.
    /// Nunca se sobrescribe en las hijas.
    /// </summary>
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

        // Finaliza por condición
        if (CheckComplete(state, t01, elapsed))
        {
            Finish(state);
            return true;
        }

        // Finaliza por tiempo
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

    // Hooks que implementan las acciones hijas
    protected abstract void OnStart(WorldState state);
    protected virtual void OnTick(WorldState state, float t01, float elapsed) { }
    protected virtual bool CheckComplete(WorldState state, float t01, float elapsed) => false;
    protected abstract void OnComplete(WorldState state);
}