using UnityEngine;

public class PlayerJumpAttackState : PlayerState
{
    private bool touchGround;
    public PlayerJumpAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Update()
    {
        base.Update();
        if (player.GroundDectected && touchGround == false)
        {
            touchGround = true;
            anim.SetTrigger("attackTrigger");
            player.setVelocity(0, rb.linearVelocity.y);
        }
        
        if(triggerCall && player.GroundDectected) stateMachine.changeState(player.idleState);
    }

    public override void Enter()
    {
        base.Enter();
        touchGround = false;

        player.setVelocity(player.jumpAttackVelocity.x * player.facingDir, player.jumpAttackVelocity.y);
    }
}
