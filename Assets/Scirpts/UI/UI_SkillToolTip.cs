using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_Tooltip
{
    [SerializeField] private TextMeshProUGUI skillname;
    [SerializeField] private TextMeshProUGUI skilldescription;
    [SerializeField] private TextMeshProUGUI skillRequirement;

    public override void ShowTooltip(bool show, RectTransform targetRect)
    {
        base.ShowTooltip(show, targetRect);
    }

    public void ShowTooltip(bool show, RectTransform targetRect, Skill_DataSO skillData)
    {
        base.ShowTooltip(show, targetRect);

        if (!show) return;
        skillname.text = skillData.name;
        skilldescription.text = skillData.description;
        skillRequirement.text = "Requirements:\n" + "- " + skillData.cost + " skill points";
    }
}
