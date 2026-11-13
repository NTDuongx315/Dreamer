using UnityEditor.Rendering;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX entityVFX;
    private Entity_Stats stat;

    [SerializeField] private DamageScaleData basicScale;
    
    [Header("target detection")]
    [SerializeField] private Transform target;
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private LayerMask whatIsTarget;
    
    private void Awake()
    {
        entityVFX = GetComponent<Entity_VFX>();
        stat = GetComponent<Entity_Stats>();
    }
    public void PerformAttack()
    {
        foreach (var target in GetDetectedCollider())
        {
            //interface sẽ thay thế nhiệm vụ gọi hàm TakeDamage, cơ chế chính của interface là bắt buộc phải set 1 
            //function cố dịnh cho nó trước và yêu cầu bắt buộc các object nếu muốn được gọi phải chứa function đó 
            //bằng cách getComponent thông qua interface, nó sẽ lấy chính xác và duy nhất function được tạo đó 
            // và dùng cho chính xác object được chỉ định tới
            //ví dụ cả chést object và entity object đều chứa interface. 
            //player chém vào chest thì hàm TakeDamage của chính xác chest sẽ được gọi và không gì khác.
            //điều này là cần thiết thay cho việc phải khai báo lại chest như 1 character là thừa thãi.
            
            IDamagAble damagAble = target.GetComponent<IDamagAble>();
            
            if (damagAble == null) continue;//skip target, go to next target

            AttackData attackData = stat.GetAttackData(basicScale);
            bool targetGotHit = damagAble.TakeDamage(attackData.physicalDamage, attackData.elementalDamage, attackData.element , transform);
            
            if (attackData.element != ElementalType.None)
                target.GetComponent<Entity_StatusHandler>().ApplyStatusEffect(attackData.element, attackData.effectData);
            
            if(targetGotHit)
                entityVFX.CreateOnHitVfx(target.transform, attackData.isCrit, attackData.element);
        }
    }

    protected Collider2D[] GetDetectedCollider()
    {
        return Physics2D.OverlapCircleAll(target.position, targetCheckRadius, whatIsTarget);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(target.position, targetCheckRadius);
    }
}
