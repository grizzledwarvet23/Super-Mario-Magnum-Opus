using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SledgeBossManager : MonoBehaviour
{
    public GameObject[] gas;
    public GameObject enemies;
    public GameObject enemies2;

    public Image radiationEffect;
    public GameObject gauge;
    public RectTransform gaugeNeedle;
    private Color ogColor;
    private int count = 0;

    private GameObject player;

    public AudioSource jumpSound;
    public HammerBro child;

    public GameObject mushroom;
    public GameObject mushroom2;

    private bool changedOnce = true;
    private float startPosX;

    public GameObject healthbar;
    public AudioSource bossMusic;

    public AudioSource laugh;

    private void Start()
    {
        startPosX = transform.position.x;
        ogColor = radiationEffect.color;
        player = GameObject.Find("player1");
    }
    public void startFirstStage()
    {
        foreach (GameObject obj in gas)
        {
            obj.SetActive(true);
            obj.GetComponent<AudioSource>().Play();
        }
        InvokeRepeating("gasEffect", 0.5f, 0.3f);
        gauge.SetActive(true);
        gaugeNeedle.gameObject.SetActive(true);
        enemies.SetActive(true);
        mushroom.SetActive(true);

    }

    public void startSecondStage()
    {
        count = 0;
        foreach (GameObject obj in gas)
        {
            obj.GetComponent<ParticleSystem>().Play();
            obj.GetComponent<AudioSource>().Play();
        }
        radiationEffect.color = new Color(radiationEffect.color.r, radiationEffect.color.g, radiationEffect.color.b, 0);
        InvokeRepeating("gasEffect", 0.5f, 0.2f);
        gauge.SetActive(true);
        gaugeNeedle.gameObject.SetActive(true);
        enemies2.SetActive(true);
        mushroom2.SetActive(true);

    }

    void gasEffect()
    {
        GameObject enemies;
        if (this.enemies.activeSelf)
        {
            enemies = this.enemies;
        }
        else 
        {
            enemies = enemies2;
        }
        //     Debug.Log("count " + enemies.transform.childCount);
        if (enemies.transform.childCount == 0)
        {
            enemies.SetActive(false);
            gauge.SetActive(false);
            gaugeNeedle.gameObject.SetActive(false);
            CancelInvoke("gasEffect");
            radiationEffect.color = ogColor;
            Debug.Log(ogColor);
            foreach (GameObject obj in gas)
            {
                obj.GetComponent<ParticleSystem>().Stop();
                obj.GetComponent<AudioSource>().Stop();
            }
            gameObject.GetComponent<Animator>().Play("sledge_return");
        }
        else
        {
            count++;
            // Debug.Log(count);
            radiationEffect.color = new Color(ogColor.r, ogColor.g, ogColor.b, (count / 100.0f) * (10 / 17f));
            gaugeNeedle.eulerAngles = new Vector3(0, 0, -1 * (count / 100f) * 180);

            if (count >= 100)
            {
                CancelInvoke("gasEffect");
                StartCoroutine(player.GetComponent<PlayerHealth>().Die());
            }
        }
    }
    void playLaugh()
    {
        laugh.Play();
    }
    void showHealthbar()
    {
        healthbar.SetActive(true);
    }
    void unlockPlayer()
    {
        player.GetComponent<PlayerHealth>().unlockControl();
    }

    void fadeIntoMusic()
    {
        StartCoroutine(SoundControl.startFade(bossMusic, 3f, 0.332f));
    }
    void flipChild()
    {
        child.flip();
    }

    void resetChild()
    {
        //GameObject child = transform.GetChild(0).gameObject;
        
        child.gameObject.transform.localPosition = new Vector3(0, 0, child.transform.localPosition.z);

        //child.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        if (!child.facingRight && gameObject.GetComponent<Animator>().GetBool("jumpFromLeft"))
        {
            child.flip();
        }
        else if (child.facingRight && !gameObject.GetComponent<Animator>().GetBool("jumpFromLeft"))
        {
            child.flip();
        }

        changedOnce = false;

    }

    void applyRootMotion()
    {
       if (!changedOnce)
        {
            
            if (gameObject.GetComponent<Animator>().GetBool("jumpFromLeft"))
            {
//                Debug.Log(transform.position.x);
                child.gameObject.transform.localPosition = new Vector3(1022.54f - startPosX, child.transform.localPosition.y, child.transform.localPosition.z);
//                Debug.Log("first");
            }
            else
            {

                child.gameObject.transform.localPosition = new Vector3(978.9f - startPosX, child.transform.localPosition.y, child.transform.localPosition.z);
 //               Debug.Log("second");
            }
            changedOnce = true;
        }
        
    }


    void setDynamic()
    {
        child.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }

    void setChildAttacking()
    {
        child.attacking = false;
        child.lastTimeJumped = Time.timeSinceLevelLoad;
    }



    void playJump()
    {
        jumpSound.Play();    
    }


}
