using UnityEngine;
using System.Collections.Generic;

public class CarMovementASTAR : MonoBehaviour
{
    public AStar aStar;          // referencia al AStar del prefab
    public float speed = 2f;

    private List<Cell> path;
    private int currentIndex = 0;
    private bool moving = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartMovement();
        }

        if (moving)
        {
            MoveStep();
        }
    }

    public void StartMovementFromUI()
    {
        StartMovement();
    }

    void StartMovement()
    {
        if (AStar.start == null || AStar.end == null)
        {
            Debug.Log("selecciona inicio y fin antes de mover.");
            return;
        }
        path = aStar.GetPath();

        if (path == null || path.Count == 0)
        {
            Debug.Log("No hay camino calculado. Presiona Spacebar para ejecutar A*.");
            return;
        }

        currentIndex = 0;
        moving = true;
    }

    void MoveStep()
    {
        if (currentIndex >= path.Count)
        {
            moving = false;
            return;
        }

        Vector3 target = path[currentIndex].transform.position + Vector3.up * 0.5f;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            currentIndex++;
        }
    }
}
