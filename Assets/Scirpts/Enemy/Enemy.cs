using System;
using System.Collections;
using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeathState deathState;
    public Enemy_StunState stunState;
    
    [Header("Battle details")]
    public float battleSpeed = 3;
    public float attackDistance = 2;
    public float battleTimeDurration = 5;
    public float minRetreatDistance = 1;
    public Vector2 retreatVelocity;

    [Header("Stun state details")] 
    public float stunDuration = 1;
    public Vector2 stunVelocity = new Vector2(3, 3);
    [SerializeField] protected bool canBeStun;

    [Header("Movement details")] 
    public float moveSpeed;
    public float idleTimer;
    [Range(0,2)]
    public float animSpeed;
    
    [Header("Player Detection")] 
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance;
    public Transform player {get; private set;}

    protected override IEnumerator EntitySlowDownCo(float duration, float slowMultiplier)
    { 
        float originalMoveSpeed = moveSpeed;
        float originalAnimSpeed = anim.speed;
        float originalBattleSpeed = battleSpeed;
        
        float speedMultiplier = 1 - slowMultiplier;
        
        moveSpeed = originalMoveSpeed * speedMultiplier;
        anim.speed = originalAnimSpeed * speedMultiplier;
        battleSpeed = originalBattleSpeed * speedMultiplier;
        
        yield return new WaitForSeconds(duration);
        
        moveSpeed = originalMoveSpeed;
        anim.speed = originalAnimSpeed;
        battleSpeed = originalBattleSpeed;
    }

    public void EnableCounterWindow(bool varEnable) => canBeStun = varEnable;

    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.changeState(deathState);
    }

    private void HandlePlayerDeath()
    {
            stateMachine.changeState(idleState);
    }

    public void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState == battleState || stateMachine.currentState == attackState) return;
        
        this.player = player;
        stateMachine.changeState(battleState);
    }

    public Transform GetPlayerReference()
    {
        if (player == null) player = playerDetection().transform;
        return player;
    }

    public RaycastHit2D playerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer | whatIsGround);
        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player")) return default;
        return hit;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(playerCheckDistance * facingDir,0));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(attackDistance * facingDir,0));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, playerCheck.position + new Vector3(minRetreatDistance * facingDir,0));
    }

    private void OnEnable()
    {
        Player.OnplayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnplayerDeath -= HandlePlayerDeath;
    }
}
