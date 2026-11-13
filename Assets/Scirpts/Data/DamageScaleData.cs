using System;
using UnityEngine;

[Serializable]
public class DamageScaleData
{
    [Header("Damage")] 
    public float physical = 1;
    public float element = 1;
    
    [Header("Chill Effect")]
    public float chillDuration = 3;
    public float chillSlowEffect = 0.2f;
    
    [Header("Burn Effect")]
    public float burnDuration = 3;
    public float burnDamageScale = 1;
    
    [Header("Lightning Effect")]
    public float lightningDuration = 3;
    public float lightningDamageScale = 1;
    public float lightningCharge = 0.4f;
}
