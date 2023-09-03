using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBoss : MonoBehaviour
{
    // Start
    public bool battleStarted;
    private float distanceToStart = 13f;
    public GameObject blocker;
    
    public SpiderInfo spiderInfo;

    // Movement
    private float movementSpeed = 8.25f;
    private bool hittedLimiter = false;

    // Jumping
    public GameObject playerCheck;
    private float playerCheckRadius = 1f;
    public LayerMask playerLayer;
    private float jumpForce = 15f;
    private float jumpTime = 2f;
    private float jumpTimeCounter;

    // Gravity
    private float fallingGravity = 1.2f;
    private float maxFallSpeed = 15f;

    // Grounded
    private float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    public GameObject groundCheck;

    // Hit Points
    private int vida = 3;
    private float checkRadius = 0.75f;
    private bool isDamaged = false;
    public LayerMask boxLayer;
    private float damageTimer = 0;
    private float inicialDamageTimer = 1f;
    private bool canBeDamaged = true;

    public GameObject PowerUp;
    public GameObject Npc;

    public GameObject boxCheck;
    public GameObject player;
    private Transform trans;
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField]
    private BoxCollider2D coll1;
    [SerializeField]
    private BoxCollider2D coll2;
    private AudioSource damage;

    private float lastFramePos;

    // Start is called before the first frame update
    void Start()
    {
        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        damage = GetComponent<AudioSource>();
        battleStarted = false;
        
        lastFramePos = trans.position.x;
        jumpTimeCounter = 0;

        if(spiderInfo.deafetedSpiderBoss)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Comprovem si el boss i el jugador est�n prou aprop per comen�ar la lluita
        if (distanceToStart > Mathf.Abs(player.transform.position.x - trans.position.x) && !battleStarted)
        {
            battleStarted = true;
        }

        if(battleStarted)
        {
            anim.SetBool("battleStarted", true);

            // Movement
            if (!isDamaged && isGrounded())
            {
                rb.velocity = new Vector2(movementSpeed * (transform.localScale.x / 4), rb.velocity.y);
            }
            else if(isDamaged && isGrounded())
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(15f * trans.localScale.x / 4, rb.velocity.y);
            }

            if(hittedLimiter)
            {
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                hittedLimiter = false;
            }

            // Damage
            if (isHitted() && canBeDamaged)
            {
                damage.Play();
                vida--;
                if(vida > 0)
                {
                    anim.SetBool("isDamaged", true);
                    isDamaged = true;
                    damageTimer = inicialDamageTimer;
                    canBeDamaged = false;                    
                }
                
            }
            if (damageTimer <= 0)
            {
                isDamaged = false;
                anim.SetBool("isDamaged", false);
                canBeDamaged = true;
            }
            else
            {
                damageTimer -= Time.deltaTime;
            }

            // Death
            if(vida <= 0)
            {
                spiderInfo.deafetedSpiderBoss = true;
                print("deafetedSpiderBoss");
                blocker.SetActive(false);
                PowerUp.SetActive(true);
                Npc.SetActive(true);
                Destroy(boxCheck);
                Destroy(playerCheck);
                coll1.enabled = false;
                coll2.enabled = false;
                rb.velocity = new Vector2(0,0);
                rb.gravityScale = 0;
                anim.SetBool("isDead", true);
            }
            else
            {
                blocker.SetActive(true);
                PowerUp.SetActive(false);
                Npc.SetActive(false);
            }

            // Attack / Jump
            if (detectsPlayer() && isGrounded() && jumpTimeCounter <= 0)
            {
                Jump();
                jumpTimeCounter = jumpTime;
            }
            else
            {
                jumpTimeCounter -= Time.deltaTime;
            }

            if (isGrounded() && jumpTimeCounter < 1.5f)
            {
                anim.SetBool("isJumping", false);
            }

            // Gravity Controls
            if (rb.velocity.y <= 0f)
            {
                // Limitem la gravetat
                if (rb.gravityScale < 2.5f)
                {
                    rb.gravityScale = rb.gravityScale * fallingGravity;
                }
                // Limitem la velocitat de caiguda
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed));
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            hittedLimiter = true;
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("EnemyLimiter"))
        {
            hittedLimiter = true;
        }
    }

    private bool isHitted()
    {
        if (boxCheck != null)
        {
            return Physics2D.OverlapCircle(boxCheck.transform.position, checkRadius, boxLayer);
        }
        return false;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(15f * (trans.localScale.x/4), jumpForce);
        anim.SetBool("isJumping", true);
    }

    private bool detectsPlayer()
    {
        if (playerCheck != null)
        {
            return Physics2D.OverlapCircle(playerCheck.transform.position, playerCheckRadius, playerLayer);
        }
        return false;
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.transform.position, groundCheckRadius, groundLayer);
    }
}
