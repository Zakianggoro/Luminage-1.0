using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTracker : MonoBehaviour
{
    private static int previousSceneIndex = -1; // Static variable to store the previous scene's build index
    private static int currentSceneIndex = -1;  // Static variable to store the current scene's build index
    private static bool hasInitialized = false; // Track if the tracker has already initialized

    void Awake()
    {
        // Ensure this is the only SceneTracker and it persists between scenes
        if (FindObjectsOfType<SceneTracker>().Length > 1)
        {
            Destroy(gameObject); // Destroy duplicate SceneTracker objects
        }
        else
        {
            DontDestroyOnLoad(gameObject); // Keep this object alive across scenes

            // Initialize only once
            if (!hasInitialized)
            {
                currentSceneIndex = SceneManager.GetActiveScene().buildIndex; // Get the starting scene index
                hasInitialized = true;
            }
        }
    }

    void OnEnable()
    {
        // Subscribe to the sceneLoaded event to track when a new scene is loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from the event when this object is disabled or destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        previousSceneIndex = currentSceneIndex; // Store the current scene as the previous scene before switching
        currentSceneIndex = scene.buildIndex;   // Update to the new scene's build index

        Debug.Log("Previous Scene Index: " + previousSceneIndex); // Log the previous scene index
        Debug.Log("Current Scene Index: " + currentSceneIndex);   // Log the current scene index
    }

    // Getter for the previous scene's build index
    public static int GetPreviousSceneIndex()
    {
        return previousSceneIndex;
    }

    // Getter for the current scene's build index
    public static int GetCurrentSceneIndex()
    {
        return currentSceneIndex;
    }
}