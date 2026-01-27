using UnityEngine;

public class AEat : GOAPAction
{
    private bool done = false;
    private Animator animator;
    private bool anim_started = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        AddPrecondition("HasFood", true);

        AddEffects("IsFull", true);
        AddEffects("HasFood", false);
    }
    public override bool Perform(WorldState state)
    {
        if (!anim_started)
        {
            animator.SetBool("IsEating", true);
            anim_started = true;
        }
        //aqui esta en buble ya que no sale cuando empieza con la animacion 
        // podemos decirle que cuando acabe la animacion se salga
        //igual podemos hacerlo salir a base de tiempo time.DeltaTime
        //throw new System.NotImplementedException();
        if (!done)
        {
            Debug.Log("SIM: NPC is eating");


            state["IsFull"] = true;
            state["HasFood"] = false;
            done = true;
        }
        return done;
    }
    public override bool IsDone()
    {
        //throw new System.NotImplementedException();
        return done;
    }
}
