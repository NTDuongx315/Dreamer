using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy => GetComponent<Enemy>();
    public override bool TakeDamage(float damage, float elementalDamage, ElementalType element, Transform damageDealer)
    {
        bool wasHit = base.TakeDamage(damage, elementalDamage, element, damageDealer);
        
        if(!wasHit) return false;
        
        if (!isDead && damageDealer.GetComponent<Player>() != null)
            enemy.TryEnterBattleState(damageDealer);
        
        return true;
    }
}
