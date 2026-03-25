using UnityEngine;

public enum DeviceType
{
    Camera,
    CardReader,
    PanicButton
}

public enum DeviceStatus
{
    Operational,
    Offline,
    Error
}

[DisallowMultipleComponent]
public class DeviceAuthoring : MonoBehaviour
{
    [Header("<size=14><color=orange>[ Device Info ]</color></size>")]
    [SerializeField, Tooltip("[DEVICETYPE FLOOR#. DEVICE#]")] private string _deviceId = "CAM04.00";
    [SerializeField] private string _displayName = "Hallway Camera";
    [SerializeField] private DeviceType _deviceType = DeviceType.Camera;
    [SerializeField] private string _floorId = "F04";
}
