using UnityEngine;
using UnityEngine.AI;
using static UICanvasEmotions;

public class AMoveToAgent : GOAPAction
{
    private const string PartnerKey = "TradePartnerId";

    private NavMeshAgent nav;
    private Transform target;
    private Animator animator;
    [SerializeField] UICanvasEmotions script_UICanvasEmotions;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        script_UICanvasEmotions = GetComponentInChildren<UICanvasEmotions>();

        duration = 0f;
        cost = 1f;

        AddPrecondition("TradeRequested", true);
        AddPrecondition("IsNearPartner", false);

        AddEffect("IsNearPartner", true);
    }

    protected override void OnStart(WorldState state)
    { 
        if (animator != null )
        {
            animator.SetBool("IsWalking", true);
            script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.BUSY);
        }
        if (!state.ContainsKey(PartnerKey))
        {
            Debug.LogError("No TradePartnerId in state! Keys:");
            foreach (var kv in state)
                Debug.Log($"  {kv.Key}={kv.Value}");
            return;
        }

        string partnerId = state[PartnerKey]?.ToString();
        if (string.IsNullOrEmpty(partnerId))
        {
            Debug.LogError("TradePartnerId exists but is empty. Keys:");
            foreach (var kv in state)
                Debug.Log($"  {kv.Key}={kv.Value}");
            return;
        }

        var partner = SocialBoard.Instance.GetAgent(partnerId);
        if (partner == null)
        {
            Debug.LogError("Partner not found in SocialBoard: " + partnerId);
            return;
        }

        target = partner.transform;

        if (nav == null)
        {
            Debug.LogError("NavMeshAgent missing!");
            return;
        }

        nav.isStopped = false;
        nav.SetDestination(target.position);

        Debug.Log("Moving toward " + partnerId);
    }

    protected override bool CheckComplete(WorldState state, float t01, float elapsed)
    {
        if (nav == null || target == null) return false;
        if (nav.pathPending) return false;

        if (nav.remainingDistance <= nav.stoppingDistance &&
            nav.velocity.sqrMagnitude < 0.01f)
        {
            if (animator != null)
                animator.SetBool("IsWalking", false);
            script_UICanvasEmotions.SetMood(EmotionReferenceInAgent.SADNESS);
            return true;
        }

        return false; 
    }

    protected override void OnComplete(WorldState state)
    {
        if (nav != null)
        {
            nav.isStopped = true;
            nav.ResetPath();
            state["IsNearPartner"] = true;
            Debug.Log("Arrived to partner.");
        }
    }
}
