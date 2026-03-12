using UnityEngine;
using UnityEngine.UIElements;

public class BuildingLevelSelector : MonoBehaviour
{
    [SerializeField] private UIDocument _doc;
    private VisualElement _root;
    private FloorController _floorController;

    private Button _lvl1Btn,
        _lvl2Btn,
        _lvl3Btn,
        _lvl4Btn;

    private void Awake()
    {
        if (!_doc)
            _doc = GetComponent<UIDocument>();

        _root = _doc.rootVisualElement;

        _lvl1Btn = _root.Q<Button>("Level_01_Btn");
        _lvl2Btn = _root.Q<Button>("Level_02_Btn");
        _lvl3Btn = _root.Q<Button>("Level_03_Btn");
        _lvl4Btn = _root.Q<Button>("Level_04_Btn");

        _lvl1Btn.clicked += () => OnLevelSelected("F01");
        _lvl2Btn.clicked += () => OnLevelSelected("F02");
        _lvl3Btn.clicked += () => OnLevelSelected("F03");
        _lvl4Btn.clicked += () => OnLevelSelected("F04");

        if (!_floorController)
            _floorController = FindFirstObjectByType<FloorController>(FindObjectsInactive.Exclude);

    }

    private void OnLevelSelected(string floorId)
    {
        Debug.Log($"Level Selected: {floorId}");
        _floorController.ActivateFloor(floorId);
    }
}
