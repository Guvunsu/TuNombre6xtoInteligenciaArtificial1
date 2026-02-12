using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sistema social global.
/// Permite:
/// - registrar agentes
/// - enviar mensajes
/// - ejecutar interacciones atómicas (trade)
/// </summary>
public class SocialBoard : MonoBehaviour
{
    public static SocialBoard Instance { get; private set; }

    // Agentes registrados por id
    private readonly Dictionary<string, GOADAgent> agents = new();

    // Buzón de mensajes por agente
    private readonly Dictionary<string, Queue<SocialMessage>> inbox = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void Register(GOADAgent agent)
    {
        agents[agent.agentId] = agent;
        if (!inbox.ContainsKey(agent.agentId))
            inbox[agent.agentId] = new Queue<SocialMessage>();
    }

    public GOADAgent GetAgent(string id)
    {
        agents.TryGetValue(id, out var a);
        return a;
    }

    /// <summary>
    /// Envía un mensaje a otro agente.
    /// </summary>
    public void Send(SocialMessage msg)
    {
        if (!inbox.ContainsKey(msg.toAgentId))
            inbox[msg.toAgentId] = new Queue<SocialMessage>();

        inbox[msg.toAgentId].Enqueue(msg);
    }

    /// <summary>
    /// Convierte mensajes entrantes en cambios del WorldState.
    /// </summary>
    public void PumpInboxIntoWorldState(GOADAgent receiver)
    {
        if (!inbox.TryGetValue(receiver.agentId, out var q))
            return;

        while (q.Count > 0)
        {
            var msg = q.Dequeue();

            if (msg.type == SocialMessageType.TradeRequest)
            {
                receiver.worldState["HasIncomingTradeRequest"] = true;
                receiver.worldState["TradePartnerId"] = msg.fromAgentId;
                receiver.worldState["TradePrice"] = msg.price;
                receiver.worldState["TradeFoodAmount"] = msg.foodAmount;
            }
            else if (msg.type == SocialMessageType.TradeAccepted)
            {
                receiver.worldState["TradeAcceptedByPartner"] = true;
                receiver.worldState["TradePartnerId"] = msg.fromAgentId;
            }
        }
    }

    /// <summary>
    /// Ejecuta un trade real entre dos agentes.
    /// </summary>
    public bool ExecuteTrade(string buyerId, string sellerId, int price, int foodAmount)
    {
        var buyer = GetAgent(buyerId);
        var seller = GetAgent(sellerId);
        if (buyer == null || seller == null) return false;

        int buyerMoney = buyer.worldState.GetInt("Money", 0);
        int sellerFood = seller.worldState.GetInt("Food", 0);

        if (buyerMoney < price) return false;
        if (sellerFood < foodAmount) return false;

        buyer.worldState["Money"] = buyerMoney - price;
        buyer.worldState["Food"] = buyer.worldState.GetInt("Food", 0) + foodAmount;

        seller.worldState["Money"] = seller.worldState.GetInt("Money", 0) + price;
        seller.worldState["Food"] = sellerFood - foodAmount;

        return true;
    }
}

public enum SocialMessageType
{
    TradeRequest,
    TradeAccepted
}

public struct SocialMessage
{
    public SocialMessageType type;
    public string fromAgentId;
    public string toAgentId;
    public int price;
    public int foodAmount;
}