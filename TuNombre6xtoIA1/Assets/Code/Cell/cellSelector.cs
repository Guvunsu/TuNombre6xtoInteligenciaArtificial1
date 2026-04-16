using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class cellSelector : MonoBehaviour
{
    int stateToUse = 0;
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)|| Input.GetKeyDown(KeyCode.Keypad0))
        {
            stateToUse = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            stateToUse = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            stateToUse = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            stateToUse = 3;
        }
        if (Input.GetMouseButton(0))
        {
            SelectCell();
        }
    }
    public void SelectCell()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    Debug.Log("clicked en ui. ignoring 3d object click");
        //    return;
        //}
        if (Physics.Raycast(ray,out hit, 100)) { 
            Cell cell = hit.collider.transform.GetComponent<Cell>();
            if(cell!= null)
            {
                cell.SetState(stateToUse);
            }
        }
    }
}
