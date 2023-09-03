using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private GameObject ProjectileLauncher;

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

            // Set the velocity of the projectile in the direction of the ProjectileLauncher
            rb.velocity = new Vector2(projectileDirection * 10f, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprovem la colisió, per veure si és amb la capa adecuada
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall") || collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Box"))
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        // Comprovem la colisió, per veure si és amb la capa adecuada
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Destroy(gameObject);
        }
    }
}
