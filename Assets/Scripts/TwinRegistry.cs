using System.Collections.Generic;
using System.Linq;
using Authoring;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DigitalTwins
{
    public class TwinRegistry : MonoBehaviour
    {
        public static TwinRegistry Instance { get; private set; }

        public IReadOnlyDictionary<string, FloorAuthoring> Floors => _floors;
        public IReadOnlyDictionary<string, DeviceAuthoring> Devices => _devices;

        private readonly Dictionary<string, FloorAuthoring> _floors = new();
        private readonly Dictionary<string, DeviceAuthoring> _devices = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Find all FloorAuthoring components in the scene and register them
            SceneManager.sceneLoaded += (_, _) => Refresh();
        }

        private void Refresh()
        {
            var foundFloors = new List<FloorAuthoring>();
            var foundDevices = new List<DeviceAuthoring>();
            for (var i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded)
                    continue;

                foreach (var rootObject in scene.GetRootGameObjects())
                {
                    foundFloors.AddRange(rootObject.GetComponentsInChildren<FloorAuthoring>(true));
                    foundDevices.AddRange(rootObject.GetComponentsInChildren<DeviceAuthoring>(true));
                }
            }

            _floors.Clear();
            _devices.Clear();

            foreach (var floor in foundFloors)
            {
                if (!_floors.ContainsKey(floor.Id))
                {
                    _floors.Add(floor.Id, floor);
                    Debug.Log($"[TwinRegistry] Registered floor: {floor.DisplayName} (ID: {floor.Id})");
                }
                else
                {
                    Debug.LogWarning(
                        $"[TwinRegistry] Duplicate floor ID detected: {floor.Id}. Skipping registration for {floor.DisplayName}."
                    );
                }
            }

            foreach (var device in foundDevices)
            {
                if (!_devices.ContainsKey(device.DeviceId))
                {
                    _devices.Add(device.DeviceId, device);
                    Debug.Log($"[TwinRegistry] Registered device: {device.DisplayName} (ID: {device.DeviceId})");
                }
                else
                {
                    Debug.LogWarning(
                        $"[TwinRegistry] Duplicate device ID detected: {device.DeviceId}. Skipping registration for {device.DisplayName}."
                    );
                }
            }
        }

        public IEnumerable<DeviceAuthoring> GetDevicesByFloor(string floorId) =>
            _devices.Values.Where(device => device.FloorId == floorId);
    }
}
