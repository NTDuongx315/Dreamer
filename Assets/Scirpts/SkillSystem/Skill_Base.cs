using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    [Header("General Details")] 
    [SerializeField] private float cooldown;
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        lastTimeUsed = lastTimeUsed - cooldown;
    }

    public bool CanUseSkill()
    {
        if (OnCoolDown()) return false;
        
        //mana check
        //unlock check
        
        return true;
    }
    
    public bool OnCoolDown() => Time.time < lastTimeUsed + cooldown; // return true when still on cooldown
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
    public void ResetCooldownBy(float cooldownAReduction) => lastTimeUsed = lastTimeUsed + cooldownAReduction;
    public void ResetCooldown() => lastTimeUsed = Time.time;
}
