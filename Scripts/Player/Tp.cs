using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tp : MonoBehaviour
{
    [SerializeField]
    private PlayerInfo playerInfo;

    private float tpTime;
    private float holdTpTime = 0.5f;

    private void Start()
    {
        tpTime = holdTpTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            tpTime -= Time.deltaTime;
        }
        if (tpTime <= 0 && playerInfo.canTp)
        {
            SceneManager.LoadScene("Town");
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            tpTime = holdTpTime;
        }
    }
}
