using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.setVelocity(0,rb.linearVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        if (player.moveInput.x == player.facingDir && player.wallDectected) return;
        if (player.moveInput.x != 0) stateMachine.changeState(player.moveState);
    }
}
