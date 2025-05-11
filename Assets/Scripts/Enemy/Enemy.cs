using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class Enemy : MonoBehaviour
{
    protected SpriteRenderer sr => GetComponent<SpriteRenderer>();
    [SerializeField] protected float movespeed;
    protected bool canMove;
    protected Rigidbody2D rb;
    protected Collider2D[] cd;
    protected Animator anim;
    protected int facingDirection = -1;
    protected bool facingRight = false;
    protected bool isGroundedInFront;
    protected bool isWallDetected;
    protected bool isGrounded;
    protected float idleDuration=1.5f;
    protected float idleTime;
    protected Transform player;

    [Header("Check Collision")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected LayerMask whatGround;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsPlayer;
    protected bool IsplayerDetected;
    [SerializeField] protected float playerDetectionRange=15;

    [Header("Dead Details")]
    [SerializeField] private float deadImpact;
    [SerializeField] private float deadRotationSpeed;
    protected bool isDead;
    private int deathRotationDirection = 1;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponentsInChildren<Collider2D>();
        
    }
    protected virtual void Start()
    {
        if(sr.flipX== true && !facingRight)
        {
            sr.flipX = false;
            Flip();
        }
    }
    //protected void UpdatePlayerPrefs()
    //{
    //    if (player == null || player.Equals(null))
    //    {
    //        player = PlayerManager.instance.player.transform;
    //    }

    //    //player = PlayerManager.instance.player.transform;

    //}
    protected virtual void Update() { 
        HandleAnimation();
        HandleColision();
        idleTime -= Time.deltaTime;
        if (isDead)
        {
            HandleDeathRotation();
        }
        
    }
    protected virtual void Flip()
    {
        facingRight = !facingRight;
        facingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }
    protected virtual void FlipController(float xValue)
    {
        if (xValue > transform.position.x && !facingRight || xValue < transform.position.x && facingRight)
        {
            Flip();
        }

    }
    [ContextMenu("Change Facing Dir")]
    public void FlipDefaultFacingDir()
    {
        sr.flipX = !sr.flipX;
    }
    protected virtual void HandleColision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatGround);
        isGroundedInFront = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatGround);
        IsplayerDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, playerDetectionRange, whatIsPlayer);
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (facingDirection * wallCheckDistance), transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (playerDetectionRange * facingDirection), transform.position.y));
    }
    public virtual void Die()
    {
        //cd.enabled = false;
        if (rb.isKinematic)
        {
            rb.isKinematic = false;
        }
        EnableColliders(false);
        anim.SetTrigger("hit");
        rb.velocity = new Vector2(rb.velocity.x, deadImpact);
        isDead = true;
        if (Random.Range(0, 100) < 50)
        {
            deathRotationDirection = deathRotationDirection * -1;
        }
        //PlayerManager.OnPlayerRespawn -= UpdatePlayerPrefs;
    }

    protected void EnableColliders(bool enable)
    {
        foreach (var collider in cd)
        {
            collider.enabled = enable;
        }
    }

    private void HandleDeathRotation() 
    {
        transform.Rotate(0f, 0f, (deadRotationSpeed * deathRotationDirection) * Time.deltaTime );
    }
    protected virtual void HandleAnimation()
    {
        anim.SetFloat("xVelocity",rb.velocity.x);
    }
}
