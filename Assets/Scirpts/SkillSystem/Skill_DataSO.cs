using UnityEngine;

[CreateAssetMenu(fileName = "Skill data - ", menuName = "RPG Setup/Skill Data")]

public class Skill_DataSO : ScriptableObject
{
    public int cost;
    
    [Header("Skill Data")]
    public string skillName;
    [TextArea]
    public string description;
    public Sprite icon;
}
