using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLvl : MonoBehaviour
{
    public int indexLvl;

    // Start is called before the first frame update
    void Start()
    {
        indexLvl = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeLevel()
    {
        SceneManager.LoadScene(indexLvl);
        indexLvl ++;
    }
}
