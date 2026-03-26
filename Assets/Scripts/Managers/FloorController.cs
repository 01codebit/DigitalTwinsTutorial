using DigitalTwins;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DigitalTwinsTutorial.Managers
{
    public class FloorController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _exteriorBuilding;

        [SerializeField]
        private DeviceUIController _deviceUIController;

        private CinemachineCamera _exteriorCamera;
        private CinemachineCamera _topDownCamera;

        public DeviceUIController DeviceUIController { get => _deviceUIController; set => _deviceUIController = value; }

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (_exteriorCamera == null)
            {
                _exteriorCamera = GameObject
                    .FindWithTag("ExteriorCam")
                    ?.GetComponent<CinemachineCamera>();
            }

            if (_topDownCamera == null)
            {
                _topDownCamera = GameObject
                    .FindWithTag("TopDownCam")
                    ?.GetComponent<CinemachineCamera>();
            }

            if (_exteriorBuilding == null)
            {
                foreach (var rootObject in scene.GetRootGameObjects())
                {
                    if (rootObject.CompareTag("ExteriorBuilding"))
                    {
                        _exteriorBuilding = rootObject;
                        Debug.Log(
                            $"[FloorController] Found exterior building in scene '{scene.name}'."
                        );
                        break;
                    }
                }
            }
        }

        public void ActivateFloor(string floorId)
        {
            if (_exteriorBuilding)
            {
                _exteriorBuilding.SetActive(false);
            }

            foreach (var floor in TwinRegistry.Instance.Floors.Values)
            {
                floor.gameObject.SetActive(floor.Id == floorId);
            }

            _topDownCamera.Priority = 20;
            _exteriorCamera.Priority = 10;

            _deviceUIController.SpawnMarkersForFloor(floorId);
        }

        public void ActivateExterior()
        {
            if (_exteriorBuilding)
            {
                _exteriorBuilding.SetActive(true);
            }

            foreach (var floor in TwinRegistry.Instance.Floors.Values)
            {
                floor.gameObject.SetActive(false);
            }

            _topDownCamera.Priority = 10;
            _exteriorCamera.Priority = 20;

            _deviceUIController.DespawnAll();
        }
    }
}
