using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailButton : MonoBehaviour
{
    [Header("Buttons")]
    public Button skill;
    public Button trait;
    public Button talent;

    [Header("Panels")]
    public GameObject skillPanel;
    public GameObject traitPanel;
    public GameObject talentPanel;

    [Header("Button Active Color")]
    public Color activeColor = Color.white;  // Color when button is active

    private Color skillDefaultColor;  // Store the default color
    private Color traitDefaultColor;
    private Color talentDefaultColor;

    private void Start()
    {
        // Store the default button colors to preserve Unity editor-assigned values
        skillDefaultColor = skill.image.color;
        traitDefaultColor = trait.image.color;
        talentDefaultColor = talent.image.color;

        // Add listeners for the buttons
        skill.onClick.AddListener(() => ShowPanel("Skill"));
        trait.onClick.AddListener(() => ShowPanel("Trait"));
        talent.onClick.AddListener(() => ShowPanel("Talent"));

        // Initialize with only the skill panel active
        ShowPanel("Skill");
    }

    // Method to control which panel is displayed and update button states
    public void ShowPanel(string panelName)
    {
        // Deactivate all panels
        skillPanel.SetActive(false);
        traitPanel.SetActive(false);
        talentPanel.SetActive(false);

        // Reset button colors to their default (editor-assigned) color
        ResetButtonColors();

        // Activate the selected panel and highlight its corresponding button
        switch (panelName)
        {
            case "Skill":
                skillPanel.SetActive(true);
                skill.image.color = activeColor;  // Highlight skill button
                break;
            case "Trait":
                traitPanel.SetActive(true);
                trait.image.color = activeColor;  // Highlight trait button
                break;
            case "Talent":
                talentPanel.SetActive(true);
                talent.image.color = activeColor;  // Highlight talent button
                break;
        }
    }

    // Reset the button colors to their original default colors
    private void ResetButtonColors()
    {
        skill.image.color = skillDefaultColor;
        trait.image.color = traitDefaultColor;
        talent.image.color = talentDefaultColor;
    }
}
