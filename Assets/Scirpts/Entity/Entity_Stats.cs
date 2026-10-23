using System;
using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    public Stat maxHealth;
    public Stat defaultDamage;
    public Stat_MajorGroup major;
    public Stat_OffensiveGroup offense;
    public Stat_DeffenseGroup deffense;

    public float GetMaxHealth()
    {
        float baseHP = maxHealth.getValue();
        float bonusHP = major.vitality.getValue() * 5;
        
        return baseHP + bonusHP;
    }

    public float GetStrength()
    {
        float baseDamage = defaultDamage.getValue();
        float bonusDamage = major.strength.getValue() * 2;
        
        return baseDamage + bonusDamage;
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
