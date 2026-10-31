using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Entity_Stats : MonoBehaviour
{
    public Stat_ResourceGroup resources;
    public Stat_MajorGroup major;
    public Stat_OffensiveGroup offense;
    public Stat_DeffenseGroup deffense;

    public float GetMaxHealth()
    {
        float baseMaxHealth = resources.maxHealth.getValue();
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

    public float GetElementalDamage(out ElementalType  elementalType)
    {
        float fireDamage = offense.fireDamage.getValue();
        float iceDamage = offense.iceDamage.getValue();
        float lightningDamage = offense.lightningDamage.getValue();
        float bonusElementalDamage = major.intelligence.getValue();

        float highestDamage = fireDamage;
        elementalType = ElementalType.Fire;
        if(iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            elementalType = ElementalType.Ice;
        }
        if(lightningDamage > highestDamage)
        {
            highestDamage = lightningDamage;
            elementalType = ElementalType.Lightning;
        }

        if (highestDamage <= 0)
        {
            elementalType = ElementalType.None;
            return 0;
        }
        
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

    public float GetElementalResist(ElementalType element)
    {
        float baseResistance = 0;
        float bonusResistance = major.intelligence.getValue() * .5f;

        switch (element)
        {
            case ElementalType.Fire:
                baseResistance = deffense.fireResist.getValue();
                break;
            case ElementalType.Ice:
                baseResistance = deffense.iceResist.getValue();
                break;
            case ElementalType.Lightning:
                baseResistance=deffense.lightningResist.getValue();
                break;
        }
        
        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 75f;
        
        float finalResistance = Mathf.Clamp(resistance, 0, resistanceCap);
        return finalResistance;
    }

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return resources.maxHealth;
            case StatType.HealthRegen: return resources.healthRegen;

            case StatType.Strength: return major.strength;
            case StatType.Agility: return major.agility;
            case StatType.Intelligence: return major.intelligence;
            case StatType.Vitality: return major.vitality;

            case StatType.AttackSpeed: return offense.attackSpeed;
            case StatType.Damage: return offense.damage;
            case StatType.CritChance: return offense.critRate;
            case StatType.CritPower: return offense.critDamage;
            case StatType.ArmorReduction: return offense.armorReduction;

            case StatType.FireDamage: return offense.fireDamage;
            case StatType.IceDamage: return offense.iceDamage;
            case StatType.LightningDamage: return offense.lightningDamage;

            case StatType.Armor: return deffense.armor;
            case StatType.Evasion: return deffense.evasion;

            case StatType.IceResistance: return deffense.iceResist;
            case StatType.FireResistance: return deffense.fireResist;
            case StatType.LightningResistance: return deffense.lightningResist;

            default:
                Debug.LogWarning($"StatType {type} not implemented yet.");
                return null;
        }
    }
}
