using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour
{
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    private BoxCollider2D coll;
    public GameObject Box;

    void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    private void DestroyObjects(string destroyTag)
    {
        GameObject[] destroyObject;
        destroyObject = GameObject.FindGameObjectsWithTag(destroyTag);
        foreach (GameObject Box in destroyObject)
            Destroy(Box);
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            DestroyObjects("Box");
            Instantiate(Box, spawnPoint1.position, Quaternion.identity);
            Instantiate(Box, spawnPoint2.position, Quaternion.identity);
            Instantiate(Box, spawnPoint3.position, Quaternion.identity);
        }
    }
}
