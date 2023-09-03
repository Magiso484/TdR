using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJump : MonoBehaviour
{
    private BoxCollider2D coll;
    public PlayerInfo playerInfo;
    private AudioSource pick;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
        pick = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        pick.Play();
        playerInfo.canWallJump = true;
    }
}
