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

            AttackData attackData = playerStats.GetAttackData(damageScale);

            damagable.TakeDamage(attackData.physicalDamage, attackData.elementalDamage, attackData.element , transform);
            
            if(attackData.element != ElementalType.None)
                target.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(attackData.element, attackData.effectData);
            
            usedElement = attackData.element;
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
