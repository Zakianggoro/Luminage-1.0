using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    [Header("Image")]
    public Sprite skillImage;

    [Header("Text")]
    public string skillName;
    public string skillDescription;
    public string skillType;

    [Header("Restriction")]
    public float skillDuration;
    public float skillCooldown;
    public float energyRequired; // Energy required to activate the skill

/*    [Header("Skill Type")]
    public EnergyType energyType;

    public enum EnergyType { Automatic, OnAttack, OnHit }

    // Property to access energy type
    public EnergyType Type
    {
        get { return energyType; }
        set { energyType = value; }
    }*/

    private void OnValidate()
    {
        // Ensure energy values are not negative
        skillDuration = Mathf.Max(0, skillDuration);
        skillCooldown = Mathf.Max(0, skillCooldown);
        energyRequired = Mathf.Max(0, energyRequired);
    }
}
