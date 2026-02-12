using System;

/// <summary>
/// Operadores de comparación para precondiciones numéricas.
/// Ejemplo: Food >= 1
/// </summary>
public enum CompareOp
{
    Equals,
    NotEquals,
    Greater,
    GreaterOrEqual,
    Less,
    LessOrEqual
}

/// <summary>
/// Tipos de efectos numéricos.
/// Ejemplo: Food += 1, Money -= 5
/// </summary>
public enum EffectOp
{
    Set,
    Add,
    Subtract
}

/// <summary>
/// Precondición numérica.
/// Se evalúa contra el WorldState.
/// </summary>
[Serializable]
public struct GOAPCondition
{
    public string key;
    public CompareOp op;
    public int intValue;

    /// <summary>
    /// Evalúa la condición contra el estado actual.
    /// </summary>
    public bool Evaluate(WorldState state)
    {
        int current = state.GetInt(key, 0);

        return op switch
        {
            CompareOp.Equals => current == intValue,
            CompareOp.NotEquals => current != intValue,
            CompareOp.Greater => current > intValue,
            CompareOp.GreaterOrEqual => current >= intValue,
            CompareOp.Less => current < intValue,
            CompareOp.LessOrEqual => current <= intValue,
            _ => false
        };
    }
}

/// <summary>
/// Efecto numérico que modifica el estado.
/// </summary>
[Serializable]
public struct GOAPNumericEffect
{
    public string key;
    public EffectOp op;
    public int intValue;

    /// <summary>
    /// Aplica el efecto al estado real.
    /// </summary>
    public void Apply(WorldState state)
    {
        int current = state.GetInt(key, 0);
        int next = current;

        switch (op)
        {
            case EffectOp.Set: next = intValue; break;
            case EffectOp.Add: next = current + intValue; break;
            case EffectOp.Subtract: next = current - intValue; break;
        }

        state[key] = next;
    }

    /// <summary>
    /// Se usa SOLO en el planner para ver
    /// si este efecto cambiaría el estado simulado.
    /// </summary>
    public bool WouldChange(WorldState state)
    {
        int current = state.GetInt(key, 0);
        int next = current;

        switch (op)
        {
            case EffectOp.Set: next = intValue; break;
            case EffectOp.Add: next = current + intValue; break;
            case EffectOp.Subtract: next = current - intValue; break;
        }

        return next != current;
    }
}