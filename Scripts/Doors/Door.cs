using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public LayerMask playerLayer;
    public Transform playerCheck;
    private float radi = 0.5f;

    public PlayerInfo playerInfo;


    // Start is called before the first frame update
    void Start()
    {
        if (playerInfo.doorHasBeenOpen)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OpenDoor() && playerInfo.hasKey)
        {
            Destroy(gameObject);
            playerInfo.doorHasBeenOpen = true;
        }
    }

    private bool OpenDoor()
    {
        if (playerCheck != null)
        {
            return Physics2D.OverlapCircle(playerCheck.transform.position, radi, playerLayer);
        }
        return false;
    }
}
