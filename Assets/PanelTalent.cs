using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelTalent : MonoBehaviour
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI talentName;
    [SerializeField] private TextMeshProUGUI talentDescription;

    public void SetTalentData(Talent talent)
    {
        talentName.text = talent.talentName;
        talentDescription.text = talent.talentDescription;
    }
}
