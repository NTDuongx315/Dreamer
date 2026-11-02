using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_TreeNode : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [SerializeField] private Skill_DataSO skillData;
    [SerializeField] private string skillName;
    [SerializeField] private Image skillIcon;
    [SerializeField] private string lockedColorHex = "#9F9797";
    private Color lastIconColor;
    public bool isUnlocked;
    public bool isLocked;
        
    //Onvalidate is a special Unity callback that gets called automatically
    //whenever a serialized value in a MonoBehaviour or ScriptableObject changes in the Inspector.
    //means, it function can runs and applied any change even before Play Mode
    private void OnValidate()
    {
        if (skillData == null) return;
        skillName = skillData.skillName;
        skillIcon.sprite = skillData.icon;
        gameObject.name = "UI_TreeNode_" + skillData.skillName;
    }

    private void Awake()
    {
        UpdateIconColor(GetColorByHex(lockedColorHex));
    }
    
    private void Unlock()
    {
        //find the player_skillmanager path then unlock skill on skill manager
        isUnlocked = true;
        UpdateIconColor(Color.white);
    }
    
    public bool CanBeUnlock()
    {
        if (isUnlocked || isLocked) return false;
        return true;
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
        else Debug.Log("cant be unlock");
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isUnlocked)
            UpdateIconColor(Color.white * 0.9f);
    }

    public void OnPointerExit(PointerEventData eventData)
    { 
        if(!isUnlocked)
            UpdateIconColor(lastIconColor);
    }

    private Color GetColorByHex(string hex)
    {
        ColorUtility.TryParseHtmlString(hex, out Color color);
        return color;
    }
}
