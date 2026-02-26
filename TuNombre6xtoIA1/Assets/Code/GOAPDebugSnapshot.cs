using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Presiona una tecla (por defecto F1) para imprimir un snapshot del WorldState:
/// - del agente en este GameObject
/// - y opcionalmente de otros agentes por agentId
/// </summary>
public class GOAPDebugSnapshot : MonoBehaviour
{
    [Header("Input")]
    public KeyCode snapshotKey = KeyCode.F1;

    [Header("Who to print")]
    [Tooltip("Si está vacío, solo imprime el agente de este GameObject (si existe).")]
    public List<string> additionalAgentIds = new List<string> { "AgentA", "AgentB" };

    [Header("Options")]
    public bool printSocialBoardAgentsList = true;

    private GOAPAgent self;

    private void Awake()
    {
        self = GetComponent<GOAPAgent>();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(snapshotKey))
            return;

        Debug.Log("===== GOAP SNAPSHOT =====");

        if (printSocialBoardAgentsList)
            PrintRegisteredAgents();

        if (self != null)
        {
            Debug.Log($"--- SELF ({self.agentId}) ---");
            PrintWorldState(self.worldState, self.agentId);
        }
        else
        {
            Debug.LogWarning("No GOAPAgent found on this GameObject.");
        }

        if (SocialBoard.Instance == null)
        {
            Debug.LogWarning("SocialBoard.Instance is null.");
            Debug.Log("=========================");
            return;
        }

        // Imprime agentes adicionales (por id)
        foreach (var id in additionalAgentIds)
        {
            if (string.IsNullOrEmpty(id)) continue;

            var agent = SocialBoard.Instance.GetAgent(id);
            if (agent == null)
            {
                Debug.LogWarning($"--- Agent '{id}' NOT FOUND in SocialBoard ---");
                continue;
            }

            Debug.Log($"--- AGENT ({agent.agentId}) ---");
            PrintWorldState(agent.worldState, agent.agentId);
        }

        Debug.Log("=========================");
    }

    private void PrintRegisteredAgents()
    {
        // No tenemos acceso directo a la lista interna del SocialBoard.
        // Esta función sirve como recordatorio visual:
        Debug.Log("[Snapshot] SocialBoard exists: " + (SocialBoard.Instance != null));
        Debug.Log("[Snapshot] Tip: Si ves warnings de GetAgent(null/empty), revisa TradePartnerId.");
    }

    private void PrintWorldState(WorldState state, string agentId)
    {
        if (state == null)
        {
            Debug.LogWarning($"[{agentId}] WorldState is null");
            return;
        }

        // Ordena para que sea legible
        List<KeyValuePair<string, object>> entries = new List<KeyValuePair<string, object>>();
        foreach (var kv in state)
            entries.Add(kv);

        entries.Sort((a, b) => string.CompareOrdinal(a.Key, b.Key));

        foreach (var kv in entries)
            Debug.Log($"[{agentId}] {kv.Key} = {kv.Value}");
    }
}
