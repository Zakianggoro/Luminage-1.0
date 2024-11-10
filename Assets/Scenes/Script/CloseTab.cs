using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTab : MonoBehaviour
{
    // SerializeField makes this private variable visible in the Inspector
    [SerializeField] private GameObject panel;

    // This function closes the panel
    public void Close()
    {
        if (panel != null)
        {
            panel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Panel reference is missing!");
        }
    }
}
