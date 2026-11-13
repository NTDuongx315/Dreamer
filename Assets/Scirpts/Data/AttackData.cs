using System;
using UnityEngine;

[Serializable]
public class AttackData
{
    public float physicalDamage;
    public float elementalDamage;
    public bool isCrit;
    public ElementalType element;

    public ElementalEffectData effectData;

    public AttackData(Entity_Stats stats, DamageScaleData damageScale)
    {
        physicalDamage = stats.GetDamage(out isCrit, damageScale.physical);
        elementalDamage = stats.GetElementalDamage(out element, damageScale.element);
        
        effectData = new ElementalEffectData(stats, damageScale);
    }
}
