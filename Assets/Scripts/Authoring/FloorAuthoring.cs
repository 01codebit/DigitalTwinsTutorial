using UnityEngine;

public class FloorAuthoring : MonoBehaviour
{
    [SerializeField] private string _floorId = "F01";
    [SerializeField] private string _displayName = "Floor 01";

    public string Id => _floorId;
    public string DisplayName => _displayName;
}
