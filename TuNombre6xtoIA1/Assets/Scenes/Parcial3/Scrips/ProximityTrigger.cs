using UnityEngine;

public class ProximityTrigger : MonoBehaviour
{
    public string agentId; // asignar desde GOAPAgent.agentId en Awake
    private GOAPAgent owner;

    private void Awake()
    {
        owner = GetComponentInParent<GOAPAgent>();
        if (owner != null) agentId = owner.agentId;
    }

    private void OnTriggerEnter(Collider other)
    {
        var otherAgent = other.GetComponentInParent<GOAPAgent>();
        if (otherAgent == null) return;

        owner.worldState["AgentIsClose"] = true;
        owner.worldState["TradePartnerId"] = otherAgent.agentId;
    }

    private void OnTriggerExit(Collider other)
    {
        var otherAgent = other.GetComponentInParent<GOAPAgent>();
        if (otherAgent == null) return;

        owner.worldState["AgentIsClose"] = false;
        owner.worldState["TradePartnerId"] = null;
    }
}
