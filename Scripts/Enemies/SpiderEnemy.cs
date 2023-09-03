using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderEnemy : MonoBehaviour
{
    public Controller controller;
    public int spiderIndex;
    private float velocity = -5f;
    public bool isDead;
    private Transform spider;
    private Collider2D coll;
    private Rigidbody2D rb;
    private Animator anim;
    public Transform playerCheck;
    public GameObject pCheck;
    private float checkRadius = 0.3f;
    public LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        spider = GetComponent<Transform>();
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        //pCheck = GameObject.FindWithTag("PlayerCheck");

        isDead = false;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(velocity * spider.localScale.x, 0);

        if (isKilledByPlayer())
        {
            isDead = true;
            Destroy(pCheck);
            controller.lastSpiderKilled = spiderIndex;
            coll.enabled = false;
            velocity = 0f;
            anim.SetBool("Dead", true);
            //Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("EnemyLimiter"))
        {
            spider.localScale = new Vector3(spider.localScale.x * -1, spider.localScale.y, spider.localScale.z);
        }
    }

    private bool isKilledByPlayer()
    {
        if(playerCheck != null)
        {
            return Physics2D.OverlapCircle(playerCheck.position, checkRadius, playerLayer);
        }
        return false;
    }
}
