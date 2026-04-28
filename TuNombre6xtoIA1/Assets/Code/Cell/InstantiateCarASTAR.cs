using UnityEngine;
using UnityEngine.InputSystem;

public class InstantiateCarASTAR : MonoBehaviour
{
    public GameObject prefab_Car;
    public AStar script_AStar;

    private GameObject currentCar;
    private Cell currentCell;

    private bool placingMode = false;

    void Update()
    {
        if (placingMode && Input.GetMouseButtonDown(0))
        {
            PlaceCar();
        }
    }
    public void ActivatePlaceMode()
    {
        placingMode = true;
    }
    public void DeactivatePlaceMode()
    {
        placingMode = false;
    }


    void PlaceCar()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Cell cell = hit.collider.GetComponent<Cell>();

            if (cell != null)
            {
                if (currentCar != null)
                {
                    currentCar.transform.position = cell.transform.position + Vector3.up * 0.5f;
                } else
                {
                    currentCar = Instantiate(prefab_Car, cell.transform.position + Vector3.up * 0.5f, Quaternion.identity);
                }

                currentCell = cell;

                AStar.start = cell;
            }
        }
    }
    public GameObject GetCar()
    {
        return currentCar;
    }

    void SelectCar()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == currentCar)
            {
                ChangeColor(true);
            } else
            {
                ChangeColor(false);
            }
        }
    }

    void ChangeColor(bool selected)
    {
        if (currentCar == null) return;

        MeshRenderer mr = currentCar.GetComponent<MeshRenderer>();

        if (selected)
            mr.material.color = Color.purple;
        else
            mr.material.color = Color.cyan;
    }
}