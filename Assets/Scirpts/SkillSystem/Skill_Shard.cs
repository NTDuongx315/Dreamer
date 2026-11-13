using System.Collections;
using UnityEngine;

public class Skill_Shard : Skill_Base
{
    private Skill_ObjectShard currentShard;
    private Entity_Health playerHealth;
    
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private float detonateTime = 2;

    [Header("Movin Shard Upgrade")] 
    [SerializeField] private float speed = 7;

    [Header("Multicast Upgrade")] 
    [SerializeField] private int multicastTime = 3;
    [SerializeField] private int curCharge;
    [SerializeField] private bool isCharging;
    
    [Header("Teleport Shard Upgrade")]
    [SerializeField] private float shardExistDuration = 10;
    [SerializeField] private float maxDistance = 15;

    [Header("Health Rewind Shard Upgrade")]
    [SerializeField] private float savedHealthPercent;
    

    protected override void Awake()
    {
        base.Awake();
        curCharge = multicastTime;
        playerHealth = GetComponentInParent<Entity_Health>();
    }

    public override void TryUseSkill()
    {
        if (!CanUseSkill()) return;
        
        if (Unlocked(SkillUpgradeType.Shard))
            HandleShardRegular();
        
        if (Unlocked(SkillUpgradeType.Shard_MoveToEnemy))
            HandleShardMoving();

        if (Unlocked(SkillUpgradeType.Shard_Multicast))
            HandleMultiCast();
        
        if(Unlocked(SkillUpgradeType.Shard_Teleport))
            HandleShardTeleport();
        
        if(Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            HandleShardHealRewind();
    }

    private void HandleShardHealRewind()
    {
        if(currentShard == null)
        {
            CreateShard();
            savedHealthPercent = playerHealth.GetHealthPercent();
        }
        else
        {
            SwapShard();
            playerHealth.SetHealthPercent(savedHealthPercent);
            SetSkillOnCooldown();
        }
    }

    private void HandleShardTeleport()
    {
        if(currentShard == null) CreateShard();
        else
        {
            SwapShard();
            SetSkillOnCooldown();
        }
    }

    private void SwapShard()
    {
        Vector3 shardPos = currentShard.transform.position;
        Vector3 playerPos = player.transform.position;
        
        currentShard.transform.position = playerPos;
        currentShard.Explode();
        
        player.TeleportPlayer(shardPos);
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
        currentShard.SetUpShard(this);

        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            currentShard.onExplode += ForceCoolDown;
    }

    public float GetDetonateTime()
    {
        if (Unlocked(SkillUpgradeType.Shard_Teleport) || Unlocked(SkillUpgradeType.Shard_TeleportHpRewind))
            return shardExistDuration;
        
        return detonateTime;
    }

    private void ForceCoolDown()
    {
        if(!OnCoolDown())
        {
            SetSkillOnCooldown();
            currentShard.onExplode -= ForceCoolDown;
        }
    }
}
