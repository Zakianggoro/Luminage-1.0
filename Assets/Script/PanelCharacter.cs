using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PanelCharacter : MonoBehaviour
{
    [Header("Character Stats")]
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI level;
    public TextMeshProUGUI atk;
    public TextMeshProUGUI def;
    public TextMeshProUGUI blockCount;
    public TextMeshProUGUI currentHealthText;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI operatorRange;

    [Header("Character Images")]
    public Image operatorImage;
    public Image operatorClassImage;
    public Image operatorRangeImage;

    [Header("Health Bar")]
    public Slider healthBar;

    [Header("Unique")]
    public PanelSkill panelSkill;
    public PanelTrait panelTrait;
    public PanelTalent panelTalent;
    public SkillButton skillButton;

    private float defaultTimeScale = 1.0f;  // Store the default timescale value
    private float slowTimeScale = 0.3f;     // Define a slower timescale for dragging

    private CharacterBio charBio; // Add a reference to store character data
    private DeployHandler deployHandler;

    private void Awake()
    {
        // Ensure the panel is off by default at the start
        gameObject.SetActive(false);
        deployHandler = FindObjectOfType<DeployHandler>();
    }

    private void Update()
    {
        if (gameObject.activeSelf) // Only update if the panel is active
        {
            UpdateHealthBar();
        }
    }

    private void UpdateHealthBar()
    {
        if (charBio != null)
        {
            healthBar.value = charBio.CurrentHealth;
            currentHealthText.text = charBio.CurrentHealth.ToString();
        }
    }

    public void SetCharacterData(CharacterBio bio)
    {
        // Store the character data reference
        charBio = bio;

        // Activate the panel when a character is selected
        Debug.Log("Character data set for: " + bio.OperatorName);
        gameObject.SetActive(true);  // Ensure this is called to turn the panel on

        Time.timeScale = slowTimeScale;

        characterName.text = bio.OperatorName;
        level.text = bio.Level.ToString();
        atk.text = bio.ATK.ToString();
        def.text = bio.DEF.ToString();
        blockCount.text = bio.BlockCount.ToString();

        // Update health texts and health bar
        currentHealthText.text = bio.CurrentHealth.ToString();
        maxHealthText.text = bio.MaxHealth.ToString() + "/";

        healthBar.maxValue = bio.MaxHealth;
        healthBar.value = bio.CurrentHealth;

        // Use ImageLoader to load images
        ImageLoader.Instance.LoadImage("Operators/Image" + bio.OperatorName, operatorImage);
        ImageLoader.Instance.LoadImage("Operators/Class" + bio.OperatorClass, operatorClassImage);
        ImageLoader.Instance.LoadImage("Operators/Range" + bio.OperatorRange, operatorRangeImage);

        if (bio.OperatorSkill != null)
        {
            panelSkill.SetSkillData(bio.OperatorSkill);  // Pass skill data to PanelSkill
            skillButton.SetCharacterSkill(bio, 0);
        }
        if (bio.OperatorTrait != null)
        {
            panelTrait.SetTraitData(bio.OperatorTrait);
        }
        if (bio.OperatorTalent != null)
        {
            panelTalent.SetTalentData(bio.OperatorTalent);
        }
    }

    // Call this to hide the panel when clicking outside
    public void HidePanel()
    {
        gameObject.SetActive(false);
        if(deployHandler != null)
        {
            deployHandler.ClearHighlightedTiles();
        }
        Time.timeScale = defaultTimeScale;
    }

    public void ShowPanel()
    {
        gameObject.SetActive(true);
    }
}
