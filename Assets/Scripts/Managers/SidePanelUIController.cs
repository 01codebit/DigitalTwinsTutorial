using Authoring;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;
using DeviceType = Authoring.DeviceType;

namespace DigitalTwinsTutorial.Managers
{
    public class SidePanelUIController : MonoBehaviour
    {
        public static SidePanelUIController Instance { get; private set; }

        [Header("<size=14><color=orange>[Settings]</color></size>")]
        [SerializeField]
        private string _uiDocumentTag = "SS_UI";

        private UIDocument _uiDocument;

        [Header("<size=14><color=orange>[Video Preview Settings]</color></size>")]
        [SerializeField]
        private RenderTexture _videoRT;

        private VideoPlayer _player;

        private Label _nameLbl,
            _typeLbl,
            _statusLbl,
            _idLbl,
            _locationLbl;
        private VisualElement _root;
        private VisualElement _videoPreview;
        private DeviceAuthoring _currentDevice;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log(
                $"[SidePanelUIController] Scene loaded: {scene.name}. Attempting to initialize UI."
            );
            if (scene.name == "UI")
                TryInitialize();
        }

        private void Start()
        {
            TryInitialize();
        }

        private void TryInitialize()
        {
            if (_uiDocument == null)
            {
                var go = GameObject.FindWithTag(_uiDocumentTag);

                if (go)
                    _uiDocument = go.GetComponent<UIDocument>();
            }

            if (_uiDocument == null)
            {
                Debug.LogError(
                    $"[SidePanelUIController] No UIDocument found with tag '{_uiDocumentTag}'."
                );
                return;
            }

            if (_root == null)
            {
                _root = _uiDocument.rootVisualElement;
                CacheUIElements();
            }
        }

        private void CacheUIElements()
        {
            _nameLbl = _root.Q<Label>("DeviceName");
            _typeLbl = _root.Q<Label>("DeviceType");
            _statusLbl = _root.Q<Label>("DeviceStatus");
            _idLbl = _root.Q<Label>("DeviceId");
            _locationLbl = _root.Q<Label>("DeviceLocation");
            _videoPreview = _root.Q<VisualElement>("VideoPreview");

            Debug.Log(
                $"[SidePanelUIController] Cached UI elements: "
                    + $"Name: {_nameLbl != null}, "
                    + $"Type: {_typeLbl != null}, "
                    + $"Status: {_statusLbl != null}, "
                    + $"ID: {_idLbl != null}, "
                    + $"Location: {_locationLbl != null}, "
                    + $"Video Preview: {_videoPreview != null}"
            );

            _root.Q<Button>("CloseBtn")?.RegisterCallback<ClickEvent>(_ => Hide());
            Hide();
        }

        public void Show(DeviceAuthoring device)
        {
            EnsureVideoPlayer();
            _currentDevice = device;

            if (_nameLbl != null)
                _nameLbl.text = $"Name: {device.DisplayName}";
            if (_typeLbl != null)
                _typeLbl.text = $"Type: {device.Type.ToString()}";
            if (_statusLbl != null)
                _statusLbl.text = $"Status: {device.Status.ToString()}";
            if (_idLbl != null)
                _idLbl.text = $"ID: {device.Id}";
            if (_locationLbl != null)
                _locationLbl.text = $"Location: {device.FloorId}";

            if ((device.Type == DeviceType.Camera) && !string.IsNullOrEmpty(device.StreamUrl))
            {
                _player.source = VideoSource.Url;
                _player.url = device.StreamUrl;
                _player.targetTexture = _videoRT;

                _videoPreview.style.backgroundImage = new Background { renderTexture = _videoRT };

                _player.prepareCompleted += (vp) =>
                {
                    Debug.Log($"[SidePanelUIController] VideoPlayer prepared video: {vp.url}.");
                    vp.Play();
                };
                _player.errorReceived += (vp, msg) =>
                {
                    Debug.Log($"[SidePanelUIController] VideoPlayer error: {msg}");
                };

                _player.Prepare();
            }
            else
            {
                StopVideo();
                _videoPreview.style.backgroundImage = null;
            }

            _root.RemoveFromClassList("hidden");
        }

        private void Hide()
        {
            _root?.AddToClassList("hidden");
            _currentDevice = null;
        }

        public bool IsShowingDevice(DeviceAuthoring device)
        {
            return _currentDevice == device && _root != null && !_root.ClassListContains("hidden");
        }

        private void StopVideo()
        {
            if (_player && _player.isPlaying)
                _player.Stop();
        }

        private void EnsureVideoPlayer()
        {
            if (_player == null)
            {
                _player = gameObject.AddComponent<VideoPlayer>();
                _player.playOnAwake = false;
                _player.isLooping = true;
                _player.renderMode = VideoRenderMode.RenderTexture;
                _player.audioOutputMode = VideoAudioOutputMode.None;

                _player.errorReceived += (source, message) =>
                {
                    Debug.LogWarning($"[SidePanelUIController] VideoPlayer error: {message}");
                };
            }
        }
    }
}
