//using UnityEngine;
//using System.Collections.Generic;

//public class Node : MonoBehaviour
//{
//    public int index;
//    public string tagName;

//    public bool locked = false;
//    public int parentIndex;

//    public Dictionary<Node, float> neighbors = new Dictionary<Node, float>();

//    public void Initialize(int index, string tagName)
//    {
//        this.index = index;
//        this.tagName = tagName;
//    }

//    public void Lock()
//    {
//        locked = true;
//    }

//    public void AddNeighbor(Node node, float cost)
//    {
//        neighbors[node] = cost;
//    }

//    // 🔥 SELECCIÓN CON CLICK
//    void OnMouseDown()
//    {
//        Dijkstra manager = FindObjectOfType<Dijkstra>();

//        if (!manager.selectedNodes.Contains(this))
//        {
//            manager.selectedNodes.Add(this);
//            Debug.Log("Seleccionado: " + index);
//        }
//    }

//    // 🔥 DIBUJAR CONEXIONES
//    void OnDrawGizmos()
//    {
//        Gizmos.color = Color.green;

//        foreach (var neighbor in neighbors)
//        {
//            if (neighbor.Key != null)
//            {
//                Gizmos.DrawLine(transform.position, neighbor.Key.transform.position);
//            }
//        }
//    }
//}