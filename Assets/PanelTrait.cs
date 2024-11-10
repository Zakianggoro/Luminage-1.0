using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelTrait : MonoBehaviour
{
    [Header("Image")]
    [SerializeField] private Image traitIcon;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI traitName;
    [SerializeField] private TextMeshProUGUI traitDescription;

    public void SetTraitData(Trait trait)
    {
        traitIcon.sprite = trait.traitImage;
        traitName.text = trait.traitName;
        traitDescription.text = trait.traitDescription;
    }
}
