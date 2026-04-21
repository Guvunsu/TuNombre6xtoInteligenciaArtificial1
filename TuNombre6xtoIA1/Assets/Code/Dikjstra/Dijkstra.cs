//using UnityEngine;
//using System.Collections.Generic;
//using UnityEngine.EventSystems;

//public class Dijkstra : MonoBehaviour
//{
//    [Header("Nodos")]
//    public List<Node> nodes = new List<Node>();

//    [Header("Selección dinámica")]
//    public List<Node> selectedNodes = new List<Node>();

//    [Header("Indices")]
//    public int startIndex = -1;
//    public int endIndex = -1;

//    float[] distances;
//    int[] prev;

//    bool isClickingUI = false;

//    [Header("Prefab")]
//    public GameObject nodePrefab;

//    // =============================
//    // 🔹 UPDATE
//    // =============================
//    void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            if (isClickingUI) return;
//            if (IsPointerOverUI()) return;

//            CreateNodeAtMouse();
//        }
//    }

//    // =============================
//    // 🔹 UI CONTROL
//    // =============================
//    public void OnUIButtonDown()
//    {
//        isClickingUI = true;
//    }

//    public void OnUIButtonUp()
//    {
//        isClickingUI = false;
//    }

//    bool IsPointerOverUI()
//    {
//        return EventSystem.current != null && EventSystem.current.IsPointerOverGameObject();
//    }

//    // =============================
//    // 🔹 CREAR NODOS
//    // =============================
//    void CreateNodeAtMouse()
//    {
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//        RaycastHit hit;

//        if (Physics.Raycast(ray, out hit))
//        {
//            GameObject obj = Instantiate(nodePrefab, hit.point, Quaternion.identity);
//            Node node = obj.GetComponent<Node>();

//            node.Initialize(nodes.Count, "Node_" + nodes.Count);
//            nodes.Add(node);

//            Debug.Log("Nodo creado en: " + hit.point);
//        }
//    }

//    // =============================
//    // 🔹 ELIMINAR NODO
//    // =============================
//    public void RemoveNode()
//    {
//        if (nodes.Count == 0) return;

//        Node last = nodes[nodes.Count - 1];
//        nodes.Remove(last);
//        Destroy(last.gameObject);

//        Debug.Log("Nodo eliminado");
//    }

//    // =============================
//    // 🔹 CONECTAR NODOS
//    // =============================
//    public void ConnectSelectedChain()
//    {
//        if (selectedNodes.Count < 2)
//        {
//            Debug.Log("Selecciona al menos 2 nodos");
//            return;
//        }

//        for (int i = 0; i < selectedNodes.Count - 1; i++)
//        {
//            Node a = selectedNodes[i];
//            Node b = selectedNodes[i + 1];

//            float cost = Vector3.Distance(a.transform.position, b.transform.position);

//            if (!a.neighbors.ContainsKey(b))
//                a.AddNeighbor(b, cost);

//            if (!b.neighbors.ContainsKey(a))
//                b.AddNeighbor(a, cost);

//            Debug.Log($"Conectado {a.index} ↔ {b.index}");
//        }

//        selectedNodes.Clear();
//    }

//    // =============================
//    // 🔹 SET START
//    // =============================
//    public void SetStartFromList()
//    {
//        if (selectedNodes.Count == 1)
//        {
//            ClearNodeColors();

//            Node node = selectedNodes[0];
//            startIndex = node.index;

//            node.GetComponent<Renderer>().material.color = Color.green;

//            Debug.Log("Start: " + startIndex);
//            selectedNodes.Clear();
//        }
//    }

//    // =============================
//    // 🔹 SET END
//    // =============================
//    public void SetEndFromList()
//    {
//        if (selectedNodes.Count == 1)
//        {
//            Node node = selectedNodes[0];
//            endIndex = node.index;

//            node.GetComponent<Renderer>().material.color = Color.red;

//            Debug.Log("End: " + endIndex);
//            selectedNodes.Clear();
//        }
//    }

//    void ClearNodeColors()
//    {
//        foreach (Node n in nodes)
//        {
//            n.GetComponent<Renderer>().material.color = Color.white;
//        }
//    }

//    // =============================
//    // 🔹 LIMPIAR SELECCIÓN
//    // =============================
//    public void ClearSelection()
//    {
//        selectedNodes.Clear();
//        Debug.Log("Selección limpiada");
//    }

//    // =============================
//    // 🔹 DIJKSTRA
//    // =============================
//    public void RunDijkstra()
//    {
//        if (startIndex == -1 || endIndex == -1)
//        {
//            Debug.LogError("Falta Start o End");
//            return;
//        }

//        int n = nodes.Count;
//        distances = new float[n];
//        prev = new int[n];

//        for (int i = 0; i < n; i++)
//        {
//            distances[i] = Mathf.Infinity;
//            prev[i] = -1;
//            nodes[i].locked = false;
//        }

//        distances[startIndex] = 0;

//        while (true)
//        {
//            int bestNode = -1;
//            float bestDist = Mathf.Infinity;

//            for (int i = 0; i < n; i++)
//            {
//                if (!nodes[i].locked && distances[i] < bestDist)
//                {
//                    bestDist = distances[i];
//                    bestNode = i;
//                }
//            }

//            if (bestNode == -1) break;

//            nodes[bestNode].Lock();

//            if (bestNode == endIndex) break;

//            foreach (var neighbor in nodes[bestNode].neighbors)
//            {
//                int neighborIndex = neighbor.Key.index;
//                float cost = neighbor.Value;

//                if (nodes[neighborIndex].locked) continue;

//                float alt = distances[bestNode] + cost;

//                if (alt < distances[neighborIndex])
//                {
//                    distances[neighborIndex] = alt;
//                    prev[neighborIndex] = bestNode;
//                }
//            }
//        }

//        PrintPath();
//    }

//    // =============================
//    // 🔹 RESULTADO
//    // =============================
//    void PrintPath() 
//    {
//        List<int> path = new List<int>();
//        int current = endIndex;

//        if (distances[endIndex] == Mathf.Infinity)
//        {
//            Debug.Log("No hay camino");
//            return;
//        }

//        while (current != -1)
//        {
//            path.Add(current);
//            current = prev[current];
//        }

//        path.Reverse();

//        string result = "Camino: ";
//        foreach (int i in path)
//        {
//            result += i + " -> ";
//        }

//        Debug.Log(result + "FIN");
//        Debug.Log("Costo total: " + distances[endIndex]);
//    }
//}