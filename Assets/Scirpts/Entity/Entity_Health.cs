using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Entity_Health : MonoBehaviour, IDamagAble
{
    private Slider healthBar;
    private Entity_VFX entityVFX;
    private Entity entity;
    private Entity_Stats stat;
    
    [SerializeField] protected float currentHealth;
    [SerializeField] protected bool isDead;
    
    [Header("On Damage KnockBack")]
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(7, 7);
    [SerializeField] private float knockBackDuration = .2f;
    [SerializeField] private float heavyKnockbackDuration = .5f;

    [Header("On Heavy Attack")] 
    [SerializeField] private float heavyAttackThreshold = .3f; //percentage of health lose to consider the attack is heavy
    protected virtual void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
        entity =  GetComponent<Entity>();
        healthBar = GetComponentInChildren<Slider>();
        stat = GetComponent<Entity_Stats>();
        
        currentHealth = stat.GetMaxHealth();
        UpdateHealthBar();
    }

    public virtual bool TakeDamage(float damage, float elementalDamage, ElementalType element, Transform damageDealer)
    {
        if (isDead) return false;
        if (AttackEvaded()) return false;
        
        Entity_Stats attackerStat = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStat != null ? attackerStat.getArmorReduction() : 0;

        float mitigation = stat.GetArmorMitigation(armorReduction);
        float physicalDamage = damage * (1 - mitigation);
        
        float resistance = stat.GetElementalResist(element);
        float elementalDamageTaken = elementalDamage * (1 - resistance);

        TakeKnockback(damageDealer, physicalDamage);
        ReduceHealth(physicalDamage + elementalDamageTaken);
        
        return true;
    }

    private void TakeKnockback(Transform damageDealer, float finalDamage)
    {
        Vector2 knockback = CalculateKnockBack(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);

        entity?.ReceiveKnockback(knockback, duration);
    }

    private bool AttackEvaded() => Random.Range(0, 100) < stat.GetEvasion();

    protected void ReduceHealth(float damage)
    {
        entityVFX?.PlayOnDamageVFx();
        currentHealth -= damage;
        UpdateHealthBar();
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }
    
    private void UpdateHealthBar()
    {
        if (healthBar == null) return;
        healthBar.value = currentHealth / stat.GetMaxHealth();
    }
    private Vector2 CalculateKnockBack(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1; //trên trục tọa dộ x, nếu target bị đánh mà 
                                                //vị trí của nó đang cao hơn dealerdamage ___dealer____target___  thì đương nhiên
                                                //tảget sẽ bị đnahs lùi về phải và ngược lại nếu target đang đứng bên trái
        Vector2 knockback = isHeavyAttacking(damage) ? heavyKnockbackPower : knockbackPower;
        knockback.x = knockback.x * direction;
        return knockback;
    }

    private float CalculateDuration(float damage) =>
        isHeavyAttacking(damage) ? heavyKnockbackDuration : knockBackDuration;
    private bool isHeavyAttacking(float damage) => damage / stat.GetMaxHealth() >= heavyAttackThreshold;
}
