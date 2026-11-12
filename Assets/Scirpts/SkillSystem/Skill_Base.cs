using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    [Header("General Details")] 
    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType upgradeType;
    [SerializeField] protected float cooldown;
    private float lastTimeUsed;

    protected virtual void Awake()
    {
        lastTimeUsed = lastTimeUsed - cooldown;
    }

    public virtual void TryUseSkill()
    {
        
    }

    public void SetSkillUpgrade(UpgradeData upgrade)
    {
        upgradeType = upgrade.upgradeType; //assign upgrade to the skillUpgradeType
        cooldown = upgrade.cooldown;
    }

    public bool CanUseSkill()
    {
        if (upgradeType == SkillUpgradeType.None) return false;
        if (OnCoolDown()) return false;
        
        //mana check
        //unlock check
        
        return true;
    }

    protected bool Unlocked(SkillUpgradeType upgradeCheck) => upgradeType == upgradeCheck;
    
    public bool OnCoolDown() => Time.time < lastTimeUsed + cooldown; // return true when still on cooldown
    public void SetSkillOnCooldown() => lastTimeUsed = Time.time;
    
    // assigned a value to the cooldownReduction so the cooldown can be reduced faster for example, a skill got 4 seconds on cooldown
    // and you consider a function where each hit the enemy will reduce the cooldown time by 0.3 second so the 0.3
    // is what cooldownReduction.
    public void ResetCooldownBy(float cooldownReduction) => lastTimeUsed = lastTimeUsed + cooldownReduction;
    public void ResetCooldown() => lastTimeUsed = Time.time;
}
