using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Dijkstra : MonoBehaviour
{
    public Node[] nodes;
    public int start, end;
    void Start()
    {
        CreateNeighbors();
        CalculateDijkstra();
    }
    void CalculateDijkstra()
    {
        float[] distances = new float[nodes.Length];
        int[] prev = new int[nodes.Length];

        // Inicialización
        for (int i = 0; i < nodes.Length; i++)
        {
            distances[i] = Mathf.Infinity;
            prev[i] = -1;
            nodes[i].locked = false;
        }

        distances[start] = 0;

        while (true)
        {
            int bestNodeIndex = -1;
            float bestDistance = Mathf.Infinity;

            for (int i = 0; i < nodes.Length; i++)
            {
                if (!nodes[i].locked && distances[i] < bestDistance)
                {
                    bestDistance = distances[i];
                    bestNodeIndex = i;
                }
            }

            if (bestNodeIndex == -1) break;

            nodes[bestNodeIndex].Lock();

            if (bestNodeIndex == end) break;

            foreach (var entry in nodes[bestNodeIndex].neighbors)
            {
                int n = entry.Key.index;

                if (nodes[n].locked) continue;

                float alt = distances[bestNodeIndex] + entry.Value;

                if (alt < distances[n])
                {
                    distances[n] = alt;
                    prev[n] = bestNodeIndex;
                    nodes[n].parentIndex = bestNodeIndex;
                }
            }
        }

        // Reconstrucción del camino
        List<int> path = new List<int>();
        int currentNode = end;

        if (distances[end] == Mathf.Infinity)
        {
            Debug.Log("No se encontraron caminos");
            return;
        }

        while (currentNode != -1)
        {
            path.Add(currentNode);
            currentNode = prev[currentNode];
        }

        path.Reverse();

        Debug.Log("Camino más corto:");

        foreach (int i in path)
        {
            Debug.Log(nodes[i].name);
        }
    }
    void CreateNeighbors()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i].ClearNeighbors();

            for (int j = 0; j < nodes.Length; j++)
            {
                if (i == j) continue;

                float dist = Vector3.Distance(
                    nodes[i].transform.position,
                    nodes[j].transform.position
                );

                if (dist < 100f)
                {
                    nodes[i].AddNeighbor(nodes[j], dist);
                    Debug.Log("Distancia: " + dist);
                }
            }
        }
    }
}
