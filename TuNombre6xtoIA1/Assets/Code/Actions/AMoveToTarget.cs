using UnityEngine;
using UnityEngine.AI;

public class AMoveToTarget : GOAPAction
{
    private NavMeshAgent agent;
    private Transform target;

    public string targetName;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // duration = 0 => no termina por tiempo, termina por CheckComplete()
        duration = 0f;

        // Este move lo usamos como acci�n gen�rica de "acercarse"
        AddEffect("AgentIsClose", true);
        cost = 1f;
    }

    protected override void OnReset()
    {
        // si quieres, limpiar destino/flags aqu�
        if (agent != null) agent.isStopped = false;
    }

    protected override void OnStart(WorldState state)
    {
        var go = GameObject.Find(targetName);
        if (go == null)
        {
            Debug.LogWarning("AMoveToTarget: targetName no encontrado: " + targetName);
            return;
        }

        target = go.transform;

        Debug.Log("AMoveToTarget: moving to " + targetName);

        // Al iniciar movimiento, ya no estamos "close"
        state["AgentIsClose"] = false;

        agent.isStopped = false;
        agent.SetDestination(target.position);
    }

    protected override void OnTick(WorldState state, float t01, float elapsed)
    {
        // Si quieres, aqu� actualizas animaci�n de caminar (opcional)
        // Ej: animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    protected override bool CheckComplete(WorldState state, float t01, float elapsed)
    {
        if (agent == null) return true;
        if (target == null) return true; // no hay target, no te quedes colgado

        if (agent.pathPending) return false;

        // Lleg�
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // A veces falta checar velocity para evitar falsos positivos
            if (!agent.hasPath || agent.velocity.sqrMagnitude < 0.01f)
                return true;
        }

        return false;
    }

    protected override void OnComplete(WorldState state)
    {
        Debug.Log("AMoveToTarget: arrived");

        if (agent != null)
            agent.isStopped = true;

        state["AgentIsClose"] = true;
    }
}
