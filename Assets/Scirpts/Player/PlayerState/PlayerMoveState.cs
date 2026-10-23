using UnityEngine;

public class PlayerMoveState : PlayerGroundState
{
    public PlayerMoveState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        
        if(player.moveInput.x == 0 || player.wallDectected) stateMachine.changeState(player.idleState);
        player.setVelocity(player.moveInput.x * player.moveSpeed, rb.linearVelocity.y);
    }
}
