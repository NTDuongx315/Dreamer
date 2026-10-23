using UnityEngine;
using System.Collections;
using System;

public class Player : Entity
{
    public static event Action OnplayerDeath;
    public PlayerInputSet input {get; private set;}
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerFallState fallState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public Player_WallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PLayerDashState dashState { get; private set; }
    public PlayerAttackState basicAttackState { get; private set; }
    public PlayerJumpAttackState jumpAttackState  { get; private set; }
    public PlayerDeathState deathState { get; private set; }
    
    public Player_ParryState parryState { get; private set; }
    
    [Header("AttackDetails")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = 0.1f;
    public float comboResetTime = 0.5f;
    private Coroutine queueAttackCo;

    [Header("Movement deatails")] 
    public float moveSpeed;
    public float jumpForce = 10;
    public Vector2 wallJumpDir;
    
    [Range(0,1)]
    public float inAirMoveMultiplier = .7f;
    [Range(0,1)]
    public float wallSlideMultiplier = .25f;

    [Space] 
    public float dashDuration = .35f;
    public float dashForce = 25;
    public Vector2 moveInput { get; private set; }
    
    [Header("After death")]
    public float airAttackDuration;
    private Coroutine delay;


    protected override void Awake()
    {
        base.Awake();
        input = new PlayerInputSet();
        idleState = new PlayerIdleState(this, stateMachine,"idle");
        moveState = new PlayerMoveState(this, stateMachine,"move");
        fallState = new PlayerFallState(this, stateMachine,"jumpFall");
        jumpState = new PlayerJumpState(this, stateMachine,"jumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine,"wallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine,"jumpFall");
        dashState = new PLayerDashState(this, stateMachine,"dash");
        basicAttackState = new PlayerAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new PlayerJumpAttackState(this, stateMachine, "jumpAttack");
        deathState = new PlayerDeathState(this, stateMachine, "dead");
        parryState = new Player_ParryState(this, stateMachine, "counter");
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        stateMachine.changeState(deathState);
        StartCoroutine(DelayDeathEvent());
    }

    private IEnumerator DelayDeathEvent()
    {
        yield return new WaitForSeconds(airAttackDuration);
        rb.simulated = false;
        OnplayerDeath?.Invoke();
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Init(idleState);
    }
    
    public void EnterAttackStateWithDelay()
    {
        if (queueAttackCo != null) StopCoroutine(queueAttackCo);

        queueAttackCo = StartCoroutine(EnterAttackState());
    }

    private IEnumerator EnterAttackState()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.changeState(basicAttackState);
    }
    private void OnEnable()
    {
        input.Enable();
        
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        input.Disable();
    }
    
}
