using UnityEngine;

public class Chest : MonoBehaviour, IDamagAble
{
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private Entity_VFX fx => GetComponent<Entity_VFX>();
    public bool TakeDamage(float damage, float elementalDamage, Transform damageDealer)
    {
        anim.SetBool("open", true);
        fx.PlayOnDamageVFx();
        rb.simulated = false;
        
        return true;
    }
}
