using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHealth;
    public Stat_MajorGroup major;
    public Stat_OffensiveGroup offense;
    public Stat_DeffenseGroup deffense;

    public float GetMaxHealth()
    {
        float baseMaxHealth = maxHealth.getValue();
        float bonusMaxHealth = major.vitality.getValue() * 5;
        
        float totalMaxHealth = baseMaxHealth + bonusMaxHealth;
        return totalMaxHealth;
    }
    
    public float GetEvasion()
    {
        float baseEvasion = deffense.evasion.getValue();
        float bonusEvasion = major.agility.getValue() * .5f;
        
        float totalEvasion = baseEvasion + bonusEvasion;
        float evasionCap = 75;
        
        float finalEvasion = Math.Clamp(evasionCap, 0, totalEvasion); //pass the value evasionCap,
        //the function Clamp() will return evasionCap if its value is in range 0->totalEvasion;
        //if it is greater than totalEvasion then return totalEvasion, if less than 0 then return 0.
        return finalEvasion;
    }
    
    public float GetDamage(out bool isCrit)
    {
        float baseDamage = offense.damage.getValue();
        float bonusDamage = major.strength.getValue();
        float totalBaseDamage = baseDamage + bonusDamage;

        float baseCritChance = offense.critRate.getValue();
        float bonusCritChance = major.agility.getValue() * .3f;
        float totalCritChance = baseCritChance + bonusCritChance;
        
        float baseCritDamage = offense.critDamage.getValue();
        float bonusCritDamage = major.strength.getValue() * .5f;
        float totalCritDamage = (baseCritDamage + bonusCritDamage) / 100;

        isCrit = Random.Range(0,100) < totalCritChance;
        float outputDamage = isCrit ? totalBaseDamage * totalCritDamage : totalBaseDamage;
        return outputDamage;
    }

    public float GetElementalDamage()
    {
        float fireDamage = offense.fireDamage.getValue();
        float iceDamage = offense.iceDamage.getValue();
        float lightningDamage = offense.lightningDamage.getValue();
        float bonusElementalDamage = major.intelligence.getValue();

        float highestDamage = fireDamage;
        if(iceDamage > highestDamage) highestDamage = iceDamage;
        if(lightningDamage > highestDamage) highestDamage = lightningDamage;

        if (highestDamage <= 0) return 0;
        
        // ý tưởng bonus elemental là lấy full power của elementPower hiện tại và cộng thêm với 50% power của những elementPower khác
        float bonusFire = (fireDamage == highestDamage) ? 0 : fireDamage * .5f;
        float bonusIce = (iceDamage == highestDamage) ? 0 : iceDamage * .5f;
        float bonusLightning = (lightningDamage == highestDamage) ? 0 : lightningDamage * .5f;
        float weakerElementDamage = bonusFire + bonusIce + bonusLightning;
        
        float finalDamage = highestDamage + weakerElementDamage + bonusElementalDamage;
        return finalDamage;
    }

    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = deffense.armor.getValue();
        float bonusArmor = major.vitality.getValue();
        float totalArmor = baseArmor + bonusArmor;
        
        float reductionMultiplier = Mathf.Clamp(1 - armorReduction, 0, 1);
        float effectiveArmor = totalArmor * reductionMultiplier;
        
        float mitigation = effectiveArmor / (effectiveArmor + 100);
        float mitigationCap = .85f;
        
        float finalArmor = Math.Clamp(mitigationCap, 0, mitigation);
        return finalArmor;
    }
    public float getArmorReduction()
    {
        float finalReduction = offense.armorReduction.getValue() / 100;
        return finalReduction;
    }
}
