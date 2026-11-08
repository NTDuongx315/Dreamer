using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Skill data - ", menuName = "RPG Setup/Skill Data")]

public class Skill_DataSO : ScriptableObject
{
    public int cost;
    public SkillUpgradeType upgradeType;
    public SkillType skillType;
    
    [Header("Skill Data")]
    public string name;
    [TextArea]
    public string description;
    public Sprite icon;
}
