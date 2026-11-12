using System.Collections;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private Skill_ObjectShard currentShard;
    
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonateTime = 2;

    [Header("Movin Shard Upgrade")] 
    [SerializeField] private float speed = 7;

    [Header("Multicast Upgrade")] 
    [SerializeField] private int multicastTime = 3;
    [SerializeField] private int curCharge;
    [SerializeField] private bool isCharging;

    public override void TryUseSkill()
    {
        if (!CanUseSkill()) return;
        
        if (Unlocked(SkillUpgradeType.Shard))
            HandleShardRegular();
        
        if (Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
            HandleShardMoving();

        if (Unlocked(SkillUpgradeType.Shard_Multicast))
            HandleMultiCast();
    }

    private void HandleMultiCast()
    {
        if (curCharge <= 0) return;
        
        CreateShard();
        currentShard.MoveTowardsClosestTarget(speed);
        curCharge--;
    
        if (!isCharging) StartCoroutine(ShardRechargeCo());
    }
    
    private IEnumerator ShardRechargeCo()
    {
        isCharging = true;
        while (curCharge < multicastTime)
        {
            yield return new WaitForSeconds(cooldown);
            curCharge++;
        }
        isCharging = false;
    }

    private void HandleShardMoving()
    {
        CreateShard();
        currentShard.MoveTowardsClosestTarget(speed);
        SetSkillOnCooldown();
    }

    private void HandleShardRegular()
    {
        CreateShard();
        SetSkillOnCooldown();
    }

    public void CreateShard()
    {
        GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
        currentShard = shard.GetComponent<Skill_ObjectShard>();
        currentShard.SetUpShard(detonateTime);
    }
}
