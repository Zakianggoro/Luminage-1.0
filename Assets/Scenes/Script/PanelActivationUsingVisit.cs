using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelActivationUsingPreviousScene : MonoBehaviour
{
    [System.Serializable]
    public class PanelActivationEntry
    {
        public int previousSceneIndex;           // Previous scene's build index to trigger activation
        public List<GameObject> panelsToActivate; // Panels to activate based on the previous scene
    }

    [SerializeField] private List<PanelActivationEntry> panelActivations; // List of panel activation settings

    void Start()
    {
        int previousScene = SceneTracker.GetPreviousSceneIndex(); // Get the previous scene's build index
        Debug.Log("Previous Scene Index: " + previousScene);

        if (panelActivations == null || panelActivations.Count == 0)
        {
            Debug.LogWarning("No panel activation entries are assigned.");
            return;
        }

        // Loop through each panel activation entry to activate panels if the previous scene matches
        foreach (PanelActivationEntry entry in panelActivations)
        {
            if (entry.previousSceneIndex == previousScene)
            {
                foreach (GameObject panel in entry.panelsToActivate)
                {
                    if (panel != null)
                    {
                        panel.SetActive(true); // Activate the panel
                        Debug.Log("Activating Panel: " + panel.name + " for Previous Scene Index: " + previousScene);
                    }
                    else
                    {
                        Debug.LogWarning("Panel is null for Previous Scene Index: " + previousScene);
                    }
                }
            }
        }
    }
}