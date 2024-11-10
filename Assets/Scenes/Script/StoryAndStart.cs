using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryAndStart : MonoBehaviour
{
    [SerializeField] private int startSceneIndex;  // Use an integer for the Start scene index
    [SerializeField] private int storySceneIndex;  // Use an integer for the Story scene index

    public void StartGame()
    {
        SceneManager.LoadScene(startSceneIndex);
    }

    public void StoryGame()
    {
        SceneManager.LoadScene(storySceneIndex);
    }
}

