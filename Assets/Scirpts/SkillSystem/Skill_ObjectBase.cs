using System;
using UnityEngine;

public class Skill_ObjectBase : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetPosition;
    [SerializeField] protected float checkRadius = 1;

    protected Entity_Stats playerStats;
    protected DamageScaleData damageScale;
    protected ElementalType usedElement;

    protected Collider2D[] EnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }
    
    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        foreach (var target in EnemiesAround(targetPosition, radius))
        {
            IDamagAble damagable = target.GetComponent<IDamagAble>();
            if (damagable == null) continue;
            
            ElementalEffectData effect = new ElementalEffectData(playerStats, damageScale);

            float physicDamage = playerStats.GetDamage(out bool isCrit, damageScale.physical);
            float elementalDamage = playerStats.GetElementalDamage(out ElementalType element, damageScale.element);

            damagable.TakeDamage(physicDamage, elementalDamage, element, transform);
            
            if(element != ElementalType.None)
                target.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(element, effect);
            
            usedElement = element;
        }
    }

    protected Transform FindClosestTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in EnemiesAround(targetPosition, 10))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                target = enemy.transform;
                closestDistance = distance;
            }
        }
        return target;
    }
    
    protected void OnDrawGizmos()
    {
        if(targetPosition == null) targetPosition = transform;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
