using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public GameObject projectile;
    private Transform trans;
    [SerializeField]
    private float spawnTime = 0.75f;
    public bool isVertical;

    // Start is called before the first frame update
    void Start()
    {
        // Spawnejem el projectil amb la funci� generate, amb 0 segons de temps inicial i 0.5 segons d'interval entre spawns
        InvokeRepeating("generate", 0, spawnTime);
        trans = GetComponent<Transform>();
    }
    
    void generate()
    {
        if (!isVertical)
        {
            Instantiate(projectile, trans.position + new Vector3(1 * trans.localScale.x, 0, 0), Quaternion.identity);
        }
        else
        {
            Instantiate(projectile, trans.position + new Vector3(0, 1 * trans.localScale.x, 0), Quaternion.identity);
        }
        
    }
}
