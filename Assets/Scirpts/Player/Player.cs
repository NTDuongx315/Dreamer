using UnityEngine;
using System.Collections;
using System;

public class Player : Entity
{
    private UI ui;
    public static event Action OnplayerDeath;
    public PlayerInputSet input {get; private set;}
    public Player_SkillManager SkillManager { get; private set; }
    public Player_VFX vfx { get; private set; }
    public Entity_Health health { get; private set; }
    public Entity_StatusHandler statusHandler { get; private set; }

    #region StateVar
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
    public Player_SwordThrowState  swordThrowState { get; private set; }
    
    #endregion
    
    [Header("AttackDetails")]
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = 0.1f;
    public float comboResetTime = 0.5f;
    private Coroutine queueAttackCo;

    [Header("Movement deatails")] 
    public float moveSpeed;
    public float jumpForce = 10;
    public Vector2 wallJumpForce;
    
    [Range(0,1)]
    public float inAirMoveMultiplier = .7f;
    [Range(0,1)]
    public float wallSlideMultiplier = .25f;

    [Space] 
    public float dashDuration = .35f;
    public float dashForce = 25;
    public Vector2 moveInput { get; private set; }
    public Vector2 mousePosition { get; private set; }
    
    [Header("After death")]
    public float airAttackDuration;
    private Coroutine delay;


    protected override void Awake()
    {
        base.Awake();

        ui = FindAnyObjectByType<UI>();
        vfx = GetComponent<Player_VFX>();
        health = GetComponent<Entity_Health>();
        statusHandler = GetComponent<Entity_StatusHandler>();
        SkillManager = GetComponent<Player_SkillManager>();
        
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
        swordThrowState = new Player_SwordThrowState(this,  stateMachine, "swordThrow");
    }
    
    protected override IEnumerator EntitySlowDownCo(float duration, float slowMultiplier)
    { 
        float originalMoveSpeed = moveSpeed;
        float originalAnimSpeed = anim.speed;
        float originalJumpForce = jumpForce;
        Vector2 originalWallJump = wallJumpForce;
        Vector2 originalJumpAttack = jumpAttackVelocity;
        Vector2[] originalAttackSpeed = new Vector2[attackVelocity.Length];
        Array.Copy(attackVelocity,originalAttackSpeed, attackVelocity.Length);
        
        float speedMultiplier = 1 - slowMultiplier;
        
        moveSpeed = originalMoveSpeed * speedMultiplier;
        anim.speed = originalAnimSpeed * speedMultiplier;
        jumpForce = originalJumpForce * speedMultiplier;
        wallJumpForce = originalWallJump * speedMultiplier;
        jumpAttackVelocity = originalJumpAttack  * speedMultiplier;
        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = attackVelocity[i] * speedMultiplier;
        }
        
        yield return new WaitForSeconds(duration);
        
        moveSpeed = originalMoveSpeed;
        anim.speed = originalAnimSpeed;
        jumpForce = originalJumpForce;
        wallJumpForce = originalWallJump;
        jumpAttackVelocity = originalJumpAttack;
        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = originalAttackSpeed[i];
        }
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
    
    public void TeleportPlayer(Vector3 position) => transform.position = position;
    
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
     
        input.Player.Mouse.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
        
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
        input.Player.ToggleSkillTreeUI.performed += ctx => ui.ToggleSkillTree();
        input.Player.Spell.performed += ctx => SkillManager.shard.TryUseSkill();
    }

    private void OnDisable()
    {
        input.Disable();
    }
    
}
