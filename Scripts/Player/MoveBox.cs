using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBox : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    public PlayerInfo playerInfo;

    private AudioSource audioBox;
    private Transform trans;

    private float inicialFramePos;
    private float framePos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trans = GetComponent<Transform>();
        audioBox = GetComponent<AudioSource>();

        inicialFramePos = trans.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        // Canviem la massa de les caixes quan el personatge té l'habilitat de moure les caixes
        if (playerInfo.canMoveBox)
        {
            rb.mass = 1.25f;
        }
        else
        {
            rb.mass = 100f;
        }
    }
}
