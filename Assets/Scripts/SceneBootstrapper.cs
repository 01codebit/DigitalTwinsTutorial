using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBootstrapper : MonoBehaviour
{
    [SerializeField]
    private string[] _additiveScene = { "Building_Exterior", "Floor_01"};

    private async Task Start()
    {
        // StartCoroutine(LoadSceneAsync());

        await LoadSceneAsync2();
    }

    private IEnumerator LoadSceneAsync()
    {
        foreach (var scene in _additiveScene)
        {
            if(!SceneManager.GetSceneByName(scene).isLoaded)
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                asyncLoad.allowSceneActivation = true;
                while (!asyncLoad.isDone)
                {
                    Debug.Log($"Loading scene {scene} progress: {asyncLoad.progress * 100f}%");
                    yield return null;
                }
            }
        }
    }

    private async Task LoadSceneAsync2()
    {
        foreach (var scene in _additiveScene)
        {
            if (!SceneManager.GetSceneByName(scene).isLoaded)
            {
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
                asyncLoad.allowSceneActivation = true;
                while (!asyncLoad.isDone)
                {
                    Debug.Log($"Loading scene '{scene}' progress: {asyncLoad.progress * 100f}%");
                    await Task.Yield();
                }

                Debug.Log($"Scene '{scene}' loaded successfully.");
            }
        }
    }
}
