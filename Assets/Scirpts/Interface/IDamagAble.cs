using UnityEngine;

public interface IDamagAble
{
    public bool TakeDamage(float damage, float elementalDamage, ElementalType element, Transform damageDealer);
}
