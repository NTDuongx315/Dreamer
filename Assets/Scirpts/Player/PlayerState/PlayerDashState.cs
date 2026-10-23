using UnityEngine;
using UnityEngine.PlayerLoop;

public class PLayerDashState : PlayerState
{
    private float originalGravity;
    private float dashDirection;
    public PLayerDashState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        dashDirection = player.moveInput.x != 0 ? (int)player.moveInput.x : player.facingDir;
        stateTimer = player.dashDuration;
        
        originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
    }

    public override void Update()
    {
        base.Update();
        dashToWallSlide();

        player.setVelocity(player.dashForce * dashDirection, 0);

        if (stateTimer < 0)
        {
            if (player.GroundDectected) stateMachine.changeState(player.idleState);
            else stateMachine.changeState(player.fallState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.setVelocity(0, 0);
        rb.gravityScale = originalGravity;
    }

    private void dashToWallSlide()
    {
        if (player.wallDectected)
        {
            if(player.GroundDectected) stateMachine.changeState(player.idleState);
            else stateMachine.changeState(player.wallSlideState);    
        }
    }
}
