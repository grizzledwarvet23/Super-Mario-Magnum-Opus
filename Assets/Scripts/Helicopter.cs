using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicopter : MonoBehaviour
{
    [System.NonSerialized]
    public bool activated;

    private GameObject player;
    public float triggerBound;
    public Camera cam;

    private GameMaster gm;
    public teleport teleportCheck;

    public GameObject projectile;
    public Transform firePoint;

    public GameObject missile;
    public Transform missilePoint;

    public AudioSource musicSource;
    public AudioClip musicClip;

    [System.NonSerialized]
    public bool descending;

    private bool sweeping;
    [System.NonSerialized]
    public bool left;

    private bool fixYPos;

    private const float y_pos = 10;
    private int attackIteration = 0;

    public AudioSource helicopterSound;
    public AudioSource gunSound;

    [System.NonSerialized]
    public Coroutine attackRoutine;
    [System.NonSerialized]
    public Coroutine moveRoutine;

    public Collider2D mortar; //enabled collider after intro

    // Start is called before the first frame update
    void Start()
    {
        activated = false;
        left = true;
        player = GameObject.Find("player1");
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!activated && player.transform.position.x >= triggerBound)
        {
            activated = true;
            GetComponent<SpriteRenderer>().enabled = true;
            musicSource.clip = musicClip;
            musicSource.volume = 0;

            

            gm.lastCheckPointPos = new Vector2(422, -8.8f);
            gm.checkPointReached = true;
            StartCoroutine(teleportCheck.loadObjects());
            
            helicopterSound.Play();
            transform.position = new Vector2(transform.position.x, 22);
            GetComponent<AICollision>().enabled = true;
            StartCoroutine(intro());
        }
        if (activated)
        {
            if (fixYPos)
            {
                transform.position = new Vector2(transform.position.x, y_pos);
            }
            if (!descending)
            {
                if (!sweeping)
                {
                    if (attackIteration == 2)
                    {
                        attackRoutine = StartCoroutine(dropBombs());
                        if (left)
                        {
                            moveRoutine = StartCoroutine(leftToRight(false, 0.025f));
                        }
                        else
                        {
                            moveRoutine = StartCoroutine(rightToLeft(false, 0.025f));
                        }
                        left = !left;
                    }
                    else
                    {
                        attackRoutine = StartCoroutine(fire());
                        if (left)
                        {
                            moveRoutine = StartCoroutine(leftToRight(true, 0.03f));
                        }
                        else
                        {
                            moveRoutine = StartCoroutine(rightToLeft(true, 0.03f));
                        }
                        left = !left;
                    }
                }
            }
        }
    }

    private IEnumerator fire()
    {
        attackIteration = (attackIteration + 1) % 3;
        yield return new WaitForSeconds(0.2f);

        gunSound.enabled = true;
        gunSound.Play();
        for (int i = 0; i < 150; i++)
        {
            Quaternion rot = firePoint.rotation;
            rot.eulerAngles = new Vector3(rot.eulerAngles.x, rot.eulerAngles.y, Random.Range(rot.eulerAngles.z - 2, rot.eulerAngles.z + 3));


            GameObject bullet = Instantiate(projectile, firePoint.position, rot);
            bullet.GetComponent<Bullet>().Initialize(20, 25);
            yield return new WaitForSeconds(0.01f);
        }
        gunSound.Stop();
    }

    private IEnumerator dropBombs()
    {
        attackIteration = (attackIteration + 1) % 3;
        yield return new WaitForSeconds(0.4f);
        for (int i = 0; i < 8; i++)
        {
            Instantiate(missile, missilePoint.position, missilePoint.rotation);
            yield return new WaitForSeconds(0.25f);
        }

    }

    private IEnumerator intro()
    {
        fixYPos = false;
        descending = true;
        for (int i = 0; i < 80; i++)
        {
            transform.position = Vector2.Lerp(new Vector2(transform.position.x, 22), new Vector2(transform.position.x, y_pos), i / 79.0f);
            yield return new WaitForSeconds(0.03f);
        }
        fixYPos = true;

        

        yield return new WaitForSeconds(2);
        fixYPos = false;
        musicSource.Play();
        StartCoroutine(SoundControl.startFade(musicSource, 2, 0.8f));
        for (int i = 0; i < 60; i++)
        {
            transform.position = Vector2.Lerp(new Vector2(transform.position.x, y_pos), new Vector2(transform.position.x, 22), i / 59.0f);
            yield return new WaitForSeconds(0.03f);
        }

        


        yield return new WaitForSeconds(1.5f);
        fixYPos = true;
        descending = false;
        mortar.enabled = true;
    }
    public IEnumerator leftToRight(bool rotate, float kLoopTime)
    {
        sweeping = true;
        if (rotate)
        {
            transform.localEulerAngles = new Vector3(0, 0, -20);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        for (int i = 0; i < 95; i++)
        {
            transform.localPosition = Vector3.Lerp(new Vector3(-36, transform.localPosition.y, 10), new Vector3(38, transform.localPosition.y, 10), i / 94.0f);
            yield return new WaitForSeconds(kLoopTime);
        }
        yield return new WaitForSeconds(3);
        sweeping = false;
        // transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    public IEnumerator rightToLeft(bool rotate, float kLoopTime)
    {
        sweeping = true;
        if (rotate)
        {
            transform.localEulerAngles = new Vector3(0, 180, -20);
        }
        else
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        for (int i = 0; i < 95; i++)
        {
            transform.localPosition = Vector3.Lerp(new Vector3(38, transform.localPosition.y, 10), new Vector3(-36, transform.localPosition.y, 10), i / 94.0f);
            yield return new WaitForSeconds(kLoopTime);
        }
        yield return new WaitForSeconds(3);
        sweeping = false;

    }

    public bool getSweeping()
    {
        return sweeping;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(new Vector3(triggerBound, -300, 0), new Vector3(triggerBound, 300, 0));
    }
}
