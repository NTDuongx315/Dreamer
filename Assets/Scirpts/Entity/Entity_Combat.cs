using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    private Entity_VFX entityVFX;
    private Entity_Stats stat;
    public float damage;
    
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
            IDamagAble damagAble = target.GetComponent<IDamagAble>();
            
            if (damagAble == null) continue;//skip target, go to next target
            float damage = stat.GetDamage(out bool isCrit);
            bool targetGotHit = damagAble.TakeDamage(damage, transform);
            //interface sẽ thay thế nhiệm vụ gọi hàm TakeDamage, cơ chế chính của interface là bắt buộc phải set 1 
            //function cố dịnh cho nó trước và yêu cầu bắt buộc các object nếu muốn được gọi phải chứa function đó 
            //bằng cách getComponent thông qua interface, nó sẽ lấy chính xác và duy nhất function được tạo đó 
            // và dùng cho chính xác object được chỉ định tới
            //ví dụ cả chést object và entity object đều chứa interface. 
            //player chém vào chest thì hàm TakeDamage của chính xác chest sẽ được gọi và không gì khác.
            //điều này là cần thiết thay cho việc phải khai báo lại chest như 1 character là thừa thãi.
            
            // Entity_Health targetHealth = target.GetComponent<Entity_Health>();
            //
            // if(targetHealth != null)
            //     targetHealth?.TakeDamage(damage, transform);
            if(targetGotHit)
                entityVFX.CreateOnHitVfx(target.transform, isCrit);
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
