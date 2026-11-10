using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    private UI_SkillTree skillTree;
    private UI ui;
    private RectTransform rect;
    private UI_TreeConnectHandler connectHandler;

    [Header("Unlok Details")] 
    public UI_TreeNode[] neededNodes;
    public UI_TreeNode[] conflictNodes;
    public bool isUnlocked;
    public bool isLocked;
    
    public Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private string lockedColorHex = "#9F9797";
    private Color lastIconColor;
        
    //Onvalidate is a special Unity callback that gets called automatically
    //whenever a serialized value in a MonoBehaviour or ScriptableObject changes in the Inspector.
    //means, it function can runs and applied any change even before Play Mode
    private void OnValidate()
    {
        if (skillData == null) return;
        skillName = skillData.name;
        skillIcon.sprite = skillData.icon;
        gameObject.name = "UI_TreeNode_" + skillData.name;
    }

    private void Awake()
    {
        skillTree = GetComponentInParent<UI_SkillTree>();
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        connectHandler = GetComponent<UI_TreeConnectHandler>();
        
        UpdateIconColor(GetColorByHex(lockedColorHex));
    }
    public bool CanBeUnlock()
    {
        if (isUnlocked || isLocked) return false;
        if(!skillTree.EnoughPoint(skillData.cost))
        {
            Debug.Log("not enough skill point");
            return false;
        }

        foreach (var node in neededNodes) //search for parent node if it is unlocked or not
        {
            if (!node.isUnlocked) return false; //if the parent node is still lock so return false
        }

        foreach (var node in conflictNodes) //search for the other node with the same parent 
        {
            if(node.isUnlocked) return false; //if the other node is already unlocked so return false
        }
        
        return true;
    }

    public void Refund()
    {
        isUnlocked = false;
        isLocked = false;
        UpdateIconColor(GetColorByHex(lockedColorHex));
        
        skillTree.AddSkillPoint(skillData.cost);
        connectHandler.ConnectionImageUnlocked(false);
    }
    
    private void Unlock()
    {
        //find the player_skillmanager path then unlock skill on skill manager
        isUnlocked = true;
        UpdateIconColor(Color.white);
        skillTree.RemoveSkillPoint(skillData.cost);
        LockConflictNode();
        connectHandler.ConnectionImageUnlocked(true);
        
        skillTree.skillManager.GetSkillByType(skillData.skillType).SetSkillUpgrade(skillData.upgradeData);
    }

    private void LockConflictNode()
    {
        foreach (var node in conflictNodes) node.isLocked = true;
    }

    private void UpdateIconColor(Color color)
    {
        if (skillIcon == null) return;
        lastIconColor = skillIcon.color;
        skillIcon.color = color;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        if(CanBeUnlock()) Unlock();
        else if(isLocked) ui.skillToolTip.LockedSkillEffect();
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.skillToolTip.ShowTooltip(true, rect, this);

        if (isLocked || isUnlocked) return;
        TogleNodeHighlight(true);
    }

    private void TogleNodeHighlight(bool highlight)
    {
        Color highlightColor = Color.white * .9f; highlightColor.a = 1;
        Color colorToApply = highlight ? highlightColor : lastIconColor;
        
        UpdateIconColor(colorToApply);
    }

    public void OnPointerExit(PointerEventData eventData)
    { 
        ui.skillToolTip.ShowTooltip(false, null);

        if (isLocked || isUnlocked) return;
        TogleNodeHighlight(false);
    }

    private void OnDisable()
    {
        if(isLocked) UpdateIconColor(GetColorByHex(lockedColorHex));
        if(isUnlocked) UpdateIconColor(Color.white);
    }

    private Color GetColorByHex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }
}
