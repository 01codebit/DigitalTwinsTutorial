using DigitalTwinsTutorial.Managers;
using UnityEngine;

namespace DigitalTwinsTutorial
{
    public class TempInput : MonoBehaviour
    {
        [SerializeField]
        private FloorController _floorController;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                _floorController.ActivateExterior();
            }
        }
    }
}
