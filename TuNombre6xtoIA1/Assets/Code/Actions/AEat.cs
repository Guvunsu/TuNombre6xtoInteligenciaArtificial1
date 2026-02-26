// ===============================
// AEat.cs (SIN CAMBIOS IMPORTANTES, SOLO COHERENTE)
// - Requiere Food >= foodConsumed y luego consume comida.
// ===============================
using UnityEngine;

public class AEat : GOAPAction
{
    public int foodConsumed = 1;
    public float eatDuration = 2.0f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        duration = eatDuration;
        cost = 1f;

        AddNumericPrecondition("Food", CompareOp.GreaterOrEqual, foodConsumed);

        AddEffect("IsFull", true);
        AddNumericEffect("Food", EffectOp.Subtract, foodConsumed);
    }

    protected override void OnReset()
    {
        if (animator != null)
            animator.SetBool("IsEating", false);
    }

    protected override void OnStart(WorldState state)
    {
        if (animator != null)
            animator.SetBool("IsEating", true);
    }

    protected override void OnComplete(WorldState state)
    {
        if (animator != null)
            animator.SetBool("IsEating", false);

        state["IsFull"] = true;

        foreach (var e in NumericEffects)
            e.Apply(state);
    }
}
