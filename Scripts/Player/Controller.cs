using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Movement
    private float movementAcceleration;
    private float maxMoveSpeed;
    private float movementDeceleration;
    private float fallingGravity = 1.2f;
    private float maxFallSpeed = 20f;
    private float moveInput;

    // Jumping
    private float jumpForce = 10f;
    private float doubleJumpForce = 20f;    
    private float jumpStartTime;
    private float jumpTime;
    private float jumpBufferTime = 0.1f;
    private float jumpBufferCounter;
    private float maxJumpSpeed = 10f;
    private float maxAltitude = 1.5f;
    private bool doubleJump;

    // WallJumping/Sliding
    public bool isWallSliding;
    private float wallSlidingSpeed;
    public Transform wallCheck;
    public LayerMask wallLayer;
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(6f, 12f);
    private float maxYSpeed = 15;


    // Coyote Jump
    private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    // Physics
    private Rigidbody2D rb;
    private Collider2D coll;

    // Position
    private Vector3 positionOnGround;

    // State
    private bool isJumping = false;
    private bool isCrouching = false;
    private bool isGrounded;

    public Transform groundCheck;
    private float checkRadius = 0.15f;
    public LayerMask whatIsGround;
    public Transform ceilingCheck;

    // Animation
    private Animator anim;

    // Move Box
    public LayerMask boxLayer;
    public bool canMoveBox;

    // Damage
    public LayerMask enemyLayer;
    private float damageTimeCounter = 1f;
    private float damageTimer;
    private bool isDamaged = false;
    private bool damaged;
    public bool isTouchingEnemy;

    // CameraShake

    private float shakeIntensity = 2.5f;
    private float shakeFrequency = 2.5f;
    private float shakeTime = 0.5f;

    // Spider
    public List <SpiderEnemy> spiders;
    public int deafetedSpidersOnScene;
    public int lastSpiderKilled;
    public bool thereAreSpiders;

    // Particles
    private ParticleSystem dust;

    // Scriptable Objects
    [SerializeField]
    public PlayerInfo playerInfo;

    // Sound
    private AudioSource audioJump;
    public AudioSource audioDamage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        dust = GetComponentInChildren<ParticleSystem>();
        audioJump = GetComponent<AudioSource>();

        movementAcceleration = 15f;
        movementDeceleration = 5f;
        maxFallSpeed = 20f;
        jumpForce = 10f;
        doubleJumpForce = 20f;
        wallSlidingSpeed = 1.5f;
        jumpStartTime = 0.2f;

        lastSpiderKilled = 0;
        deafetedSpidersOnScene = 0;

        playerInfo.canCrouch = true;
}

    void Update()
    {
        //Jump Buffering
        if (Input.GetKeyDown(KeyCode.Space)){
            jumpBufferCounter = jumpBufferTime;
        }
        else{
            jumpBufferCounter -= Time.deltaTime;
        }

        // CoyoteTimer
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            rb.gravityScale = 1;
            doubleJump = true;
            positionOnGround = transform.position;;
            anim.SetBool("isJumping", false);
            anim.SetBool("isDoubleJumping", false);
            anim.SetBool("isGrounded", true);
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            anim.SetBool("isJumping", true);
            anim.SetBool("isGrounded", false);
        }

        WallSlide();
        WallJump();
        isBoxed();
        damaged = isTouchingEnemy;

        // Handle Jumping
        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
        else if (Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }

        if (Input.GetButton("Jump") && isJumping && rb.velocity.y >= 0)
        {
            if (jumpTime > 0)
            {
                Jump(jumpForce);
                jumpTime -= Time.deltaTime;
            }
            else
            {
                isJumping = true;
            }
        }

        // Handle Double Jumping
        else if (rb.velocity.y < -7.5f && isJumping && doubleJump && playerInfo.canDoubleJump)
        {
            DoubleJump(doubleJumpForce);
            doubleJump = false;
            anim.SetBool("isDoubleJumping", true);
        }

        // Handle crouching
        if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) && playerInfo.canCrouch)
        {
            isCrouching = true;
            coll.enabled = false;
        }
        else if (!cantStand())
        {
            isCrouching = false;
            coll.enabled = true;
        }

        // Reduïm la velocitat del jugador al ajupir-se
        if (!isCrouching)
        {
            maxMoveSpeed = 7.5f;
            anim.SetBool("isCrouching", false);
        }
        else if(isCrouching && !isJumping)
        {
            maxMoveSpeed = 3.75f;
            anim.SetBool("isCrouching", true);
        }

        // Handle Animations
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (Mathf.Abs(moveInput) > 0 && !isDamaged)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }
        if (isBoxed() && moveInput!= 0)
        {
            anim.SetBool("isPushing", true);
        }
        else
        {
            anim.SetBool("isPushing", false);
        }

        // Damage
        if (isTouchingEnemy && !isDamaged){
            Damage();
            isDamaged = true;
            damageTimer = damageTimeCounter;
        }

        damageTimer -= Time.deltaTime;
        if (damageTimer <= 0)
        {
            isDamaged = false;
        }

        // Spider Kill
        if (thereAreSpiders)
        {
            if (spiders[lastSpiderKilled].isDead)
            {
                SpiderKill();
                spiders[lastSpiderKilled].isDead = false;
            }
        }
    }

    void FixedUpdate()
    {     
        // Comprovem si toca el terra o una caixa
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        if (!isGrounded)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, boxLayer);
        }

        // Gravity controls
        if (rb.velocity.y < -2f)
        {
            // Limitem la gravetat
            if (rb.gravityScale < 2.5f)
            {
                rb.gravityScale = rb.gravityScale * fallingGravity;
            }
            // Limitem la velocitat de caiguda
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
        }

        // Handle movement
        float moveInput = Input.GetAxisRaw("Horizontal");

        if (!isWallJumping && !isDamaged)
        {
            Run();
            ApplyDeceleration();
        }


        // Flip the sprite based on movement direction
        if (!isWallJumping)
        {
            if (moveInput != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(moveInput)*2, 2, 2);
            }
        }

        // Handle jumping
        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f)
            {
                Jump(jumpForce);
                audioJump.Play();
                jumpTime = jumpStartTime;
                jumpBufferCounter = 0;
            }
        }
    }

    void Jump(float force)
    {
        // SI SOBRE PASSA L'ALTURA MAX NO SALTA
        if ((Mathf.Abs(positionOnGround.y - transform.position.y) < maxAltitude) && !cantStand())
        {
            rb.velocity = new Vector2(rb.velocity.x, force);

            // Si saltem mentre ens movem produïm particules
            if (Mathf.Abs(rb.velocity.x) > 5f)
            {
                ProduceDust();
            }

            if (rb.velocity.y > maxJumpSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, maxJumpSpeed));
            }
        }
    }

    void DoubleJump(float force)
    {
        rb.velocity = new Vector2(rb.velocity.x, force);
        audioJump.Play();
    }

    void Run()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        //Acceleració
        rb.AddForce(new Vector2(moveInput, 0f) * movementAcceleration);
        //Max velocity
        if (Mathf.Abs(rb.velocity.x) > maxMoveSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxMoveSpeed, rb.velocity.y);
        }
    }

    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    public bool isBoxed()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, boxLayer);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            isTouchingEnemy = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            isTouchingEnemy = false;
        }
    }

    void WallSlide()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");

        if(isWalled() && !isGrounded && moveInput != 0f && playerInfo.canWallJump)
        {
            isWallSliding = true;
            anim.SetBool("isWallSliding", true);
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
            anim.SetBool("isWallSliding", false);
        }
    }

    void WallJump()
    {
        if(isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x / 2;
            wallJumpingCounter = wallJumpingTime;
            rb.gravityScale = 1;
            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f && playerInfo.canWallJump)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            audioJump.Play();
            wallJumpingCounter = 0f;

            if (transform.localScale.x/2 != wallJumpingDirection)
            {
                Vector3 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    void StopWallJumping()
    {
        isWallJumping = false;
    }

    void ApplyDeceleration()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(moveInput) < 0.4f)
        {
            rb.drag = movementDeceleration;
        }
        else
        {
            rb.drag = 0f;
        }
    }

    void Damage()
    {
        // Reiniciem la velocitat i la fricció del jugador per evitar bugs
        rb.drag = movementDeceleration;
        rb.velocity = new Vector2(0,0);

        // Apliquem la força d'impuls, depenent de cap on miri el personatge serà impulsat cap a un costat o un altre
        rb.AddForce(new Vector2(15f * -Mathf.Sign(transform.localScale.x), 10f), ForceMode2D.Impulse);

        // Apliquem el CameraShake
        CameraShake.Instance.Shake(shakeIntensity, shakeFrequency, shakeTime);

        // Play Sound
        audioDamage.Play();
    }

    void SpiderKill()
    {
        // Reiniciem la velocitat i la fricció del jugador per evitar bugs
        rb.drag = movementDeceleration;
        rb.velocity = new Vector2(0, 0);

        // Apliquem la força d'impuls
        rb.AddForce(new Vector2(0f, 25f), ForceMode2D.Impulse);
        // Sumem 1 a les aranyes assesinades
        deafetedSpidersOnScene ++;
    }

    void ProduceDust()
    {
        dust.Play();
    }

    private bool cantStand()
    {
        // Comprovem si el jugador té un sostre a sobre que no el permeti aixecar-se
        return Physics2D.OverlapCircle(ceilingCheck.position, 0.25f, whatIsGround);
    }
}