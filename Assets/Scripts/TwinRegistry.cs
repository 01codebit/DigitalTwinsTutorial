using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TwinRegistry : MonoBehaviour
{
    public static TwinRegistry Instance { get; private set; }

    public IReadOnlyDictionary<string, FloorAuthoring> Floors => _floors;

    private Dictionary<string, FloorAuthoring> _floors = new();

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

        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (!scene.isLoaded) continue;

            foreach (var rootObject in scene.GetRootGameObjects())
            {
                foundFloors.AddRange(rootObject.GetComponentsInChildren<FloorAuthoring>(true));
            }
        }

        _floors.Clear();

        foreach (var floor in foundFloors)
        {
            if (!_floors.ContainsKey(floor.Id))
            {
                _floors.Add(floor.Id, floor);
                Debug.Log($"Registered floor: {floor.DisplayName} (ID: {floor.Id})");
            }
            else
            {
                Debug.LogWarning($"Duplicate floor ID detected: {floor.Id}. Skipping registration for {floor.DisplayName}.");
            }
        }
    }
}
