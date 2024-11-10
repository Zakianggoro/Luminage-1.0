using System;
using UnityEngine;

[Serializable]
public class Tower {

    public string name;
    public int cost;
    public GameObject prefab;
    public bool onGround;
    public bool onRange;

    // Reference to the instantiated operator GameObject in the scene
    private GameObject placedOperatorInstance;


    public Tower(string _name, int _cost, GameObject _prefab, bool _onGround, bool _onRange)
    {
        name = _name;
        cost = _cost;
        prefab = _prefab;
        onGround = _onGround;
        onRange = _onRange;
    }

    // Method to set the reference to the placed operator instance
    public void SetPlacedOperator(GameObject instance)
    {
        placedOperatorInstance = instance;
    }

    // Method to get the reference to the placed operator instance
    public GameObject GetPlacedOperator()
    {
        return placedOperatorInstance;
    }

    // Method to clear the reference when the operator is recalled
    public void ClearPlacedOperator()
    {
        placedOperatorInstance = null;
    }

}
