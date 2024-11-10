using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneIndex : MonoBehaviour
{
    // Use [SerializeField] to expose private variables in the Inspector
    [SerializeField] private int Button1Index;

    [SerializeField] private int Button2Index;

    [SerializeField] private int Button3Index;

    [SerializeField] private int Button4Index;

    [SerializeField] private int Button5Index;

    [SerializeField] private int Button6Index;

    [SerializeField] private int Button7Index;


    // Public method to change the scene, can be called from a UI button
    public void Button1()
    {
        SceneManager.LoadScene(Button1Index);
    }

    public void Button2()
    {
        SceneManager.LoadScene(Button2Index);
    }

    public void Button3()
    {
        SceneManager.LoadScene(Button3Index);
    }

    public void Button4()
    {
        SceneManager.LoadScene(Button4Index);
    }

    public void Button5()
    {
        SceneManager.LoadScene(Button5Index);
    }

    public void Button6()
    {
        SceneManager.LoadScene(Button6Index);
    }

    public void Button7()
    {
        SceneManager.LoadScene(Button7Index);
    }
}