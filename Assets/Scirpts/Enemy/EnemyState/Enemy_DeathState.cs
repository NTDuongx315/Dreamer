using UnityEngine;

public class Enemy_DeathState : EnemyState
{
    public Enemy_DeathState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        stateMachine.SwitchOffStateMachine();
    }
}
