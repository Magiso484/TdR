using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField]
    private LevelConnection connection;

    [SerializeField]
    private string targetSceneName;

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    public GameObject player;

    private void Start()
    {
        if (connection == LevelConnection.ActiveConnection)
        {
            player.transform.position = spawnPoint.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si el jugador toca el teletransportador
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            print("yes");
            LevelConnection.ActiveConnection = connection;
            SceneManager.LoadScene(targetSceneName);
        }
    }
}
