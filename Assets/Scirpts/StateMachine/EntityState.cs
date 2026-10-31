using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string animBoolName;
    
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Entity_Stats stat;
    
    protected float stateTimer;
    protected bool triggerCall;
    
    public EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }
    
    public virtual void Enter() //everytime state will be changed, enter will be called
    {
        anim.SetBool(animBoolName, true);
        triggerCall = false;
    }
    
    public virtual void Update() //run logic of the state
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();
    }

    public virtual void Exit() //called everytime whenever exit stage and change to a new one
    {
        anim.SetBool(animBoolName, false);
    }

    public void animationTrigger()
    {
        triggerCall = true;
    }

    public void SyncAttackSpeed()
    {
        float attackSpeed = stat.offense.attackSpeed.getValue();
        anim.SetFloat("attackSpeedMultiplier", attackSpeed);
    }

    public virtual void UpdateAnimationParameters()
    {
        
    }
}
