using DigitalTwinsTutorial.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class TopBarUI : MonoBehaviour
{
    [SerializeField]
    private UIDocument _uiDocument;

    private Label _cityLabel;
    private VisualElement _cityDropdown;
    private VisualElement _cityPopup;
    private VisualElement _arrowIcon;

    private VisualElement _floorDropdown;
    private VisualElement _floorPopup;

    private FloorController _floorController;

    private void Awake()
    {
        if (!_uiDocument)
        {
            _uiDocument = GetComponent<UIDocument>();
        }

        var root = _uiDocument.rootVisualElement;

        _cityLabel = root.Q<Label>("CityLabel");
        _cityDropdown = root.Q<VisualElement>("CityDropdown");
        _cityPopup = root.Q<VisualElement>("CityPopup");
        _arrowIcon = root.Q<VisualElement>("Arrow");

        _cityDropdown?.RegisterCallback<ClickEvent>(evt => ToggleCityPopup());
        foreach (var cityButton in _cityPopup.Query<Button>().ToList())
        {
            cityButton.RegisterCallback<ClickEvent>(evt =>
            {
                SelectCity(cityButton.text);
                evt.StopPropagation();
            });
        }

        // Hide the city popup by default
        HideCityPopup();

        _floorDropdown = root.Q<VisualElement>("FloorDropdown");
        _floorPopup = root.Q<VisualElement>("FloorPopup");

        _floorDropdown?.RegisterCallback<ClickEvent>(evt => ToggleFloorPopup());
        foreach (var floorButton in _floorPopup.Query<Button>().ToList())
        {
            floorButton.RegisterCallback<ClickEvent>(evt =>
            {
                SelectFloor(floorButton.text);
                evt.StopPropagation();
            });
        }

        // Hide the floor popup by default
        HideFloorPopup();

        TryResolveControllers();
        SceneManager.sceneLoaded += (_, _) => TryResolveControllers();
    }

    private void TryResolveControllers()
    {
        if (!_floorController)
        {
            _floorController = FindFirstObjectByType<FloorController>(FindObjectsInactive.Exclude);
        }
    }

    private void ToggleCityPopup()
    {
        bool isHidden = _cityPopup.ClassListContains("hidden");
        if (isHidden)
        {
            ShowCityPopup();
        }
        else
        {
            HideCityPopup();
        }
    }

    private void ShowCityPopup()
    {
        _cityPopup.RemoveFromClassList("hidden");
        _arrowIcon.style.rotate = new Rotate(180);
    }

    private void HideCityPopup()
    {
        if (!_cityPopup.ClassListContains("hidden"))
        {
            _cityPopup.AddToClassList("hidden");
        }
        _arrowIcon.style.rotate = new Rotate(0);
    }

    private void SelectCity(string text)
    {
        _cityLabel.text = text;
        HideCityPopup();
    }

    private void ToggleFloorPopup()
    {
        bool isHidden = _floorPopup.ClassListContains("hidden");
        if (isHidden)
        {
            ShowFloorPopup();
        }
        else
        {
            HideFloorPopup();
        }
    }

    private void ShowFloorPopup()
    {
        _floorPopup.RemoveFromClassList("hidden");
    }

    private void HideFloorPopup()
    {
        _floorPopup.AddToClassList("hidden");
    }

    private void SelectFloor(string text)
    {
        HideFloorPopup();

        if (!_floorController)
        {
            Debug.LogWarning("[TopBarUI] No FloorController found. Cannot select floor.");
            return;
        }

        switch (text)
        {
            case "All":
                _floorController.ActivateExterior();
                break;
            case "Floor 1":
                _floorController.ActivateFloor("F01");
                break;
            case "Floor 2":
                _floorController.ActivateFloor("F02");
                break;
            case "Floor 3":
                _floorController.ActivateFloor("F03");
                break;
            case "Floor 4":
                _floorController.ActivateFloor("F04");
                break;
            default:
                Debug.LogWarning($"[TopBarUI] Unknown floor selection: {text}");
                break;
        }
    }
}
