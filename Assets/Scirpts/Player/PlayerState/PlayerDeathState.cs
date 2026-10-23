using UnityEngine;

public class PlayerDeathState : PlayerState
{
    public PlayerDeathState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        stateMachine.SwitchOffStateMachine();
    }
}
