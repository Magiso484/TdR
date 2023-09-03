using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scari : MonoBehaviour
{
    private float countdown = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if(countdown < 0)
        {
            SceneManager.LoadScene("MainMenu");
        }
        countdown -= Time.deltaTime;
    }
}
