using UnityEngine;

public class PlayerFallState : PLayerAirState
{
    public PlayerFallState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        if (player.GroundDectected)
        {
            stateMachine.changeState(player.idleState);
        }
        
        if (player.wallDectected)
        {
            stateMachine.changeState(player.wallSlideState);
        }
    }
}
