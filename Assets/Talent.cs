using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Talent", menuName = "Talent")]
public class Talent : ScriptableObject
{ 
    [Header("Text")]
    public string talentName;
    public string talentDescription;
}
