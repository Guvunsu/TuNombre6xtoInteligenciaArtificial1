using UnityEngine;

public class AStar : MonoBehaviour
{
    public static Cell start, end;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            CalculateAStar();
        }
    }
    void CalculateAStar()
    {
        start.SetIdealDistance(end.transform.position);
    }
}
