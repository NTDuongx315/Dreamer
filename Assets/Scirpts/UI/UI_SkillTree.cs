using UnityEngine;

public class UI_SkillTree : MonoBehaviour
{
    public int skillPoint;
    
    public bool EnoughPoint(int cost) => skillPoint >= cost;
    public void RemoveSkillPoint(int cost) => skillPoint -= cost;
}
