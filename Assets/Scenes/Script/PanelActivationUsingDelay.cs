using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelActivationUsingDelay : MonoBehaviour
{
    [System.Serializable]
    public class TimedPanelActivationEntry
    {
        public int previousSceneIndex;            // Previous scene's build index to trigger activation
        public List<GameObject> panelsToActivate; // Panels to activate with countdown
        public float activeDuration = 15.0f;      // Duration for which the panels stay active
    }

    [SerializeField] private List<TimedPanelActivationEntry> timedPanelActivations;

    void Start()
    {
        int previousScene = SceneTracker.GetPreviousSceneIndex(); // Get the previous scene's build index
        Debug.Log("Previous Scene Index: " + previousScene);

        if (timedPanelActivations == null || timedPanelActivations.Count == 0)
        {
            Debug.LogWarning("No timed panel activation entries are assigned.");
            return;
        }

        // Activate panels that deactivate after a time limit based on previous scene index
        foreach (TimedPanelActivationEntry entry in timedPanelActivations)
        {
            if (entry.previousSceneIndex == previousScene)
            {
                foreach (GameObject panel in entry.panelsToActivate)
                {
                    if (panel != null)
                    {
                        panel.SetActive(true); // Activate the panel
                        Debug.Log("Activating Panel with Countdown: " + panel.name + " for Previous Scene Index: " + previousScene);

                        // Start coroutine to deactivate the panel after its specific duration
                        StartCoroutine(DeactivatePanelAfterTime(panel, entry.activeDuration));
                    }
                    else
                    {
                        Debug.LogWarning("Panel is null for Previous Scene Index: " + previousScene);
                    }
                }
            }
        }
    }

    // Coroutine to deactivate the panel after the specified duration
    IEnumerator DeactivatePanelAfterTime(GameObject panel, float duration)
    {
        // Wait for the duration before deactivating the panel
        yield return new WaitForSeconds(duration);

        // Deactivate the panel
        if (panel != null)
        {
            panel.SetActive(false);
            Debug.Log("Deactivating Panel: " + panel.name);
        }
    }
}