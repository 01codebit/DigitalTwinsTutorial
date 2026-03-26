using System.Collections.Generic;
using UnityEngine;

namespace DigitalTwins
{
    public class DeviceUIController : MonoBehaviour
    {
        [SerializeField] private GameObject _markerPrefab;
        readonly List<GameObject> _activeMarkers = new();

        public void SpawnMarkersForFloor(string floorId)
        {
            DespawnAll();

            foreach (var device in TwinRegistry.Instance.GetDevicesByFloor(floorId))
            {
                var marker = Instantiate(
                    _markerPrefab,
                    device.Anchor.position,
                    Quaternion.identity
                );
                marker.transform.SetParent(device.Anchor, true);
                marker.GetComponent<MarkerBinder>().Bind(device);
                _activeMarkers.Add(marker);
            }
        }

        public void DespawnAll()
        {
            foreach (var marker in _activeMarkers)
            {
                if (marker)
                    Destroy(marker);
            }
            _activeMarkers.Clear();
        }
    }
}
