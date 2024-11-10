using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextPanel : MonoBehaviour
{
    [SerializeField] private List<string> textList;              // List of texts to be displayed
    [SerializeField] private TextMeshProUGUI tmpText;            // Reference to the TextMeshPro component
    [SerializeField] private GameObject panelToDeactivate;       // The panel to deactivate when text list is finished
    [SerializeField] private Button clickableButton;             // Button that triggers the text change

    private int currentTextIndex = 0;                            // Index of the current text being displayed

    void Start()
    {
        if (tmpText == null)
        {
            Debug.LogError("TextMeshPro component is not assigned!");
            return;
        }

        if (panelToDeactivate == null)
        {
            Debug.LogError("Panel to deactivate is not assigned!");
            return;
        }

        if (clickableButton == null)
        {
            Debug.LogError("Button component is not assigned!");
            return;
        }

        if (textList.Count > 0)
        {
            DisplayText();
        }
        else
        {
            Debug.LogError("No text in the text list!");
        }

        // Add a listener to the button to handle click events
        clickableButton.onClick.AddListener(OnButtonClick);
    }

    // Function to display the current text in the TMP component
    void DisplayText()
    {
        tmpText.text = textList[currentTextIndex];
    }

    // Called when the button is clicked
    void OnButtonClick()
    {
        if (currentTextIndex < textList.Count - 1)
        {
            currentTextIndex++;
            DisplayText();
        }
        else
        {
            // If it's the last text, deactivate the panel
            panelToDeactivate.SetActive(false);
        }
    }
}
