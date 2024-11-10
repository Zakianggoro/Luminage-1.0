using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class OpenPanel : MonoBehaviour
{
    // Reference to the panel GameObjects
    public GameObject Panel1;
    public GameObject Panel2;
    public GameObject Panel3;
    public GameObject Panel4;
    public GameObject Panel5;

    public Animator anim;
    private bool isMenuOpen = false;

    // Keep track of the currently active panel
    private GameObject currentActivePanel = null;

    // Duration of the scaling animation
    public float animationDuration = 0.3f;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Method to toggle Panel1's visibility with animation
    public void OpenPanel1()
    {
        TogglePanel(Panel1);
    }

    // Method to toggle Panel2's visibility with animation
    public void OpenPanel2()
    {
        //TogglePanel(Panel2);
        isMenuOpen = !isMenuOpen;
        anim.SetBool("Nyala", isMenuOpen);
    }

    // Method to toggle Panel3's visibility with animation
    public void OpenPanel3()
    {
        TogglePanel(Panel3);
    }

    // Method to toggle Panel4's visibility with animation
    public void OpenPanel4()
    {
        TogglePanel(Panel4);
    }

    // Method to toggle Panel5's visibility with animation
    public void OpenPanel5()
    {
        TogglePanel(Panel5);
    }

    // General method to toggle a panel with animation
    private void TogglePanel(GameObject panel)
    {
        if (panel == null)
            return;

        // If another panel is active, close it first
        if (currentActivePanel != null && currentActivePanel != panel)
        {
            StartCoroutine(ScalePanel(currentActivePanel, Vector3.one, Vector3.zero, animationDuration, false));
            currentActivePanel = null;
        }

        // Check if the panel is already active
        if (panel.activeSelf)
        {
            // Deactivate the panel
            StartCoroutine(ScalePanel(panel, Vector3.one, Vector3.zero, animationDuration, false));
        }
        else
        {
            // Activate the new panel
            panel.SetActive(true);
            StartCoroutine(ScalePanel(panel, Vector3.zero, Vector3.one, animationDuration, true));
            currentActivePanel = panel;
        }
    }

    // Coroutine to scale the panel
    private IEnumerator ScalePanel(GameObject panel, Vector3 startScale, Vector3 endScale, float duration, bool activateOnEnd)
    {
        RectTransform panelRectTransform = panel.GetComponent<RectTransform>();

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            panelRectTransform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        panelRectTransform.localScale = endScale;

        if (!activateOnEnd)
        {
            panel.SetActive(false);
        }
    }
}