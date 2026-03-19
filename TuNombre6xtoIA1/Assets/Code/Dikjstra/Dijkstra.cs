using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Dijkstra : MonoBehaviour
{
    public Node[] nodes;
    public int start, end;
    void CalculateDijkstra()
    {
        //1. inicializar
        float[] distances = new float[nodes.Length];
        int[] prev = new int[nodes.Length];

        // limpiamos flags
        foreach (Node node in nodes)
        {
            node.locked = false;
            node.parentIndex = -1;

            // la distancia de A -> A es 0, Ya estamos ahi
            distances[start] = 0;

            //2 loop principal
            while (true)
            {
                //buscamos el nodo NO visitando con menor distancia global
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
                //si no hay candidato, no hay mas nodos alcanzables 
                if (bestNodeIndex == -1) break;
                //marcamos como visitado
                nodes[bestNodeIndex].Lock();
                //si ya llegamos al destino, paramos
                if (bestNodeIndex == end) break;
                //relajamos vecinos de bestNodeIndex
                foreach (var entry in nodes[bestNodeIndex].neighbors)
                {
                    int n = entry.Key.index;
                    if (nodes[n].locked) continue;

                    //costo acumulado
                    float alt = distances[bestNodeIndex] + entry.Value;
                    //si encontramos un camino mejor, actualizamos
                    if (alt < distances[n])
                    {
                        distances[n] = alt;
                        prev[n] = bestNodeIndex;
                        nodes[n].parentIndex = bestNodeIndex;
                    }
                }
            }
            // 3) reconstruimos ruta (usando prev[])
            List<int> path = new List<int>();
            int currentNode = end;

            // si Distance[end] es Infinity, no existre camino
            if (distances[end] == Mathf.Infinity)
            {
                Debug.Log("No se enontraron caminos");
            }
            while (currentNode != null)
            {
                path.Add(currentNode);
                currentNode = prev[currentNode];
            }
            path.Reverse();
        }
    }
}
