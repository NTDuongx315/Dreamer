using UnityEngine;

public class Player_WallSlideState : PlayerState
{
    public Player_WallSlideState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        HandleWallSlide();
        
        if(input.Player.Jump.WasPressedThisFrame()) stateMachine.changeState(player.wallJumpState);
        
        if(player.GroundDectected)
        {
            stateMachine.changeState(player.idleState);
            
            if(player.facingDir != player.moveInput.x)
                player.Flip();
        }
        
        if(!player.wallDectected) stateMachine.changeState(player.fallState);
    }

    private void HandleWallSlide()
    {
        if (player.moveInput.y != 0) player.setVelocity(player.moveInput.x, rb.linearVelocity.y);
        else player.setVelocity(player.moveInput.x, rb.linearVelocity.y * player.wallSlideMultiplier);
    }
}
