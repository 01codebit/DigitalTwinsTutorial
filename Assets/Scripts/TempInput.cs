using UnityEngine;

public class TempInput : MonoBehaviour
{
    [SerializeField] private FloorController _floorController;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            _floorController.ActivateExterior();
        }        
    }
}
