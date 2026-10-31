using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class PlayerAttackState : PlayerState
{
    private float attackVelocityTimer;
    private float lastTimeAttacked;
    
    private bool comboAttackQueue;
    private int attackDir;
    private const int FirstComboIndex = 1;
    private int comboIndex = 1;
    private int comboLimit = 3;
    
    public PlayerAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }
    

    public override void Enter()
    {
        base.Enter();
        comboAttackQueue = false;
        ResetCombo();
        SyncAttackSpeed();

        attackDir = player.moveInput.x != 0 ? (int)player.moveInput.x : player.facingDir;
        
        anim.SetInteger("basicAttackIndex", comboIndex);
        
        applyAttackVelocity();
        
    }

    private void ResetCombo()
    {
        if (comboIndex > comboLimit || lastTimeAttacked + player.comboResetTime < Time.time) comboIndex = FirstComboIndex;
    }

    public override void Update()
    { 
        base.Update();
        attackVelocityTimer -= Time.deltaTime;
        handleAttackVelocity();
        if (input.Player.Attack.WasPressedThisFrame()) QueueNextAttack();
        
        if(triggerCall) HandleExit();
    }

    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        lastTimeAttacked = Time.time;
    }

    private void HandleExit()
    {
        if(comboAttackQueue)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else stateMachine.changeState(player.idleState);
    }

    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit) comboAttackQueue = true;
    }

    private void handleAttackVelocity()
    {
        if (attackVelocityTimer <= 0) player.setVelocity(0, rb.linearVelocity.y);
    }

    private void applyAttackVelocity()
    {
        attackVelocityTimer = player.attackVelocityDuration;
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];
        player.setVelocity(attackVelocity.x * attackDir, attackVelocity.y);
    }
}
