using UnityEngine;
using UnityEngine.AI;

public class AMoveToAgent : GOAPAction
{
    private NavMeshAgent nav;
    private Transform target;

    [Header("Who to move to (worldState key)")]
    public string partnerIdKey = "TradePartnerId";

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        duration = 0f; // termina por condición (llegar)
        AddEffect("IsNearPartner", true);
        cost = 1f;
    }

    protected override void OnStart(WorldState state)
    {
        state["IsNearPartner"] = false;

        string partnerId = state.ContainsKey(partnerIdKey) ? state[partnerIdKey]?.ToString() : null;
        if (string.IsNullOrEmpty(partnerId))
        {
            target = null;
            return;
        }

        var partner = SocialBoard.Instance.GetAgent(partnerId);
        target = partner != null ? partner.transform : null;

        if (target != null && nav != null)
        {
            nav.isStopped = false;
            nav.SetDestination(target.position);
        }
    }

    protected override bool CheckComplete(WorldState state, float t01, float elapsed)
    {
        if (nav == null || target == null) return true;
        if (nav.pathPending) return false;

        if (nav.remainingDistance <= nav.stoppingDistance)
        {
            if (!nav.hasPath || nav.velocity.sqrMagnitude < 0.01f)
                return true;
        }
        return false;
    }

    protected override void OnComplete(WorldState state)
    {
        if (nav != null) nav.isStopped = true;
        state["IsNearPartner"] = true;
    }
}