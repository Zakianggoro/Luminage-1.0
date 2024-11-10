using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager main;
    public Transform[] path;

    public int currency;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        currency = 100;
    }

    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {

            currency -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }


    public Transform[] GetPathFromLane(Lane lane)
    {
        // Return the lane-specific waypoints if available, otherwise use the default path
        return lane.waypoints != null && lane.waypoints.Length > 0 ? lane.waypoints : path;
    }
}
