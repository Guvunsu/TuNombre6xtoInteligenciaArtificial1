using UnityEngine;
using UnityEngine.AI;

public class APolicePatrol : GOAPAction
{
    public string[] patrolPoints;
    private int currentIndex = 0;
    private NavMeshAgent agent;
    private Transform target;
    private Animator animator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        duration = 0f;
        cost = 1f;

        // Esta acción no requiere estar cerca de un agente; es una actividad propia
        AddEffect("Patrolling", true);
        AddEffect("AgentIsClose", false); // opcional: mientras patrulla no está cerca de alguien
    }

    protected override void OnStart(WorldState state)
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogWarning("APolicePatrol: no patrolPoints asignados.");
            return;
        }

        var go = GameObject.Find(patrolPoints[currentIndex]);
        if (go == null)
        {
            Debug.LogWarning("APolicePatrol: patrol point not found: " + patrolPoints[currentIndex]);
            return;
        }

        target = go.transform;
        if (agent != null)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }

        if (animator != null)
            animator.SetBool("IsWalking", true);
    }

    protected override bool CheckComplete(WorldState state, float t01, float elapsed)
    {
        if (agent == null || target == null) return true;
        if (agent.pathPending) return false;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude < 0.01f)
                return true;
        }
        return false;
    }

    protected override void OnComplete(WorldState state)
    {
        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if (animator != null)
            animator.SetBool("IsWalking", false);

        // Avanza al siguiente punto
        currentIndex = (currentIndex + 1) % patrolPoints.Length;

        // Marca que está patrullando
        state["Patrolling"] = true;
        state["AgentIsClose"] = false;
    }
}
