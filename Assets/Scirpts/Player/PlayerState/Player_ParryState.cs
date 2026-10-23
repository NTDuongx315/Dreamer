using UnityEngine;

public class Player_ParryState : PlayerState
{
    private PLayer_Combat combat;
    private bool hasParry;
    public Player_ParryState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        combat = player.GetComponent<PLayer_Combat>();
    }

    public override void Enter()
    {
        base.Enter();
        
        hasParry = combat.Parry();
        stateTimer = combat.getParryRecoverDuration();
        anim.SetBool("counterSucess", hasParry);
    }

    public override void Update()
    {
        base.Update();
        
        player.setVelocity(0, rb.linearVelocity.y);
        
        //why not using or (triggerCall || stateTimer<0)? Simply because if the stateTimer<0 BEFORE triggercall, 
        //the animation will change to idlestate BEFORE the animation even ends. 
        if(triggerCall) 
            stateMachine.changeState(player.idleState);
        
        if(stateTimer < 0 && !hasParry)
            stateMachine.changeState(player.idleState);
    }
}
