using System.Collections.Generic;
using UnityEngine;

public class OperatorAttacking : MonoBehaviour
{
    [SerializeField] private CharacterBio charBio;
    [SerializeField] private LayerMask enemyMask;
    //[SerializeField] private Animator animator;
    [SerializeField] private float attackInterval = 1.0f; // Time between attacks
    [SerializeField] private bool antiAir;
    private float attackTimer;

    private List<EnemyAI> blockedEnemies = new List<EnemyAI>(); // List of blocked enemies

    private void Update()
    {
        // Update the timer
        attackTimer += Time.deltaTime;

        // Get all enemies within attack range
        List<Collider2D> targetsInRange = charBio.GetTargetsInRange();
        EnemyAI targetToAttack = null;

        if (targetsInRange.Count > 0)
        {
            // Find the nearest enemy in range
            float closestDistance = float.MaxValue;
            foreach (var target in targetsInRange)
            {
                EnemyAI enemy = target.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    float distance = Vector2.Distance(transform.position, enemy.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        targetToAttack = enemy;
                    }
                }
            }
        }

        // Attack the closest enemy if there is one in range and timer is ready
        if (targetToAttack != null && attackTimer >= attackInterval)
        {
            // Trigger the attack animation
            // animator.SetTrigger("Attack");

            // Reset the timer
            attackTimer = 0f;

            if (antiAir)
            {
                EnemyFlying flyingEnemy = targetToAttack.GetComponent<EnemyFlying>();
                flyingEnemy.TakeRangedDamage(charBio.ATK);
            }
            else
            {
                // Deal damage to the closest target
                targetToAttack.TakeDamage(charBio.ATK);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyAI enemy = other.GetComponent<EnemyAI>();
        if (enemy != null && !blockedEnemies.Contains(enemy) && charBio.BlockCount > 0)
        {
            // Block the enemy if there's block capacity
            BlockEnemy(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        EnemyAI enemy = other.GetComponent<EnemyAI>();
        if (enemy != null && blockedEnemies.Contains(enemy))
        {
            // Release the enemy when it leaves the blocking range
            ReleaseEnemy(enemy);
        }
    }

    private void BlockEnemy(EnemyAI enemy)
    {
        Debug.Log("Enemy Blocked");
        blockedEnemies.Add(enemy);
        charBio.BlockCount--;
        enemy.OnBlockedByOperator(this);

        if (charBio.BlockCount <= 0)
        {
            Debug.Log("Max Block Count reached!");
        }
    }

    private void ReleaseEnemy(EnemyAI enemy)
    {
        blockedEnemies.Remove(enemy);
        charBio.BlockCount++;
        enemy.OnUnblockedByOperator();
    }

    private void OnDisable()
    {
        // Release all blocked enemies when the operator is deactivated
        foreach (var enemy in blockedEnemies)
        {
            enemy.OnUnblockedByOperator();
        }
        blockedEnemies.Clear();
        charBio.BlockCount = charBio.MaxBlockCount;
    }
}
