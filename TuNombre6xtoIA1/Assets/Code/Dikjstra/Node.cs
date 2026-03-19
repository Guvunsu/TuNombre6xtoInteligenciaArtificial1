using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
public class Node : MonoBehaviour
{
    public int index;
    public string tagName;
    public int parentIndex;
    public bool locked = false;
    public float distance = Mathf.Infinity;

    public Dictionary<Node, float> neighbors = new Dictionary<Node, float>();
    public GameObject lineEdge = null;
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
    public void AddNeighbor(Node node, float cost)
    {
        neighbors.Add(node, cost);
    }
    void Start()
    {

    }

    void Update()
    {

    }
}
