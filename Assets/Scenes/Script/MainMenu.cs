using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // This will allow you to assign the scene name from the Unity Inspector.
    [SerializeField] private string sceneToChange;

    // This function will quit the game when the application is running as a build, and stop play mode in the editor.
    public void QuitGame()
    {
        // If running in Unity editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    // This function will change the scene to the serialized scene name.
    public void ChangeScene()
    {
        // Load the scene specified in the Inspector
        if (!string.IsNullOrEmpty(sceneToChange))
        {
            SceneManager.LoadScene(sceneToChange);
        }
        else
        {
            Debug.LogWarning("No scene name specified!");
        }
    }
}