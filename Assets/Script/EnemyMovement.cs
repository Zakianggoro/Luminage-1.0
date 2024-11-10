using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 3f;

    private Transform[] waypoints;  // Array of waypoints to follow
    private int waypointIndex = 0;  // Index to track the current waypoint
    private float baseSpeed;        // Store the base speed for resetting speed changes

    private void Start()
    {
        baseSpeed = moveSpeed;
    }

    private void Update()
    {
        // Check if enemy has reached the current waypoint
        if (waypoints != null && Vector2.Distance(waypoints[waypointIndex].position, transform.position) <= 0.1f)
        {
            waypointIndex++;  // Move to the next waypoint

            // If the enemy reaches the final waypoint, destroy it and invoke the event
            if (waypointIndex >= waypoints.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
        }
    }

    public void SetPath(Transform[] pathWaypoints)
    {
        if (pathWaypoints != null && pathWaypoints.Length > 0)
        {
            waypoints = pathWaypoints;
            // Don't move the enemy's position here, let it start where it's instantiated
        }
        else
        {
            Debug.LogError("Waypoints not set or empty.");
        }
    }

    public void UpdateSpeed(float newSpeed)
    {
        moveSpeed = newSpeed;
    }

    public void ResetSpeed()
    {
        moveSpeed = baseSpeed;
    }

    public void Blocked()
    {
        moveSpeed = 0;
    }

    private void FixedUpdate()
    {
        MoveTowardsWaypoint();
    }

    private void MoveTowardsWaypoint()
    {
        if (waypoints != null && waypointIndex < waypoints.Length)
        {
            // Move towards the current waypoint
            Vector2 direction = (waypoints[waypointIndex].position - transform.position);
            float distance = direction.magnitude;
            Vector2 velocity = direction * (moveSpeed / distance);
            rb.velocity = velocity;
        }
    }

    public void StopMoving()
    {
        moveSpeed = 0;
        Debug.Log("Enemy Stopped Moving");
    }
}
