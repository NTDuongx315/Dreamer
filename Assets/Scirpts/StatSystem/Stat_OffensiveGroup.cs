using System;
using UnityEngine;

[Serializable]
public class Stat_OffensiveGroup
{
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
