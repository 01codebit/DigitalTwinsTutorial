using System;
using Authoring;
using UnityEngine;
using UnityEngine.iOS;
using UnityEngine.UIElements;

public class MarkerBinder : MonoBehaviour
{
    [SerializeField]
    private bool _billboard = true;
    
    [SerializeField]
    private float _hoverScale = 1.25f;

    [SerializeField]
    private float _activeScale = 1.5f;

    [SerializeField]
    private float _scaleSpeed = 10f;

    private Camera _mainCamera;
    private UIDocument _uiDocument;
    private DeviceAuthoring _device;

    private VisualElement _iconBg;
    private VisualElement _iconFg;

    private bool _isHovered = false;
    private bool _isActive = false;
    private Vector3 _baseScale;

    private static MarkerBinder _activeMarker;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _uiDocument = GetComponent<UIDocument>();
        var root = _uiDocument.rootVisualElement;

        _iconBg = root.Q<VisualElement>("IconBackground");
        _iconFg = root.Q<VisualElement>("IconForeground");

        _baseScale = transform.localScale;

        root.RegisterCallback<PointerEnterEvent>(_ => SetHover(true));
        root.RegisterCallback<PointerLeaveEvent>(_ => SetHover(false));
        root.RegisterCallback<ClickEvent>(_ => OnClick());
    }

    public void Bind(DeviceAuthoring device)
    {
        _device = device;

        if (_iconBg != null && device.IconBackground)
        {
            _iconBg.style.backgroundImage = new StyleBackground(device.IconBackground);
        }

        if (_iconFg != null && device.Icon)
        {
            _iconFg.style.backgroundImage = new StyleBackground(device.Icon);
        }
    }

    private void SetHover(bool isHovered)
    {
        _isHovered = isHovered;
    }

    private void OnClick()
    {
        if (_activeMarker != null && _activeMarker != this)
        {
            _activeMarker.SetActive(false);
        }

        _activeMarker = this;
        SetActive(true);
    }

    private void SetActive(bool active)
    {
        _isActive = active;
    }

    private void ResetScaleState()
    {
        _isHovered = false;
        _isActive = false;
    }

    private void Update()
    {
        float targetScale = 1f;
        if (_isActive)
        {
            targetScale = _activeScale;
        }
        else if (_isHovered)
        {
            targetScale = _hoverScale;
        }

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            _baseScale * targetScale,
            Time.deltaTime * _scaleSpeed
        );
    }

    private void LateUpdate()
    {
        if (_billboard && _mainCamera != null)
        {
            transform.LookAt(
                transform.position + _mainCamera.transform.rotation * Vector3.forward,
                _mainCamera.transform.rotation * Vector3.up
            );
        }
    }
}
