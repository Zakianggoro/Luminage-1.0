using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSlide : MonoBehaviour
{
    [SerializeField] private RectTransform[] panels = new RectTransform[3]; // Array for three panels
    [SerializeField] private Vector2 startPos = new Vector2(1226, 0);       // Starting position
    [SerializeField] private Vector2 endPos = new Vector2(695, 0);          // Ending position
    [SerializeField] private float transitionDuration = 0.5f;               // Duration of the transition

    private bool[] isPanelOpen = new bool[3]; // Tracks each panel's open/closed state

    private void Start()
    {
        // Set all panels to start position at the beginning
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i] != null)
                panels[i].anchoredPosition = startPos;
            isPanelOpen[i] = false; // Initialize all panels as closed
        }
    }

    public void SlidePanel(int panelIndex)
    {
        if (panelIndex < 0 || panelIndex >= panels.Length) return; // Check if index is valid
        StopAllCoroutines(); // Stop any ongoing transitions

        Vector2 targetPos = isPanelOpen[panelIndex] ? startPos : endPos; // Toggle between start and end positions
        StartCoroutine(SlideToPosition(panels[panelIndex], targetPos, transitionDuration));

        isPanelOpen[panelIndex] = !isPanelOpen[panelIndex]; // Toggle panel state
    }

    private System.Collections.IEnumerator SlideToPosition(RectTransform panel, Vector2 targetPos, float duration)
    {
        Vector2 initialPos = panel.anchoredPosition;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            panel.anchoredPosition = Vector2.Lerp(initialPos, targetPos, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = targetPos; // Ensure the final position is set
    }
}