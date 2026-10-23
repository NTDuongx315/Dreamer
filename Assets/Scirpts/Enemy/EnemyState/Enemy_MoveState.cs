using UnityEngine;
using UnityEngine.Splines;

public class Enemy_MoveState : Enemy_GroundState
{
    public Enemy_MoveState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if(!enemy.GroundDectected || enemy.wallDectected) enemy.Flip();
    }

    public override void Update()
    {
        base.Update();
        enemy.setVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);
        
        if(!enemy.GroundDectected || enemy.wallDectected)
        {
            stateMachine.changeState(enemy.idleState);
        }
    }
}
