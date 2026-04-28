using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class AStar : MonoBehaviour
{
    public static Cell start, end;
    public Cell[] allCells;

    [Header("Colors")]
    public Color openColor = Color.cyan;
    public Color closeColor = Color.purple;
    public Color path = Color.orange;
    public Color currentColor = Color.white;

    private List<Cell> openSet = new List<Cell>();
    private List<Cell> closeSet = new List<Cell>();

    private bool searchStarted = false;
    private bool searchDone = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Step();
        }
    }
    Cell GetLowestF(List<Cell> openSet)
    {
        Cell best = openSet[0];
        foreach (Cell cell in openSet)
        {
            if (cell.F() < best.F() || (cell.F() == best.F() && cell.H() < best.H()))
                best = cell;
        }
        return best;
    }
    void Step()
    {
        //start.SetH(end.transform.position);
        if (searchDone)
        {
            Debug.Log("Search ended");
            return;
        }
        if (!searchStarted)
        {
            if (start == null || end == null)
            {
                Debug.Log("start or end cell not set");
                return;
            }
            start.SetG(0);
            start.SetH(end.transform.position);
            openSet.Add(start);
            searchStarted = true;

            return;
        }

        //no hay caminoi posible
        if (openSet.Count == 0)
        {
            searchDone = true;
            //ReconstructPath();
            Debug.Log("NoPathFound");
            return;
        }

        //tomamos el nodo con menor f 
        Cell current = GetLowestF(openSet);
        PaintCell(current, currentColor);

        //llegamos al final?
        if (current == end)
        {
            searchDone = true;
            Debug.Log("Path Found");
            return;
        }
        openSet.Remove(current);
        closeSet.Add(current);
        PaintCell(current, currentColor);

        //evaluar vecinos
        foreach (int neighborIndex in current.neighborIndex)
        {
            if (neighborIndex < 0 || neighborIndex >= allCells.Length) { continue; }
            Cell neighbor = allCells[neighborIndex];
            if (neighbor.enum_state == Cell.State.OBSTACLE)
            {
                continue;
            }
            if (closeSet.Contains(neighbor)) continue;

            int tentativeG = current.G() + 1;
            if (!openSet.Contains(neighbor))
            {
                neighbor.SetG(tentativeG);
                neighbor.SetH(end.transform.position);
                neighbor.parent = current;
                openSet.Add(neighbor);
                PaintCell(neighbor, openColor);
            } else if (tentativeG < neighbor.G())
            {
                neighbor.SetG(tentativeG);
                neighbor.parent = current;

            }
        }
    }

    void PaintCell(Cell cell, Color color)
    {
        if (cell == start || cell == end) return;
        cell.GetComponent<MeshRenderer>().material.color = color;
    }

    public List<Cell> GetPath()
    {
        List<Cell> path = new List<Cell>();

        if (end == null) return path;

        Cell current = end;

        while (current != null)
        {
            path.Add(current);
            current = current.parent;
        }

        path.Reverse();
        return path;
    }
}
