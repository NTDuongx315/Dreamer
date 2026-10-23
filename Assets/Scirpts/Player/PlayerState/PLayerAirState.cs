using UnityEngine;

public class PLayerAirState : PlayerState
{
    public PLayerAirState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        if (player.moveInput.x != 0) player.setVelocity(player.moveInput.x * (player.moveSpeed * player.inAirMoveMultiplier), rb.linearVelocity.y);
        
        if(input.Player.Attack.WasPressedThisFrame()) stateMachine.changeState(player.jumpAttackState);
    }
}
