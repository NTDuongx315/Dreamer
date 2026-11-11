using System;
using UnityEngine;

public class Skill_ObjectBase : MonoBehaviour
{
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetPosition;
    [SerializeField] protected float checkRadius = 1;

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

            damagable.TakeDamage(1, 1, ElementalType.None, transform);
        }
    }
    
    protected void OnDrawGizmos()
    {
        if(targetPosition == null) targetPosition = transform;
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
