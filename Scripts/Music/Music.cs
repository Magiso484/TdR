using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    public AudioSource menu;
    public AudioSource town;
    public AudioSource left;
    public AudioSource lava;
    public AudioSource boss;
    public AudioSource end;

    private AudioSource[] allAudioSources;

    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        StopAllAudio();
    }

    void Update()
    {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        if (sceneName == "Town" && !town.isPlaying)
        {
            StopAllAudio();
            town.Play();
        }
        else if (sceneName == "MainMenu" && !menu.isPlaying)
        {
            StopAllAudio();
            menu.Play();
        }
        else if (sceneName == "Connection Scene 1" && !left.isPlaying)
        {
            StopAllAudio();
            left.Play();
        }
        else if ((sceneName == "Lava1" || sceneName == "Lava3") && !lava.isPlaying)
        {
            StopAllAudio();
            lava.Play();
        }
        else if (sceneName == "BossRoom1" && !boss.isPlaying)
        {
            StopAllAudio();
            boss.Play();
        }
        else if (sceneName == "Scari")
        {
            StopAllAudio();
            boss.Play();
        }
        else if (sceneName == "End" && !end.isPlaying)
        {
            StopAllAudio();
            end.Play();
        }
        else if (sceneName == "End2")
        {
            Destroy(gameObject);
        }
        else if (sceneName == "Credits")
        {
            Destroy(gameObject);
        }

    }
    void StopAllAudio()
    {
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }

    //void PlayMusic(string name)
    //{
    //    if (sceneName == "Town")
    //    {
    //        town.Play();
    //    }
    //}
}
