using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceManager : MonoBehaviour
{
    public GameObject[] waves;
    private int index = 0;

    public float timeBeforeStart;

    public Animator whiteFade;
    public Spaceship ship;
    public AudioSource shipSound;
    public Collider2D border;
    public GameObject deadzone;

    GameMaster gm;

    public GameObject originalCam;
    public GameObject targetCam;
    public GameObject player;
    public GameObject background;
    public GameObject[] enemies;
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        if (gm.loadPlayer)
        {
            player.SetActive(true);
            ship.gameObject.SetActive(false);
            background.SetActive(true);
            foreach(GameObject obj in enemies)
            {
                obj.SetActive(true);
            }
        }
        else
        {
            StartCoroutine(startWaves(timeBeforeStart));
        }
        //spawnNextWave();
    }

    IEnumerator startWaves(float t)
    {
        yield return new WaitForSeconds(t);
        spawnNextWave();
    }

    private void Update()
    {
        if(index < waves.Length && waves[index].transform.childCount == 0)
        {
            index++;
            Invoke("spawnNextWave", 1.5f);
        }
    }

    void spawnNextWave()
    {
        if (index < waves.Length)
        {
            waves[index].SetActive(true);
        }
        else
        {
            border.enabled = false;
            ship.enabled = false;
            deadzone.SetActive(false);
            StartCoroutine(accelerate());
            StartCoroutine(fade());
            //whiteFade.Play("whiteFadeIn");
        }
    }

    IEnumerator accelerate()
    {
        yield return new WaitForSeconds(0);
        Rigidbody2D rb = ship.gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;     
        rb.gravityScale = -4;
        
    }
    IEnumerator fade()
    {
        yield return new WaitForSeconds(1f);
        whiteFade.Play("whiteFadeIn");
        StartCoroutine(SoundControl.startFade(shipSound, 2f, 0));
        yield return new WaitForSeconds(3f);
        whiteFade.Play("whiteFadeOut");
        player.SetActive(true);
        gm.loadPlayer = true;
        ship.gameObject.SetActive(false);

        originalCam.SetActive(false);
        targetCam.SetActive(true);
        background.SetActive(true);

        foreach(GameObject obj in enemies)
        {
            obj.SetActive(true);
        }

        gm.lastCheckPointPos = player.transform.position;
        gm.ogCam = originalCam.name;
        gm.tgCam = targetCam.name;




    }

}
