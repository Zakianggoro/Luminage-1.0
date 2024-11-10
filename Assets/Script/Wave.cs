using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lane
{
    public GameObject[] enemies;   // List of enemies to spawn in this lane
    public Transform[] spawnPoints; // Spawn points specific to this lane
    public float spawnInterval;    // Time interval between enemy spawns for this lane
    public Transform[] waypoints;  // Waypoints that define the enemy path
}


[System.Serializable]
public class Wave
{
    public Lane[] lanes;           // Array of lanes for this wave
    public float timeBetweenLanes; // Time between starting each lane in a wave
}

