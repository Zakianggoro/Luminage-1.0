using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiAttackSkill : SkillBase
{
    // Reference to the SkillMultiplierHandler for stat changes
    [SerializeField] private SkillMultiplierHandler multiplierHandler;

    private int originalBlockCount;
    private int originalATK;

    // Accumulate energy over time in this class
    private void Update()
    {
        AccumulateEnergy(Time.deltaTime);  // Energy accumulates independently, once per frame
    }

    public override void ActivateSkill(CharacterBio operatorBio)
    {
        // Use the skillData from the base class (SkillBase)
        Debug.Log($"{skillData.skillName} is being activated!");

        // Store the original stats
        originalBlockCount = operatorBio.BlockCount;
        originalATK = operatorBio.ATK;

        // Apply the skill effects using multipliers from the SkillMultiplierHandler
        operatorBio.ATK = Mathf.RoundToInt(originalATK * multiplierHandler.AttackMultiplier);
        operatorBio.BlockCount = originalBlockCount; // Assuming operator attacks equal to their block count

        Debug.Log($"{operatorBio.OperatorName} now has increased attack: {operatorBio.ATK} and can attack multiple enemies equal to block count: {operatorBio.BlockCount}.");
    }

    public override void DeactivateSkill(CharacterBio operatorBio)
    {
        // Reset to original values
        operatorBio.ATK = originalATK;
        operatorBio.BlockCount = originalBlockCount;

        Debug.Log($"{skillData.skillName} has ended. {operatorBio.OperatorName}'s attack and block count have been reset.");
    }
}
