using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int enemiesOnScene;

    [SerializeField]
    private GameObject Blocker;
    [SerializeField]
    private SpiderInfo spiderInfo;
    [SerializeField]
    private Controller controller;

    public string scene;


    // Start is called before the first frame update
    void Start()
    {
        if ((scene == spiderInfo.Laverint3) && spiderInfo.deafetedLaverint3)
        {
            Destroy(Blocker);
        }
        else if ((scene == spiderInfo.Laverint4) && spiderInfo.deafetedLaverint4)
        {
            Destroy(Blocker);
        }
        else if ((scene == spiderInfo.Laverint8) && spiderInfo.deafetedLaverint8)
        {
            Destroy(Blocker);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.deafetedSpidersOnScene == enemiesOnScene)
        {
            if (scene == spiderInfo.Laverint3)
            {
                spiderInfo.deafetedLaverint3 = true;
                Destroy(Blocker);
            }
            else if (scene == spiderInfo.Laverint4)
            {
                spiderInfo.deafetedLaverint4 = true;
                Destroy(Blocker);
            }
            else if (scene == spiderInfo.Laverint8)
            {
                spiderInfo.deafetedLaverint8 = true;
                Destroy(Blocker);
            }
        }
    }
}
