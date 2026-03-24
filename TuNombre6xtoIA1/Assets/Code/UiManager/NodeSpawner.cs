using UnityEngine;
using UnityEngine.InputSystem;
public class NodeSpawner : MonoBehaviour
{
    public GameObject Node;
    public Transform nodeContainer;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (!hit.transform.CompareTag("Grid")) return;

                Debug.DrawLine(ray.origin, hit.point);
                GameObject instance = Instantiate(Node);
                instance.transform.position = hit.point;
                instance.transform.SetParent(nodeContainer);
            }
        }
    }
}
