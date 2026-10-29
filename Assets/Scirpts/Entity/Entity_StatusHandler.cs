using System;
using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityvfx;
    private Entity_Stats stats;
    private ElementalType currentEffect = ElementalType.None;

    private void Awake()
    {
        stats = GetComponent<Entity_Stats>();
        entity = GetComponent<Entity>();
        entityvfx = GetComponent<Entity_VFX>();
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
        return currentEffect == ElementalType.None;
    }
}
