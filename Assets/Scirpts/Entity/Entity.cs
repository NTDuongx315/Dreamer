using UnityEngine;
using System.Collections;
using System;

public class Entity : MonoBehaviour
{
    public event Action OnFlipped;
    public Animator anim {get; private set;}
    protected StateMachine stateMachine { get; private set; }
    public Rigidbody2D rb {get; private set;}
    public Entity_Stats stat {get; private set;}
    
    private bool facingRight = true;
    public int facingDir { get; private set; } = 1;

    [Header("Collision Detection")] 
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform primaryWallCheck;
    [SerializeField] private Transform secondaryWallCheck;
    public bool GroundDectected { get; private set; }
    public bool wallDectected { get; private set; }
    
    //Condition variables
    private bool isKnockback;
    private Coroutine knockBackCo;
    private Coroutine slowDownCo;
    
    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        stat = GetComponent<Entity_Stats>();
        
        stateMachine = new StateMachine();
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        HandleCollisionDetection();
        stateMachine.UpdateActiveState(); //this Update function is from EntityState class because in the Start() above,
        // stateMachine call Init() means currentState is now assigned as IdleState. So stateMachine.currentState is actually IdleState
    }

    public void currentAnimationTrigger()
    {
        stateMachine.currentState.animationTrigger();
    }

    public virtual void EntityDeath()
    {
        
    }

    public virtual void EntitySlowDown(float duration, float slowMultiplier)
    {
        if(slowDownCo != null) StopCoroutine(slowDownCo);
        slowDownCo = StartCoroutine(EntitySlowDownCo(duration, slowMultiplier));
    }

    protected virtual IEnumerator EntitySlowDownCo(float duration, float slowMultiplier)
    {
        yield return null;
    }

    public void ReceiveKnockback(Vector2 knockback, float knockbackDuration)
    {
        if (knockBackCo != null) StopCoroutine(knockBackCo);
        knockBackCo = StartCoroutine(KnockbackCo(knockback, knockbackDuration));
    }

    private IEnumerator KnockbackCo(Vector2 knockback, float knockbackDuration)
    {
        isKnockback = true;
        rb.linearVelocity = knockback;
        
        yield return new WaitForSeconds(knockbackDuration);
        
        rb.linearVelocity = Vector2.zero;
        isKnockback = false;
    }    

    public void setVelocity(float x, float y)
    {
        if (isKnockback) return;
        
        rb.linearVelocity = new Vector2(x, y);
        HandleFlip(x);
    }

    public void HandleFlip(float x)
    {
        if (x > 0 && facingRight == false) Flip();
        else if (x < 0 && facingRight == true) Flip();
    }
    
    public void Flip()
    {
        transform.Rotate(0,180,0);
        facingRight = !facingRight;
        facingDir = -facingDir;
        
        OnFlipped?.Invoke();
    }

    private void HandleCollisionDetection()
    {
        GroundDectected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        //GroundDectected = Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y), new Vector2(0.35f, 1f), 0f, Vector2.down, groundCheckDistance, whatIsGround);
        if (secondaryWallCheck != null)
            wallDectected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance,
                                whatIsGround)
                            && Physics2D.Raycast(secondaryWallCheck.position, Vector2.right * facingDir,
                                wallCheckDistance,
                                whatIsGround);
        else
            wallDectected = Physics2D.Raycast(primaryWallCheck.position, Vector2.right * facingDir, wallCheckDistance,
                whatIsGround);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + new Vector3(0,-groundCheckDistance));
        Gizmos.DrawLine(primaryWallCheck.position, primaryWallCheck.position + new Vector3(wallCheckDistance * facingDir,0));
        if(secondaryWallCheck !=null)
        Gizmos.DrawLine(secondaryWallCheck.position, secondaryWallCheck.position + new Vector3(wallCheckDistance * facingDir,0));
    }
}
