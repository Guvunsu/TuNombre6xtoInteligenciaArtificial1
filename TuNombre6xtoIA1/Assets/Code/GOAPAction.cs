using System.Collections.Generic;
using UnityEngine;

// Clase base para todas las acciones GOAP.
// El PADRE controla el flujo/tiempo. Las HIJAS definen comportamiento con hooks.
public abstract class GOAPAction : MonoBehaviour
{
    public float cost = 1f;

    [Header("Timing")]
    [Tooltip("0 = no usa duración fija (termina por condición en CheckComplete). >0 = termina también por tiempo.")]
    public float duration = 0f;

    protected Dictionary<string, object> preconditions = new Dictionary<string, object>();
    protected Dictionary<string, object> effects = new Dictionary<string, object>();

    public Dictionary<string, object> Preconditions => preconditions;
    public Dictionary<string, object> Effects => effects;

    // Runtime
    private bool running = false;
    private float startTime = -1f;
    private bool completed = false;

    protected void AddPrecondition(string key, object value) => preconditions[key] = value;
    protected void AddEffect(string key, object value) => effects[key] = value;

    public virtual bool ArePreconditionsMet(WorldState state)
    {
        foreach (var precondition in preconditions)
        {
            if (!state.ContainsKey(precondition.Key)) return false;
            if (!state[precondition.Key].Equals(precondition.Value)) return false;
        }
        return true;
    }

    /// <summary>
    /// Resetea la acción para que se pueda reutilizar en otro plan.
    /// </summary>
    public virtual void ResetAction()
    {
        running = false;
        startTime = -1f;
        completed = false;
        OnReset();
    }

    /// <summary>
    /// Hook opcional por si una acción quiere limpiar flags propios.
    /// </summary>
    protected virtual void OnReset() { }

    /// <summary>
    /// El agente SIEMPRE llama este Perform (de la clase padre).
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

        // Termina por condición
        if (CheckComplete(state, t01, elapsed))
        {
            Finish(state);
            return true;
        }

        // Termina por tiempo (si duration > 0)
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

    // Hooks que implementan las hijas
    protected abstract void OnStart(WorldState state);

    /// <summary>
    /// Se llama cada frame mientras la acción corre.
    /// t01 = progreso 0..1 si duration>0, si duration==0 t01=0.
    /// elapsed = segundos desde que inició.
    /// </summary>
    protected virtual void OnTick(WorldState state, float t01, float elapsed) { }

    /// <summary>
    /// Si devuelves true aquí, la acción termina (útil para MoveTo, esperar a evento, etc.).
    /// Por defecto no termina por condición.
    /// </summary>
    protected virtual bool CheckComplete(WorldState state, float t01, float elapsed) => false;

    /// <summary>
    /// Se llama una vez al terminar.
    /// </summary>
    protected abstract void OnComplete(WorldState state);

    // Compatibilidad/debug (opcional)
    public bool IsDone() => completed;
}