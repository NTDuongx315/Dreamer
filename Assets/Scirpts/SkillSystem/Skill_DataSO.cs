using System;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Skill data - ", menuName = "RPG Setup/Skill Data")]

public class Skill_DataSO : ScriptableObject
{
    [Header("Skill Data")]
    public string name;
    [TextArea]
    public string description;
    public Sprite icon;
    
    [Header("Unlock & Upgrade")]
    public int cost;
    public bool unlockbyDefault;
    public SkillType skillType;
    public UpgradeData upgradeData;
}

[Serializable]
//each upgrade skill will get different cooldown so make up a new class that manage that.
public class UpgradeData 
{
    public SkillUpgradeType upgradeType;
    public float cooldown;
    public DamageScaleData damageScale;
}
