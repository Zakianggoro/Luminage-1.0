using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intelPanel : MonoBehaviour
{
    // SerializeFields for the two panels to be activated/deactivated
    [SerializeField] private GameObject panel1;
    [SerializeField] private GameObject panel2;

    // Method to activate the first panel
    public void ActivatePanel1()
    {
        if (panel1 != null)
        {
            panel1.SetActive(true);  // Activate the first panel
        }
        else
        {
            Debug.LogWarning("Panel 1 is not assigned!");
        }
    }

    // Method to deactivate the first panel
    public void DeactivatePanel1()
    {
        if (panel1 != null)
        {
            panel1.SetActive(false);  // Deactivate the first panel
        }
        else
        {
            Debug.LogWarning("Panel 1 is not assigned!");
        }
    }

    // Method to activate the second panel
    public void ActivatePanel2()
    {
        if (panel2 != null)
        {
            panel2.SetActive(true);  // Activate the second panel
        }
        else
        {
            Debug.LogWarning("Panel 2 is not assigned!");
        }
    }

    // Method to deactivate the second panel
    public void DeactivatePanel2()
    {
        if (panel2 != null)
        {
            panel2.SetActive(false);  // Deactivate the second panel
        }
        else
        {
            Debug.LogWarning("Panel 2 is not assigned!");
        }
    }
}