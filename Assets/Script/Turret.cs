using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private Transform firingPoint;

    [Header("Atribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 1f; //Bullets Per Second
    
    private Transform target;
    private float timeUntilFire;

    // Update is called once per frame
    void Update()
    {
        if (target == null) {
            FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1/bps) {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    private void FindTarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, 
            (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0 )
        {
            target = hits[0].transform;
        }
    }

    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, 
            target.position.x - transform.position.x) * Mathf.Rad2Deg - 90;
        
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }

    private void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefabs, firingPoint.position, Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>(); 
        bulletScript.SetTarget(target);
    }


    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.blue;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingRange);

    }
}