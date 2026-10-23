using UnityEngine;

//mô hình là đầu tiên Enter() vào state rồi Update() để trigger và mô tả diễn biến state đó, và Exit() để cbi sang state khác

//why abstract? using abstract so the program knows that the class is only a blueprint that can not be used except its inheritance
//just to mkae sure people cant use the EntityState class freely.
public abstract class PlayerState : EntityState //how EACH state behave
{
    protected Player player;
    protected PlayerInputSet input;
    
    public PlayerState(Player player,StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.player = player;
        
        anim = player.anim;
        rb = player.rb;
        input = player.input;
    }

    public override void Update()
    {
        base.Update();
        if(input.Player.Dash.WasPressedThisFrame() && CanDash()) stateMachine.changeState(player.dashState);
        // if(input.Player.Attack.WasPressedThisFrame()) stateMachine.changeState((player.basicAttackState));
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private bool CanDash()
    {
        if(player.wallDectected) return false;
        if(stateMachine.currentState == player.dashState) return false;
        return true;
    }
}
