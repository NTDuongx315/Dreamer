using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.setVelocity(player.wallJumpForce.x * -player.facingDir, player.wallJumpForce.y);
    }

    public override void Update()
    {
        base.Update();
        
        if(rb.linearVelocity.y < 0) stateMachine.changeState(player.fallState);
        if(player.wallDectected) stateMachine.changeState(player.wallSlideState);
    }
}
