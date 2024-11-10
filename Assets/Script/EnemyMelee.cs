using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    [SerializeField] private int attackDamage = 10; // Damage dealt per attack
    [SerializeField] private float attackInterval = 1.0f; // Time between attacks
    private float attackTimer;

    private OperatorHealth targetOperator; // Reference to the operator being attacked

    private void Update()
    {
        attackTimer += Time.deltaTime;
        // If there is an operator in range and the timer exceeds the attack interval, attack the operator
        if (targetOperator != null && attackTimer >= attackInterval)
        {
            Attack();
            attackTimer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object is an operator
        OperatorHealth operatorHealth = other.GetComponent<OperatorHealth>();
        if (operatorHealth != null)
        {
            targetOperator = operatorHealth;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // If the operator leaves melee range, stop attacking
        if (other.GetComponent<OperatorHealth>() == targetOperator)
        {
            targetOperator = null;
        }
    }

    private void Attack()
    {
        if (targetOperator != null)
        {
            Debug.Log($"{gameObject.name} is attacking {targetOperator.gameObject.name}");
            targetOperator.TakeDamage(attackDamage);
            Debug.Log("operator is damage equal to " + attackDamage);
        }
    }
}
