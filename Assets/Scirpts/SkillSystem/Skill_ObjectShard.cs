using System;
using UnityEngine;

public class Skill_ObjectShard : Skill_ObjectBase
{
    [SerializeField] private GameObject shardPrefab;

    public void SetUpShard(float destinationTime)
    {
        Invoke(nameof(Explode), destinationTime);
    }
    
    private void Explode()
    {
        DamageEnemiesInRadius(transform, checkRadius);
        //make a copy of the prefab, place it on the screen follow by transform.position and rotation: Quaternion.identity
        Instantiate(shardPrefab, transform.position, Quaternion.identity); 
        
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() == null) return;
        Explode();
    }
}
