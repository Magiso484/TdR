using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private GameObject ProjectileLauncher;
    [SerializeField]
    private bool move;

    // Start is called before the first frame update
    void Start()
    {
        // Find the ProjectileLauncher GameObject
        ProjectileLauncher = GameObject.Find("Projectile Launcher");

        // Check if the ProjectileLauncher was found
        if (ProjectileLauncher != null)
        {
            // Get the Transform component of the ProjectileLauncher
            Transform PJTrans = ProjectileLauncher.GetComponent<Transform>();

            // Get the direction in which the ProjectileLauncher is facing
            float projectileDirection = PJTrans.localScale.x;

            // Get the Rigidbody2D and BoxCollider2D components of the projectile
            rb = GetComponent<Rigidbody2D>();
            coll = GetComponent<BoxCollider2D>();

            move = true;
        }
    }

    void Update()
    {
        if(move)
        {
            rb.velocity = new Vector2(0, -5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprovem la colisi�, per veure si �s amb la capa adecuada
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Comprovem la colisi�, per veure si �s amb la capa adecuada
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Destroy(gameObject);
        }
    }
}
