using Unity.VisualScripting;
using UnityEngine;

public class PlayerGroundState : PlayerState
{
    public PlayerGroundState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        
        if (rb.linearVelocity.y < 0 && !player.GroundDectected) stateMachine.changeState(player.fallState);
        if (input.Player.Jump.WasPressedThisFrame()) stateMachine.changeState(player.jumpState);
        if (input.Player.Attack.WasPressedThisFrame()) stateMachine.changeState((player.basicAttackState));
        if (input.Player.Parry.WasPressedThisFrame()) stateMachine.changeState(player.parryState);
        
    }
}
 