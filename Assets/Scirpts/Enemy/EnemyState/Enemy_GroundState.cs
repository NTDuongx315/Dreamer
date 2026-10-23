using UnityEngine;

public class Enemy_GroundState : EnemyState
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Enemy_GroundState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        if (enemy.playerDetection())
        {
            stateMachine.changeState(enemy.battleState);
        }
    }
}
