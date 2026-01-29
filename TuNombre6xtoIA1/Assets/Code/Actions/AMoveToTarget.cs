using UnityEngine;
using UnityEngine.AI;

public class AMoveToTarget : GOAPAction
{
    private NavMeshAgent agent;
    private Transform target;

    private bool isMoving = false;
    private bool done = false;

    public string targetName;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // Buscamos el target en la escena
        target = GameObject.Find(targetName).transform;

        AddEffect("AgentIsClose", true);

        cost = 1f;
    }

    public override bool Perform(WorldState state)
    {
        if (!isMoving)
        {
            Debug.Log("SM: Move to target");
            agent.isStopped = false;
            agent.SetDestination(target.position);
            isMoving = true;
        }

        // El agente ya llegó?
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Debug.Log("SM: I am here!");
            agent.isStopped = true;
            state["AgentIsClose"] = true;
            done = true;
        }

        return done;
    }

    public override void ResetAction()
    {
        done = false;
    }

    public override bool IsDone() { return done; }
}
