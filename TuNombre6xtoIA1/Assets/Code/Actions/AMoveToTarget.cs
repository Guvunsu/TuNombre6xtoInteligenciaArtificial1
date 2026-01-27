using UnityEngine;
using UnityEngine.AI;

public class AMoveToTarget : GOAPAction
{
    private NavMeshAgent agent;
    private Transform target;

    private bool isMoving = false;
    private bool done = false;

    public string targetName;

    private Animator animator;
    private bool anim_started = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.Find(targetName).transform;

        AddEffects("AgentIsClose", true);
        cost = 1;
    }
    public override bool Perform(WorldState state)
    {
        if (!isMoving)
        {
            Debug.Log("SM: Move to target");
            agent.isStopped = false;
            agent.SetDestination(target.position);
            isMoving = true;
            animator.SetBool("IsWalking", true);
            anim_started = true;
        }
        //el agente ya llego?
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Debug.Log("SM: IM HERE");
            agent.isStopped = true;
            state["agentIsClose"] = true;
            done = true;
            animator.SetBool("IsWalking", false);
            anim_started = false;
        }
        return done;
    }
    public override bool IsDone()
    {
        return done;
    }
}
