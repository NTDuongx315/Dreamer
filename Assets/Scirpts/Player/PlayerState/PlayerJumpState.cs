using UnityEngine;

public class PlayerJumpState : PLayerAirState
{
    public PlayerJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.setVelocity(rb.linearVelocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();
        if(rb.linearVelocity.y < 0 && stateMachine.currentState != player.jumpAttackState) stateMachine.changeState(player.fallState);
    }
}
