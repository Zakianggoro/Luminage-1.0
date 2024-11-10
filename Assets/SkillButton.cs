using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    [Header("UI Components")]
    public Slider energySlider;  // Reference to the slider UI
    public TextMeshProUGUI energyText;  // Reference to the text showing energy
    public Button skillButton;  // Skill button that activates the skill

    // Reference to the character panel manager (or you can directly reference the PanelCharacter)
    public CharPanelManager charPanelManager;  // Drag the CharPanelManager in the inspector

    private SkillBase currentSkill;  // This should be of type SkillBase
    private CharacterBio selectedCharacter;

    private void Start()
    {
        skillButton.interactable = false;  // Initially, disable the button
    }

    // This method will be called when a character is selected
    public void SetCharacterSkill(CharacterBio selectedCharacter, int skillIndex)
    {
        this.selectedCharacter = selectedCharacter; // Assign the selected character

        if (skillIndex < selectedCharacter.OperatorSkills.Count)
        {
            // Initialize the skill from the selected character's skill list
            InitializeSkill(selectedCharacter.OperatorSkills[skillIndex]);
        }
        else
        {
            Debug.LogError("Skill index out of range.");
        }
    }

    // Initialize the skill button UI with a specific SkillBase
    public void InitializeSkill(SkillBase skill)
    {
        if (skill == null)
        {
            Debug.LogError("Skill is null.");
            return;
        }

        currentSkill = skill;

        // Set up the slider and energy text
        energySlider.maxValue = skill.skillData.energyRequired;
        energySlider.value = currentSkill.GetCurrentEnergyInt();  // Display the integer value of energy
        energyText.text = $"{currentSkill.GetCurrentEnergyInt()}/{skill.skillData.energyRequired}";

        Debug.Log("Initialized skill: " + skill.skillData.skillName);
    }

    private void Update()
    {
        if (currentSkill != null)
        {
            // Update the slider and energy text, showing energy as a whole number
            energySlider.value = currentSkill.GetCurrentEnergyInt();
            energyText.text = $"{currentSkill.GetCurrentEnergyInt()}/{currentSkill.skillData.energyRequired}";

            // Enable the button if the skill is ready and it's a manual activation
            skillButton.interactable = currentSkill.IsSkillReady() && currentSkill.isManualActivation;

            // Hide the slider when the skill is ready (energy is fully accumulated)
            if (currentSkill.IsSkillReady())
            {
                skillButton.interactable = true;
                energySlider.gameObject.SetActive(false);  // Hide the slider when energy is full
            }
            else
            {
                energySlider.gameObject.SetActive(true);   // Ensure the slider is visible if not ready
            }
        }
    }

    public void OnSkillButtonPressed()
    {
        if (currentSkill != null && currentSkill.IsSkillReady())
        {
            // Activate the skill using the selected character
            currentSkill.ActivateSkill(selectedCharacter); // Use selectedCharacter to activate the skill
            ResetUI();

            // Hide the character panel
            charPanelManager.HideCharacterPanel();  // Call to hide the panel
        }
    }

    private void ResetUI()
    {
        // Reset UI after skill activation
        energySlider.enabled = true;
        energySlider.value = 0;
        energyText.text = $"0/{currentSkill.skillData.energyRequired}";
        ResetValue();
        skillButton.interactable = false;
    }

    private void ResetValue()
    {
        currentSkill.ResetEnergy();
    }
}
