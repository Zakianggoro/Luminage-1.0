using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Wave[] waves;                // Array of waves
    public float timeBetweenWaves = 5f; // Time between each wave
    private int currentWaveIndex = 0;   // Tracks the current wave

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            yield return StartCoroutine(SpawnWave(waves[currentWaveIndex]));
            currentWaveIndex++;
            yield return new WaitForSeconds(timeBetweenWaves); // Delay between waves
        }

        // Handle end of level (e.g., victory screen) after all waves
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.lanes.Length; i++)
        {
            StartCoroutine(SpawnLane(wave.lanes[i]));  // Spawn each lane concurrently
            yield return new WaitForSeconds(wave.timeBetweenLanes); // Delay between lanes
        }
    }

    private IEnumerator SpawnLane(Lane lane)
    {
        for (int i = 0; i < lane.enemies.Length; i++)
        {
            Transform spawnPoint = lane.spawnPoints[Random.Range(0, lane.spawnPoints.Length)]; // Select a random spawn point
            GameObject enemyInstance = Instantiate(lane.enemies[i], spawnPoint.position, spawnPoint.rotation); // Spawn at the spawn point

            // Fetch the waypoints for this lane from the LevelManager
            Transform[] waypoints = LevelManager.main.GetPathFromLane(lane);

            // Set the path for the enemy (enemy will move towards the first waypoint naturally)
            EnemyMovement enemyMovement = enemyInstance.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.SetPath(waypoints);  // Path is set, but position remains the same
            }

            yield return new WaitForSeconds(lane.spawnInterval);  // Wait for the next spawn
        }
    }

}
