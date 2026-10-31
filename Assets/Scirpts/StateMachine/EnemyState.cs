using UnityEngine;

public abstract class EnemyState : EntityState
{
    protected Enemy enemy;

    protected EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;
        rb = enemy.rb;
        anim = enemy.anim;
        stat = enemy.stat;
    }
    

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        
        float battleAnimSppedMulti = enemy.battleSpeed / enemy.moveSpeed;
        anim.SetFloat("aniSpeedMulti", enemy.animSpeed);
        anim.SetFloat("aniBattleSpeedMulti", battleAnimSppedMulti);
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
    }
}
