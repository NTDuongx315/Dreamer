using System;
using UnityEngine;

[Serializable]
public class Stat_OffensiveGroup
{
    public Stat attackSpeed;
    
    //Physical
    public Stat damage;
    public Stat critDamage;
    public Stat critRate;
    public Stat armorReduction;
    
    //Elemental Damage
    public Stat iceDamage;
    public Stat fireDamage;
    public Stat lightningDamage;
}   
