using System;
using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityvfx;
    private Entity_Stats stats;
    private Entity_Health entityHealth;
    private ElementalType currentEffect = ElementalType.None;

    [Header("Electricity Effect")] 
    [SerializeField] private GameObject lightningStrikeVfx;
    [SerializeField] private float curCharge;
    [SerializeField] private float maxCharge = 1;
    private Coroutine electricityEffectCo;

    private void Awake()
    {
        entityHealth = GetComponent<Entity_Health>();
        stats = GetComponent<Entity_Stats>();
        entity = GetComponent<Entity>();
        entityvfx = GetComponent<Entity_VFX>();
    }

    public void AppliedLightningEffect(float duration, float lightningDamage, float charge)
    {
        float lightningResistance = stats.GetElementalResist(ElementalType.Lightning);
        float reduceCharge = charge * (1 - lightningResistance);
        curCharge += reduceCharge;

        if (curCharge >= maxCharge)
        {
            ReadyToStrike(lightningDamage);
            StopElectricityEffect();
            return;
        }
        
        if(electricityEffectCo != null) StopCoroutine(electricityEffectCo);
        electricityEffectCo = StartCoroutine(ElectricityEffectCo(duration));
    }

    private void StopElectricityEffect()
    {
        currentEffect = ElementalType.Lightning;
        curCharge = 0;
        entityvfx.StopAllVfx();
    }

    private void ReadyToStrike(float lightningDamage)
    {
        Instantiate(lightningStrikeVfx, transform.position, Quaternion.identity);
        entityHealth.ReduceHealth(lightningDamage);
    }

    private IEnumerator ElectricityEffectCo(float duration)
    {
        currentEffect = ElementalType.Lightning;
        entityvfx.PlayOnStatusVfx(duration, ElementalType.Lightning);
        
        yield return new WaitForSeconds(duration);
        StopElectricityEffect();
    }

    public void AppliedBurnEffect(float duration, float fireDamage)
    {
        float fireResistance = stats.GetElementalResist(ElementalType.Fire);
        float reduceDamage = fireDamage * (1 - fireResistance);
        StartCoroutine(BurnEffectCo(duration, reduceDamage));
    }

    private IEnumerator BurnEffectCo(float duration, float totalDamage)
    {
        currentEffect = ElementalType.Fire;
        entityvfx.PlayOnStatusVfx(duration, ElementalType.Fire);

        int tickPerSecond = 2;
        int tickCount = Mathf.RoundToInt(tickPerSecond * duration);
        
        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f / tickPerSecond;

        for (int i = 0; i < tickCount; i++)
        {
            entityHealth.ReduceHealth(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }
        currentEffect = ElementalType.None;
    }

    public void AppliedChillEffect(float duration, float slowMultiplier)
    {
        float iceResistance = stats.GetElementalResist(ElementalType.Ice);
        float reduceDuration = duration * (1 - iceResistance);
        
        StartCoroutine(ChillEffectCo(reduceDuration, slowMultiplier));
    }

    private IEnumerator ChillEffectCo(float duration, float slowMultiplier)
    {
        entity.EntitySlowDown(duration, slowMultiplier);
        currentEffect = ElementalType.Ice;
        entityvfx.PlayOnStatusVfx(duration, ElementalType.Ice);
        yield return new WaitForSeconds(duration);
        currentEffect = ElementalType.None;
    }

    public bool CanBeApplied(ElementalType element)
    {
        if (element == ElementalType.Lightning && currentEffect == ElementalType.Lightning) return true;
        
        return currentEffect == ElementalType.None;
    }
}
