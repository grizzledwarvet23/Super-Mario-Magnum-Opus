using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    [System.NonSerialized]
    public bool isPaused;
    private float defaultTimeScale;

    GameObject player;

    void Start()
    {
        player = GameObject.Find("player1");
        defaultTimeScale = Time.timeScale;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    void Pause()
    {
        if (player != null)
        {
            player.GetComponent<Move2D>().enabled = false;
            player.GetComponent<Weapon>().enabled = false;
        }
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
        isPaused = true;
    }

    void Resume()
    {
        if (player != null && !player.GetComponent<PlayerHealth>().locked)
        {
            player.GetComponent<Move2D>().enabled = true;
            player.GetComponent<Weapon>().enabled = true;
        }

        pauseMenuUI.SetActive(false);
        Time.timeScale = defaultTimeScale;
        AudioListener.pause = false;
        isPaused = false;
    }




}
