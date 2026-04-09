using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int index;
    public string tagName;
    public int parentIndex = -1;
    public bool locked = false;
    public float distance = Mathf.Infinity;

    public Dictionary<Node, float> neighbors = new Dictionary<Node, float>();
    public List<GameObject> edges = new List<GameObject>();

    public void Initialize(int index, string tagName)
    {
        this.index = index;
        this.tagName = tagName;
    }

    public void Lock()
    {
        locked = true;
    }

    public void ClearNeighbors()
    {
        neighbors.Clear();
    }

    public void AddNeighbor(Node node, float cost)
    {
        if (node == null || node == this) return;

        if (!neighbors.ContainsKey(node))
        {
            neighbors.Add(node, cost);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        foreach (var neighbor in neighbors)
        {
            if (neighbor.Key != null)
            {
                Gizmos.DrawLine(transform.position, neighbor.Key.transform.position);
            }
        }

        if (parentIndex != -1)
        {
            Node parentNode = FindObjectOfType<Dijkstra>().nodes[parentIndex];

            if (parentNode != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(transform.position, parentNode.transform.position);
            }
        }
    }
}