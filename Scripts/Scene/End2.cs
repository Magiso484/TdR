using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class End2 : MonoBehaviour
{
    private AudioSource audios;
    public PlayerInfo playerInfo;
    public SpiderInfo spiderInfo;

    // Start is called before the first frame update
    void Start()
    {
        audios = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            playerInfo.canWallJump = false;
            playerInfo.canDoubleJump = false;
            playerInfo.canMoveBox = false;
            playerInfo.canTp = false;
            playerInfo.hasKey = false;
            playerInfo.doorHasBeenOpen = false;

            spiderInfo.deafetedLaverint3 = false;
            spiderInfo.deafetedLaverint4 = false;
            spiderInfo.deafetedLaverint8 = false;
            spiderInfo.deafetedSpiderBoss = false;

            SceneManager.LoadScene("MainMenu");
        }
    }
}
