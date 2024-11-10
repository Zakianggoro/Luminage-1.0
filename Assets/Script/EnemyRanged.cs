using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : MonoBehaviour
{
    [Header("Attack Parameters")]
    public float attackRange = 5f;             // Range at which the enemy can attack
    public float attackCooldown = 1.5f;        // Time between each attack
    public int attackDamage = 10;              // Damage dealt per attack
    public LayerMask operatorLayer;            // Layer to detect operators on

    private GameObject targetOperator;         // The operator in range to attack
    private float attackTimer = 0f;            // Timer to control attack cooldown
    private bool isAttacking = false;          // Flag to control movement during attack

    private EnemyMovement enemyMovement;       // Reference to the enemy's movement script

    private void Start()
    {
        // Initialize the movement script
        enemyMovement = GetComponent<EnemyMovement>();
    }

    private void Update()
    {
        if (!isAttacking)
        {
            DetectOperator(); // Continuously check for operators within range
        }

        if (targetOperator != null)
        {
            // If attack cooldown has expired, initiate the attack
            if (attackTimer <= 0f)
            {
                StartAttack();
            }
            else
            {
                attackTimer -= Time.deltaTime;  // Count down the attack cooldown
            }
        }
    }

    private void DetectOperator()
    {
        // Detect operators within attack range
        Collider2D[] operatorsInRange = Physics2D.OverlapCircleAll(transform.position, attackRange, operatorLayer);

        if (operatorsInRange.Length > 0)
        {
            // Select the closest operator as the target
            targetOperator = operatorsInRange[0].gameObject;
            float closestDistance = Vector2.Distance(transform.position, targetOperator.transform.position);

            foreach (Collider2D col in operatorsInRange)
            {
                float distance = Vector2.Distance(transform.position, col.transform.position);
                if (distance < closestDistance)
                {
                    targetOperator = col.gameObject;
                    closestDistance = distance;
                }
            }
        }
        else
        {
            targetOperator = null; // No operators in range
        }
    }

    private void StartAttack()
    {
        isAttacking = true;

        // Stop movement during attack
        if (enemyMovement != null)
        {
            enemyMovement.StopMoving();
        }

        // Trigger the attack animation here, e.g., animator.SetTrigger("Attack");
        DealDamage();
        attackTimer = attackCooldown;   // Reset the attack timer
    }

    // This method will be called by an animation event at the right time during the attack animation
    public void DealDamage()
    {
        if (targetOperator != null)
        {
            OperatorHealth operatorHealth = targetOperator.GetComponent<OperatorHealth>();
            if (operatorHealth != null)
            {
                operatorHealth.TakeDamage(attackDamage); // Apply damage to the operator
            }
        }
    }

    // This method will be called by an animation event at the end of the attack animation
    public void EndAttack()
    {
        isAttacking = false;

        // Resume movement after the attack
        if (enemyMovement != null)
        {
            enemyMovement.ResetSpeed();
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the attack range for debugging
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
