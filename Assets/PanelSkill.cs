using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelSkill : MonoBehaviour
{
    [Header("Image")]
    [SerializeField] private Image skillIcon;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillType;
    [SerializeField] private TextMeshProUGUI skillDescription;

    public void SetSkillData(Skill skill)
    {
        skillIcon.sprite = skill.skillImage;
        skillName.text = skill.skillName;
        skillType.text = skill.skillType;
        skillDescription.text = skill.skillDescription;
    }
}
