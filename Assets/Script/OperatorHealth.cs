using System;
using UnityEngine;

public class OperatorHealth : MonoBehaviour
{
    [SerializeField] private CharacterBio charBio; // Reference to character bio containing max health and other stats
    private int currentHealth;
    public event Action<int> OnHealthChanged; // Event for health change
    private BuildManager buildManager;
    private Tower associatedTower; // Reference to the Tower scriptable object for this operator

    private void Start()
    {
        // Set operator's health to maximum at start
        currentHealth = charBio.MaxHealth;

        // Automatically get BuildManager instance
        buildManager = BuildManager.main;

        if (buildManager == null)
        {
            Debug.LogError("BuildManager instance is not found. Ensure BuildManager is in the scene.");
        }
    }

    // Call this to set the associated Tower object for this operator
    public void SetAssociatedTower(Tower tower)
    {
        associatedTower = tower;
    }

    public void TakeDamage(int damage)
    {
        charBio.CurrentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Current Health: {currentHealth}");

        // Invoke health changed event
        OnHealthChanged?.Invoke(currentHealth);

        // Check if health has dropped to zero or below
        if (charBio.CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");

        // Automatically recall the operator through BuildManager
        if (associatedTower != null && buildManager != null)
        {
            buildManager.RecallOperator(associatedTower);
        }

        // Destroy the GameObject to remove it from the scene
        Destroy(gameObject);
    }


    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public int GetMaxHealth()
    {
        return charBio.MaxHealth;
    }
}
