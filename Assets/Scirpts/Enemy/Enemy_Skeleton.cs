using UnityEngine;

public class Enemy_Skeleton : Enemy, ICounterAble
{
    public bool CanBeParry { get => canBeStun; }
    protected override void Awake()
    {
        base.Awake();
        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
        deathState = new Enemy_DeathState(this, stateMachine, "death");
        stunState = new Enemy_StunState(this, stateMachine, "stun");
    }
    
    protected override void Start()
    {
        base.Start();
        stateMachine.Init(idleState);
    }
    
    public void HandleCounter()
    {
        if (!canBeStun) return;
        stateMachine.changeState(stunState);
    }
}
