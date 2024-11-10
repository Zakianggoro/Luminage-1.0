using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : MonoBehaviour
{
    [Header("Skill Data")]
    public Skill skillData; // Reference to ScriptableObject

    public float currentEnergy; // Energy accumulated over time
    public bool isManualActivation;

    [Header("Skill Type")]
    public EnergyType energyType;

    public enum EnergyType { Automatic, OnAttack, OnHit }

    // Property to access energy type
    public EnergyType Type;

    // Activation method using the character's bio
    public abstract void ActivateSkill(CharacterBio operatorBio);

    public virtual void DeactivateSkill(CharacterBio operatorBio)
    {
        // Optional: Add logic to deactivate the skill when duration ends
    }

    public virtual void AccumulateEnergy(float amount)
    {
        // Accumulate energy using a float, but clamp it within the required energy range
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0, skillData.energyRequired);
    }

    public bool IsSkillReady()
    {
        // Check if the integer part of the current energy is enough to activate the skill
        return Mathf.FloorToInt(currentEnergy) >= skillData.energyRequired;
    }

    public void ResetEnergy()
    {
        currentEnergy = 0;
    }

    public int GetCurrentEnergyInt()
    {
        // Return the current energy as an integer
        return Mathf.FloorToInt(currentEnergy);
    }
}
