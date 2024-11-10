using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trait", menuName = "Trait")]
public class Trait : ScriptableObject
{
    [Header("Image")]
    public Sprite traitImage;

    [Header("Text")]
    public string traitName;
    public string traitDescription;
}
