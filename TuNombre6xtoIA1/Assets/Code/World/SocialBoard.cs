using System.Collections.Generic;
using UnityEngine;

public class SocialBoard : MonoBehaviour
{
    public static SocialBoard Instance { get; private set; }

    private readonly Dictionary<string, GOADAgent> agents = new();
    private readonly Dictionary<string, Queue<SocialMessage>> inbox = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Debug.Log("[SocialBoard] Awake");
    }

    public void Register(GOADAgent agent)
    {
        if (agent == null) return;

        agents[agent.agentId] = agent;

        if (!inbox.ContainsKey(agent.agentId))
            inbox[agent.agentId] = new Queue<SocialMessage>();

        Debug.Log($"[SocialBoard] Registered agentId={agent.agentId} (total={agents.Count})");
    }

    public GOADAgent GetAgent(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogWarning("[SocialBoard] GetAgent called with null/empty id");
            return null;
        }

        agents.TryGetValue(id, out var a);
        Debug.Log($"[SocialBoard] GetAgent({id}) -> {(a != null)}");
        return a;
    }

    public void Send(SocialMessage msg)
    {
        Debug.Log($"[SocialBoard] Send {msg.type} from={msg.fromAgentId} to={msg.toAgentId} price={msg.price} food={msg.foodAmount}");

        if (string.IsNullOrEmpty(msg.toAgentId))
        {
            Debug.LogWarning("[SocialBoard] Message has no receiver (toAgentId empty).");
            return;
        }

        if (!inbox.ContainsKey(msg.toAgentId))
        {
            Debug.LogWarning($"[SocialBoard] Inbox not found for {msg.toAgentId}. Is that agent registered?");
            return;
        }

        inbox[msg.toAgentId].Enqueue(msg);
    }

    /// <summary>
    /// Convierte mensajes -> flags/datos en el WorldState del receptor.
    /// Esto es el puente Event -> State para que GOAP pueda reaccionar.
    /// </summary>
    public void PumpInboxIntoWorldState(GOADAgent receiver)
    {
        if (receiver == null) return;

        if (!inbox.TryGetValue(receiver.agentId, out var q))
        {
            Debug.LogWarning($"[SocialBoard] No inbox for receiver={receiver.agentId}");
            return;
        }

        while (q.Count > 0)
        {
            var msg = q.Dequeue();
            Debug.Log($"[{receiver.agentId}] Received message {msg.type} from={msg.fromAgentId}");

            if (msg.type == SocialMessageType.TradeRequest)
            {
                receiver.worldState["HasIncomingTradeRequest"] = true;
                receiver.worldState["TradePartnerId"] = msg.fromAgentId;
                receiver.worldState["TradePrice"] = msg.price;
                receiver.worldState["TradeFoodAmount"] = msg.foodAmount;
            } else if (msg.type == SocialMessageType.TradeAccepted)
            {
                receiver.worldState["TradeAcceptedByPartner"] = true;
                receiver.worldState["TradePartnerId"] = msg.fromAgentId;
            }
        }
    }

    /// <summary>
    /// Ejecuta el trade de forma at�mica:
    /// buyer paga Money, seller entrega Food.
    /// </summary>
    public bool ExecuteTrade(string buyerId, string sellerId, int price, int foodAmount)
    {
        Debug.Log($"[SocialBoard] ExecuteTrade buyer={buyerId} seller={sellerId} price={price} foodAmount={foodAmount}");

        var buyer = GetAgent(buyerId);
        var seller = GetAgent(sellerId);

        if (buyer == null || seller == null)
        {
            Debug.LogWarning("[SocialBoard] ExecuteTrade failed: buyer or seller not found.");
            return false;
        }

        int buyerMoney = buyer.worldState.GetInt("Money", 0);
        int sellerFood = seller.worldState.GetInt("Food", 0);

        Debug.Log($"[SocialBoard] buyerMoney={buyerMoney}, sellerFood={sellerFood}");

        if (buyerMoney < price)
        {
            Debug.LogWarning("[SocialBoard] ExecuteTrade failed: buyer has not enough money.");
            return false;
        }

        if (sellerFood < foodAmount)
        {
            Debug.LogWarning("[SocialBoard] ExecuteTrade failed: seller has not enough food.");
            return false;
        }

        // buyer: -Money, +Food
        buyer.worldState["Money"] = buyerMoney - price;
        buyer.worldState["Food"] = buyer.worldState.GetInt("Food", 0) + foodAmount;

        // seller: +Money, -Food
        seller.worldState["Money"] = seller.worldState.GetInt("Money", 0) + price;
        seller.worldState["Food"] = sellerFood - foodAmount;

        Debug.Log($"[SocialBoard] Trade OK. buyer Money={buyer.worldState.GetInt("Money")} Food={buyer.worldState.GetInt("Food")} | seller Money={seller.worldState.GetInt("Money")} Food={seller.worldState.GetInt("Food")}");

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
