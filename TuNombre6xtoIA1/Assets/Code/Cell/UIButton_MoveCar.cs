using UnityEngine;

public class UIButton_MoveCar : MonoBehaviour
{
    public InstantiateCarASTAR carSystem;

    public void MoveCar()
    {
        GameObject car = carSystem.GetCar();

        if (car == null) return;

        CarMovementASTAR movement = car.GetComponent<CarMovementASTAR>();

        if (movement != null)
        {
            movement.StartMovementFromUI();
        }
    }
}