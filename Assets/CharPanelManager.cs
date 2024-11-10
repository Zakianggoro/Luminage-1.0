using UnityEngine;
using UnityEngine.EventSystems;

public class CharPanelManager : MonoBehaviour
{
    public PanelCharacter panelCharacter; // Reference to the PanelCharacter script

    private void OnEnable()
    {
        EventManager.OnCharacterSelected += ShowCharacterPanel;
        EventManager.OnBackgroundClick += HideCharacterPanel;  // Listen for background clicks
        EventManager.OnDeploymentCompleted += HideCharacterPanel;
    }

    private void OnDisable()
    {
        EventManager.OnCharacterSelected -= ShowCharacterPanel;
        EventManager.OnBackgroundClick -= HideCharacterPanel;  // Unsubscribe from background clicks
        EventManager.OnDeploymentCompleted -= HideCharacterPanel;
    }

    private void Update()
    {
        // Check if the left mouse button was clicked
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            HideCharacterPanel(); // Hide the panel if clicked outside
        }
    }

    // Method to show the character panel and set its data
    private void ShowCharacterPanel(CharacterBio bio)
    {
        panelCharacter.SetCharacterData(bio); // Set character data in PanelCharacter
    }

    // Method to hide the character panel
    public void HideCharacterPanel()
    {
        Debug.Log("Hiding character panel");
        panelCharacter.HidePanel(); // Hide the panel
    }


    private void ShowCharacterPanel()
    {
        panelCharacter.ShowPanel();
    }

    // Check if the pointer is over any UI elements
    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
