using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMultiplierHandler : MonoBehaviour
{
    [Header("Damage Multiplier")]
    [SerializeField] private float attackMultiplier;
    [SerializeField] private float dmgPercent;

    [Header("Defense Multiplier")]
    [SerializeField] private float defPercent;

    [Header("Health Multiplier")]
    [SerializeField] private float healPercent;
    [SerializeField] private float healthBoost;

    [Header("Utility")]
    [SerializeField] private float knockBack;
    [SerializeField] private float pull;
    [SerializeField] private float aspdBoost;
    [SerializeField] private int blockPlus;

    [Header("Demerit")]
    [SerializeField] private float healthLoss;

    // Getter and Setter for Attack Multiplier
    public float AttackMultiplier
    {
        get { return attackMultiplier; }
        set { attackMultiplier = Mathf.Max(0, value); }  // Ensures a positive value
    }

    // Getter and Setter for Damage Percent
    public float DmgPercent
    {
        get { return dmgPercent; }
        set { dmgPercent = Mathf.Max(0, value); }  // Ensures a positive value
    }

    // Getter and Setter for Defense Percent
    public float DefPercent
    {
        get { return defPercent; }
        set { defPercent = Mathf.Max(0, value); }  // Ensures a positive value
    }

    // Getter and Setter for Heal Percent
    public float HealPercent
    {
        get { return healPercent; }
        set { healPercent = Mathf.Max(0, value); }  // Ensures a positive value
    }

    // Getter and Setter for Health Boost
    public float HealthBoost
    {
        get { return healthBoost; }
        set { healthBoost = Mathf.Max(0, value); }  // Ensures a positive value
    }

    // Getter and Setter for KnockBack
    public float KnockBack
    {
        get { return knockBack; }
        set { knockBack = Mathf.Max(0, value); }  // Ensures a positive value
    }

    // Getter and Setter for Pull
    public float Pull
    {
        get { return pull; }
        set { pull = Mathf.Max(0, value); }  // Ensures a positive value
    }

    // Getter and Setter for Attack Speed Boost
    public float AspdBoost
    {
        get { return aspdBoost; }
        set { aspdBoost = Mathf.Max(0, value); }  // Ensures a positive value
    }

    // Getter and Setter for Block Plus
    public int BlockPlus
    {
        get { return blockPlus; }
        set { blockPlus = Mathf.Max(0, value); }  // Ensures a positive value
    }

    // Getter and Setter for Health Loss (Demerit)
    public float HealthLoss
    {
        get { return healthLoss; }
        set { healthLoss = Mathf.Clamp(value, 0, 100); }  // Limits health loss between 0 and 100 (or adjust based on your needs)
    }
}
