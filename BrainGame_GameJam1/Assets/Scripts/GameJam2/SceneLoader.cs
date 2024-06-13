/*
 *  File: SceneLoader.cs
 *  Author: Devon
 *  Purpose: Load a specified scene passed in as a parameter during OnClick events
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private ActiveSceneManager activeSceneManager;

    private void Start()
    {
        GameObject sceneManagerObject = GameObject.FindWithTag("SceneManager");
        activeSceneManager = sceneManagerObject.GetComponent<ActiveSceneManager>();
    }

    //load the specified scene
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadMain()
    {
        activeSceneManager.ReturnToWorldMap();
    }
}