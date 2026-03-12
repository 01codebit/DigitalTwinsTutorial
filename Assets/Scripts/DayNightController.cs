using UnityEngine;

public class DayNightController : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private Light _directionalLight;

    [SerializeField] private Color _dayBackground;
    [SerializeField] private Color _nightBackground;
    
    [SerializeField] private float _dayLightIntensity;
    [SerializeField] private float _nightLightIntensity;

    private float _counter = 0f;
    private bool _switch = false;
    
    void Update()
    {
        _counter += Time.deltaTime;
        if (_counter > 5.0f)
        {
            _counter = 0;
            ToggleDayNight();
        }
    }

    private void ToggleDayNight()
    {
        if (_switch)
        {
            SetDay();
        }
        else
        {
            SetNight();
        }

        _switch = !_switch;
    }
    
    private void SetDay()
    {
        _mainCamera.backgroundColor = _dayBackground;
        _directionalLight.intensity = _dayLightIntensity;
    }

    private void SetNight()
    {
        _mainCamera.backgroundColor = _nightBackground;
        _directionalLight.intensity = _nightLightIntensity;
    }
}
