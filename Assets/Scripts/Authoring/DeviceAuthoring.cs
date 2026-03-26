using System;
using UnityEngine;

namespace Authoring
{
    public enum DeviceType
    {
        Camera,
        CardReader,
        PanicButton,
    }

    public enum DeviceStatus
    {
        Operational,
        Offline,
        Error,
    }

    [DisallowMultipleComponent]
    public class DeviceAuthoring : MonoBehaviour
    {
        [Header("<size=14><color=orange>[Device Info]</color></size>")]
        [SerializeField, Tooltip("[DEVICETYPE FLOOR#. DEVICE#]")]
        private string _deviceId = "CAM04.00";

        [SerializeField]
        private string _displayName = "Hallway Camera";

        [SerializeField]
        private DeviceType _deviceType = DeviceType.Camera;

        [SerializeField]
        private string _floorId = "F04";

        [Header("<size=14><color=orange>[Device State]</color></size>")]
        [SerializeField]
        private DeviceStatus _deviceStatus = DeviceStatus.Operational;

        [Header("<size=14><color=orange>[Assets]</color></size>")]
        [SerializeField]
        private Sprite _cameraIcon;

        [SerializeField]
        private Sprite _cardReaderIcon;

        [SerializeField]
        private Sprite _panicButtonIcon;

        [SerializeField]
        private Sprite _operationalBg;

        [SerializeField]
        private Sprite _offlineBg;

        [SerializeField]
        private Sprite _errorBg;

        [Header("<size=14><color=orange>[Optional Custom Override]</color></size>")]
        [SerializeField, Tooltip("Override the default icon based on device type")]
        private Sprite _customIcon;

        [SerializeField, Tooltip("Override the default background based on device status")]
        private Sprite _customBg;

        [SerializeField]
        private Transform _markerAnchor;

        private Sprite _currentIcon;
        private Sprite _currentBg;
        private DeviceStatus _lastStatus;

        public string DeviceId => _deviceId;
        public string DisplayName => _displayName;
        public DeviceType DeviceType => _deviceType;
        public string FloorId => _floorId;
        public DeviceStatus DeviceStatus => _deviceStatus;
        public Transform Anchor => _markerAnchor ? _markerAnchor : transform;
        public Sprite Icon => _currentIcon;
        public Sprite IconBackground => _currentBg;

        private void Awake()
        {
            RefreshVisualState();
            _lastStatus = _deviceStatus;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying && _deviceStatus != _lastStatus)
            {
                SetStatus(_deviceStatus);
            }

            if (!Application.isPlaying)
            {
                RefreshVisualState();
            }

            _lastStatus = _deviceStatus;
        }
#endif

        public void RefreshVisualState()
        {
            _currentIcon = _deviceType switch
            {
                DeviceType.Camera => _customIcon ? _customIcon : _cameraIcon,
                DeviceType.CardReader => _customIcon ? _customIcon : _cardReaderIcon,
                DeviceType.PanicButton => _customIcon ? _customIcon : _panicButtonIcon,
                _ => null,
            };

            _currentBg = _deviceStatus switch
            {
                DeviceStatus.Operational => _customBg ? _customBg : _operationalBg,
                DeviceStatus.Offline => _offlineBg,
                DeviceStatus.Error => _errorBg,
                _ => null,
            };
        }

        public void SetStatus(DeviceStatus newStatus)
        {
            if (_deviceStatus != newStatus)
            {
                _deviceStatus = newStatus;
            }

            RefreshVisualState();

            var binder = GetComponentInChildren<MarkerBinder>();
            if (binder)
            {
                binder.Bind(this);
            }

            _lastStatus = _deviceStatus;
        }
    }
}
