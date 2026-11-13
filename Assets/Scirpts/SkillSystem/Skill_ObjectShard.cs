using System;
using UnityEngine;

public class Skill_ObjectShard : Skill_ObjectBase
{
    public event Action onExplode;
    private Skill_Shard shardManage;
    
    [SerializeField] private GameObject shardPrefab;
    private Transform target;
    private float speed;

    private void Update()
    {
        if (target == null) return;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    //on the Update we have the basic movement of where the shard would go whihc is toward to the target but target is still unknown
    // so we make this function.
    public void MoveTowardsClosestTarget(float speed)
    {
        target = FindClosestTarget();
        this.speed = speed;
    }

    public void SetUpShard(Skill_Shard shard)
    {
        this.shardManage = shard;
        playerStats = shard.player.stat;
        damageScale = shard.damageScaleData;
        
        Invoke(nameof(Explode), shard.GetDetonateTime());
    }
    
    public void Explode()
    {
        DamageEnemiesInRadius(transform, checkRadius);
        //make a copy of the prefab, place it on the screen follow by transform.position and rotation: Quaternion.identity
        Instantiate(shardPrefab, transform.position, Quaternion.identity); 
        
        onExplode?.Invoke();
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null) return;
        Explode();
    }
}
