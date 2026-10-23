using UnityEngine;

public class Enemy_StunState : EnemyState
{
    public Enemy_StunState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    override public void Enter()
    {
        base.Enter();
        stateTimer = enemy.stunDuration;
        rb.linearVelocity = new Vector2(enemy.stunVelocity.x * -enemy.facingDir, enemy.stunVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.changeState(enemy.idleState);
        }
    }
}
