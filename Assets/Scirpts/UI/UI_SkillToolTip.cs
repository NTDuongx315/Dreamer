using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_Tooltip
{
    private UI ui;
    private UI_SkillTree skilltree;
        
    [SerializeField] private TextMeshProUGUI skillname;
    [SerializeField] private TextMeshProUGUI skilldescription;
    [SerializeField] private TextMeshProUGUI skillRequirement;

    [Space] 
    [SerializeField] private string metConditionHex;
    [SerializeField] private string notMetConditionHex;
    [SerializeField] private string importantnfoHex;
    [SerializeField] private Color testcolor;
    [SerializeField] private string lockedSkillText = "Skill Lock";
    
    private Coroutine textEffectCo;

    public void LockedSkillEffect()
    {
        if(textEffectCo != null) StopCoroutine(textEffectCo);
        textEffectCo = StartCoroutine(textBlinkEffectCo(skillRequirement, .15f, 3));
    }

    private IEnumerator textBlinkEffectCo(TextMeshProUGUI text, float blinkInterval, int blinkCount)
    {
        for (int i = 0; i < blinkCount; i++)
        {
            text.text = $"<color={notMetConditionHex}> Skill Lock: </color>";
            yield  return new WaitForSeconds(blinkInterval);
            
            text.text = $"<color={importantnfoHex}> Skill Lock: </color>";
            yield return new WaitForSeconds(blinkInterval);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        ui = GetComponentInParent<UI>();
        skilltree = ui.GetComponentInChildren<UI_SkillTree>(true);
    }

    public override void ShowTooltip(bool show, RectTransform targetRect)
    {
        base.ShowTooltip(show, targetRect);
    }

    public void ShowTooltip(bool show, RectTransform targetRect, UI_TreeNode node)
    {
        base.ShowTooltip(show, targetRect);

        if (!show) return;
        skillname.text = node.skillData.name;
        skilldescription.text = node.skillData.description;
        
        string skillLockedText = $"<color={importantnfoHex}> {lockedSkillText} </color>";
        string requirements = node.isLocked? skillLockedText: GetRequirements(node.skillData.cost, node.neededNodes, node.conflictNodes);
        
        skillRequirement.text = requirements;
    }

    private string GetRequirements(int skillCost, UI_TreeNode[] neededNode, UI_TreeNode[] conflictNode)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Requirements:");
        
        string costColor = skilltree.EnoughPoint(skillCost) ? metConditionHex : notMetConditionHex;
        sb.AppendLine($"<color={costColor}>- {skillCost} skill point(s)</color>");

        foreach (var node in neededNode)
        {
            if(node == null) continue;
            string nodeColor = node.isUnlocked ? metConditionHex : notMetConditionHex;
            sb.AppendLine($"<color={nodeColor}>- {node.skillData.name}</color>");
        }

        if (conflictNode.Length <= 0) return sb.ToString();

        sb.AppendLine();
        sb.AppendLine($"<color={importantnfoHex}> Skill Lock: </color>");
        foreach (var node in conflictNode)
        {
            if (node == null) continue;
            sb.AppendLine($"<color={importantnfoHex}>- {node.skillData.name}</color>");
        }
        
        return sb.ToString();
    }
}
