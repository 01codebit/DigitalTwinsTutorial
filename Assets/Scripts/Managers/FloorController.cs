using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class FloorController : MonoBehaviour
{
    [SerializeField]
    private GameObject _exteriorBuilding;

    [SerializeField]
    private UIDocument _dashboard;

    private VisualElement _dashboardRoot;
    private VisualElement _dashboardElement;

    private float _leftDelta = 0f;

    private CinemachineCamera _exteriorCamera;
    private CinemachineCamera _topDownCamera;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        _dashboardRoot = _dashboard.rootVisualElement;
        _dashboardElement = _dashboardRoot.Q<VisualElement>("Dashboard");
        _leftDelta = 620f;
        _dashboardElement.style.left = -1 * _leftDelta;

        Debug.Log($"[FloorController] Left delta set to {_leftDelta}");
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

        ActivateDashboard(floorId);
    }

    private void ActivateDashboard(string floorId)
    {
        _dashboardElement.style.left = 20f;
        _dashboardRoot.Q<Label>("Title").text = TwinRegistry.Instance.Floors[floorId].DisplayName;
    }

    public void ActivateExterior()
    {
        _dashboardElement.style.left = -1 * _leftDelta;

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
    }
}
