using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHealth;
    public Stat defaultDamage;
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

    public float GetDamage(out bool isCrit)
    {
        float baseDamage = defaultDamage.getValue();
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
}
