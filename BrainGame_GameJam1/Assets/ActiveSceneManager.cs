using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActiveSceneManager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void LoadAndSetActiveScene(string sceneName)
    {
        StartCoroutine(LoadSceneAndSetActive(sceneName));
    }

    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        // Get the current active scene
        Scene currentActiveScene = SceneManager.GetActiveScene();

        // Load the new scene additively
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        // Get the newly loaded scene
        Scene loadedScene = SceneManager.GetSceneByName(sceneName);

        // Check if the scene was loaded successfully
        if (loadedScene.IsValid())
        {
            // Deactivate all root objects in the current active scene
            foreach (GameObject go in currentActiveScene.GetRootGameObjects())
            {
                go.SetActive(false);
            }

            // Set the newly loaded scene as the active scene
            SceneManager.SetActiveScene(loadedScene);
        }
        else
        {
            Debug.LogError("Failed to load scene: " + sceneName);
        }
    }

    public void ReturnToWorldMap()
    {
        StartCoroutine(UnloadAndReturnToWorldMap());
    }

    private IEnumerator UnloadAndReturnToWorldMap()
    {
        // Get the current active scene (assumed to be the side-scrolling scene)
        Scene currentActiveScene = SceneManager.GetActiveScene();

        // Get the world map scene (assuming its name is "WorldMap")
        Scene worldMapScene = SceneManager.GetSceneByName("WorldMap");

        // Check if the world map scene is loaded
        if (worldMapScene.IsValid())
        {
            // Activate all root objects in the world map scene
            foreach (GameObject go in worldMapScene.GetRootGameObjects())
            {
                go.SetActive(true);
            }

            // Set the world map scene as the active scene
            SceneManager.SetActiveScene(worldMapScene);

            // Unload the current active scene (side-scrolling scene)
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(currentActiveScene);
            while (!asyncOperation.isDone)
            {
                yield return null;
            }
        }
        else
        {
            Debug.LogError("WorldMap scene is not loaded.");
        }
    }
}