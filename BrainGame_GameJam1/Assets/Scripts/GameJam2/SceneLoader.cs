/*
 *  File: SceneLoader.cs
 *  Author: Devon
 *  Purpose: Load a specified scene passed in as a parameter during OnClick events
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //load the specified scene
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}