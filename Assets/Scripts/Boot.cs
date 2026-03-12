using UnityEngine;

public class Boot : MonoBehaviour
{
    private void Awake() => DontDestroyOnLoad(gameObject);
}
