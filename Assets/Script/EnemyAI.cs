using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] EnemyMovement enemyMovement;
    [SerializeField] private int health;
    [SerializeField] private int currencyWorth;
    private OperatorAttacking blockingOperator;
    private bool isBlocked;
    private bool isDestroyed = false;


    void Update()
    {
        if (!isBlocked)
        {
            MoveForward();
        }
        else
        {
            if (isBlocked)
            {
                StopMoving();
            }
        }
    }

    void MoveForward()
    {
        enemyMovement.ResetSpeed();
    }

    void StopMoving()
    {
        enemyMovement.Blocked();
    }

    public void OnBlockedByOperator(OperatorAttacking op)
    {
        isBlocked = true;
        blockingOperator = op;
        Debug.Log("Enemy Blocked");
    }

    public void OnUnblockedByOperator()
    {
        isBlocked = false;
        blockingOperator = null;
    }

    public void TakeDamage(int damage)
    {
        // Handle taking damage
        Debug.Log("Enemy takes " + damage + " damage.");
        health -= damage;

        if (health <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
