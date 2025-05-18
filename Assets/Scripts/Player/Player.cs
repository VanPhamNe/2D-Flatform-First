using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Private bool + bien thi chuong trinh tu hieu la false
    private Rigidbody2D rb;
    private Animator anim;
    private float xInput;
    private float yInput;
    private bool canControl = false;
    private CapsuleCollider2D cd;
    private float defaultGravity;
    [SerializeField] private Difficult gameDifficulty;
    private GameManager gameManager;
    [Header("Movement")]
    [SerializeField] private float speed;

    [Header("Jumping")]
    [SerializeField] private float jumpForce;
    public bool canDoubleJump;
    [SerializeField] private float jumpForceDoubleJump;

    [Header("Buffer Jump & Coyote")]
    [SerializeField] private float bufferJumpWindow = 0.25f;
    private float bufferJumpActive=-1;
    [SerializeField] private float coyoteJumpWindow = 0.25f;
    private float coyoteJumpActive = -1;

    [Header("Knockback")]
    [SerializeField] private float knockBackDuration = 1;
    [SerializeField] private Vector2 knockbackPower;
    private bool isKnocked;
    private PlayerHealth playerHeal;

    [Header("Collision info")]
    [SerializeField] private float wallCheckDistance;
    private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isWallDetected;
    private bool isAirBorn;
    [SerializeField] private float wallJumpDuration = .6f;
    [SerializeField] private Vector2 wallJumpForce;
    private bool isWallJumping;
    [Space]
    [SerializeField] private LayerMask whatEnemy;
    [SerializeField] private Transform enemyCheck;
    [SerializeField] private float enemyCheckRadius;

    [Header("Flip")]
    private bool facingRight = true;
    private int facingDirection = 1;

    [Header("VFX")]
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private AnimatorOverrideController[] animators;
    [SerializeField] private int skinId;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        cd=GetComponent<CapsuleCollider2D>();
        playerHeal = GetComponent<PlayerHealth>();
    }
    private void Start()
    {
        gameManager = GameManager.instance;
        defaultGravity = rb.gravityScale;
        UpdateGameDiff();
        RespwanFinish(false);
        UpdateSkin();
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateAirbornStatus();
        HandleEnemyDetected();
        if (!canControl)
        {
            HandleColision();
            AnimationController();
            return;
        }
        if (isKnocked)
        {
            return;
        }
        CheckInput();
        HandleWallSlide();
        Move();
        FlipController();
        HandleColision();
        AnimationController();
        
    }
    [ContextMenu("Update Difficult")]
    private void UpdateGameDiff()
    {

        DifficultManager difficultManager = DifficultManager.instance;
        if (difficultManager != null)
        {
            gameDifficulty = difficultManager.currentDifficult;
        }
    }
    public void UpdateSkin() {
        SkinManager skinManager = SkinManager.instance;
        if (skinManager == null)
            return;
        anim.runtimeAnimatorController = animators[skinManager.chooseSkinId];
    }
    public void RespwanFinish(bool finsish)
    {
        if (finsish)
        {
            rb.gravityScale = defaultGravity;
            canControl = true;
            cd.enabled= true;
            AudioManager.instance.PlaySFX(11);
        }
        else
        {
            rb.gravityScale = 0;
            canControl = false;
            cd.enabled= false;
        }
    }
    private void UpdateAirbornStatus() {
        if (isGrounded && isAirBorn)
        {
            HandleLanding(); //Tiep dat
        }
        else if (!isGrounded && !isAirBorn)
        {
            BecomeAir();//vua roi khoi mat dat
        }
    }
    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput =Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Jump"))
        { 
            JumpController();
            RequestBufferJump();
        }
    }

    private void BecomeAir()
    {
        isAirBorn = true;
        if (rb.velocity.y < 0)
        {
            ActiveCoyoteJump();
        }
    }

    private void HandleLanding() {
        isAirBorn = false;
        canDoubleJump = true;
        AttempBufferJump();
        if (Mathf.Abs(xInput) < 0.01f)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
    }
    private void Jump()
    {
        AudioManager.instance.PlaySFX(3);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    private void DoubleJump()
    {   
        isWallJumping=false;
        canDoubleJump = false;
        rb.velocity = new Vector2(rb.velocity.x, jumpForceDoubleJump);
    }
    private void WallJump()
    {
        AudioManager.instance.PlaySFX(12);
        canDoubleJump = true;
        rb.velocity = new Vector2(wallJumpForce.x * -facingDirection, wallJumpForce.y);
        Flip();
        StopAllCoroutines();
        StartCoroutine(WallJumpCourotine());
    }
    #region Buffer Jump & Coyote Jump
    private void RequestBufferJump()
    {
        if (isAirBorn)
            bufferJumpActive = Time.time;
    }
    private void AttempBufferJump()
    {
        if (Time.time < bufferJumpActive + bufferJumpWindow)
        {
            bufferJumpActive = Time.time - 1;
            Jump(); 
        }
    }
    private void ActiveCoyoteJump() => coyoteJumpActive = Time.time;
    private void CancelCoyoteJump() => coyoteJumpActive = Time.time - 1;
    #endregion
    private IEnumerator WallJumpCourotine()
    {
        isWallJumping = true;
        yield return new WaitForSeconds(wallJumpDuration);
        isWallJumping = false;
    }
    private void JumpController()
    {
        bool coyoteJump= Time.time <coyoteJumpActive+coyoteJumpWindow;
        if (isGrounded || coyoteJump)
        {
            if (coyoteJump)
            {
                Debug.Log("Coyote Jump");
            }
            Jump();
        }
        else if (isWallDetected && !isGrounded)
        {
            WallJump();
        }
        else if (canDoubleJump)
        {
            DoubleJump();
        }
        CancelCoyoteJump();
    }
 
    public void Knock(float sourceDamageXPosition,int dameamount)
    {
        float knockbackDir = 1;
        if (transform.position.x < sourceDamageXPosition) //Bi sat thuong tu ben phai
        {
            knockbackDir = -1;
        }
        if (isKnocked)
        {
            return;
        }
        AudioManager.instance.PlaySFX(9);
        CameraManager.instance.ShakeCamera(knockbackDir);
        StartCoroutine(KnockCourotine());
        //rb.velocity = new Vector2(knockbackPower.x * -facingDirection, knockbackPower.y);
        rb.velocity = new Vector2(knockbackPower.x * knockbackDir, knockbackPower.y);
        if (playerHeal != null)
        {
            switch (gameDifficulty)
            {
                case Difficult.Easy:
                case Difficult.Normal:
                    playerHeal.LoseHeart(dameamount);
                    if (playerHeal.IsEmpty())
                    {
                        Die();
                    }
                    break;

                case Difficult.Hard:
                    Die();
                    break;
            }
        }
    }
    public void Die()
    {
        AudioManager.instance.PlaySFX(0);
        GameObject newdeathVFX = Instantiate(deathVFX, transform.position, Quaternion.identity);
        switch (gameDifficulty)
        {
            case Difficult.Easy:
                PlayerManager.instance.RespawnPlayer();
                break;
            case Difficult.Normal:
            case Difficult.Hard:
                gameManager.RestartLevel();
                break;
        }
        Destroy(gameObject);
       
    }
    private IEnumerator KnockCourotine() {
        isKnocked = true;
        anim.SetBool("Knockback",true);
        yield return new WaitForSeconds(knockBackDuration);
        isKnocked = false;
        anim.SetBool("Knockback", false);
    }
    private void HandleColision() {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
    }
    private void AnimationController()
    {
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isGround", isGrounded);
        anim.SetBool("isWallDetected", isWallDetected);
    }
    private void Move()
    {
     
        if (isWallDetected || isWallJumping) return;
        rb.velocity = new Vector2(xInput * speed, rb.velocity.y);
    }
    private void Flip()
    {
        facingRight = !facingRight;
        facingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }
    private void FlipController()
    {
        if (xInput > 0 && !facingRight||xInput <0 && facingRight)
        {
            Flip();
        }
        
    }
    private void HandleWallSlide()
    {
        bool canWallSlide = isWallDetected && rb.velocity.y < 0;
        float yControl = yInput<0 ? 1:0.5f;
        if (canWallSlide == false)
        {
            return;
        }
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * yControl);
      
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadius);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (facingDirection * wallCheckDistance), transform.position.y));
    }
    private void HandleEnemyDetected()
    {
        if(rb.velocity.y >= 0 || isKnocked)
        {
            return;
        }
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemyCheck.position, enemyCheckRadius, whatEnemy);
        foreach (var enemy in colliders)
        {
            Enemy newEnemy= enemy.GetComponent<Enemy>();
            if (newEnemy != null)
            {
                AudioManager.instance.PlaySFX(1);
                newEnemy.Die();
                Jump();
            }
        }
    }
    public void Push(Vector2 dir,float duration=0)
    {
       StartCoroutine(PushCourotine(dir, duration));
    }
    private IEnumerator PushCourotine(Vector2 dir, float duration)
    {
        canControl = false;
        rb.velocity = Vector2.zero;
        rb.AddForce(dir, ForceMode2D.Impulse);
        yield return new WaitForSeconds(duration);
        canControl = true;
    }
}
