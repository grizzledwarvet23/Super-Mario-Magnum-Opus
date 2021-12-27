using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroyOnReload : MonoBehaviour
{
    private static dontDestroyOnReload instance;
    public bool isMusic;
    public AudioSource musicToEnable, musicToDisable;

    public int thisLevel;
    void Awake() // PREVIOUSLY AWAKE
    {
        
   //     Debug.Log(instance);

        if (instance == null)
        {
          //  Debug.Log("yeh");   
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            
           Destroy(gameObject);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
//        Debug.Log(level);
        //for music
        if (isMusic)
        {
            foreach (AudioSource audio in gameObject.GetComponents<AudioSource>())
            {
                if (audio.enabled == true)
                {
                    if (musicToDisable == audio)
                    {
                        audio.enabled = false;
                        musicToEnable.enabled = true;
                        musicToEnable.Play();
                    }
                    else
                    {
                        audio.Play();
                    }
                }
            }
        }
        if(level != thisLevel)//can make a varibale for it later
        {
            Destroy(gameObject);
        }
    }
}
