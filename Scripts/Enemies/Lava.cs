using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject player;
    private BoxCollider2D coll;
    private Rigidbody2D rb;
    public StartTrigger startTrigger;
    private Transform trans;

    private float velocity = 1.25f;
    private Vector3 inicialPos;
    private Vector3 spawnPointPos;
    public bool willRise;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();

        inicialPos = transform.position;
        spawnPointPos = spawnPoint.transform.position;
    }

    void Update()
    {
        if (startTrigger.starts)
        {
            rb.velocity = new Vector2(0, velocity);
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (willRise)
            {
                if(inicialPos != null)
                {
                    trans.position = inicialPos;
                }
                startTrigger.starts = false;
            }
            spawnPoint.transform.position = spawnPointPos;
            player.transform.position = spawnPoint.transform.position;
        }
    }
}

