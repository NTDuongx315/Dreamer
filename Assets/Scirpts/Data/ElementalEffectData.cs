using System;

public class ElementalEffectData
{
    public float chillDuration;
    public float chillSlowEffect;
    
    public float burnDuration;
    public float burnDamage;

    public float shockDuration;
    public float shockDamage;
    public float shockCharge;

    public ElementalEffectData(Entity_Stats stats, DamageScaleData damageScale)
    {
        chillDuration = damageScale.chillDuration;
        chillSlowEffect = damageScale.chillSlowEffect;

        burnDamage = stats.offense.fireDamage.getValue() * damageScale.burnDamageScale;
        burnDuration = damageScale.burnDuration;

        shockDuration = damageScale.lightningDuration;
        shockCharge = damageScale.lightningCharge;
        shockDamage = stats.offense.lightningDamage.getValue() * damageScale.lightningDamageScale;
    }
}
