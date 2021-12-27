using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nuclearRod : MonoBehaviour
{

    private float gaugeCount = 0;
    public GameObject gauge;
    public GameObject gaugeNeedle;
    public Image screenEffect;
    [System.NonSerialized]
    public GameObject player;
    private GameMaster gm;

    public AudioSource geigerSound;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject.GetComponentInChildren<nuclearRod>() == null)
        {
            transform.parent = collision.gameObject.transform;
            player = collision.gameObject;
            disableComponents();
            gauge.SetActive(true);
            gaugeNeedle.SetActive(true);
            if (player.name == "player1")
            {
                transform.localPosition = new Vector2(0, -0.1f);
                gm.lastCheckPointPos = new Vector2(329.87f, 24.07f); //spawnpoint of right nuclear rod
                gm.checkPointReached = true;
                
                InvokeRepeating("nuclearDecay", 0.5f, 0.07f);
            }
            else if (player.name == "submarine")
            {
                giveToSubmarine();
            }
        }
    }

    void disableComponents()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<floatAboveGround>().enabled = false;
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    void nuclearDecay()
    {
        gaugeCount += 0.105f;
        if (!geigerSound.isPlaying)
        {
            geigerSound.Play();
        }
//        Debug.Log(gaugeCount);
        if (gaugeCount >= 100)
        {
            CancelInvoke("nuclearDecay");
            transform.parent = null;
            if (!player.GetComponent<PlayerHealth>().isDying)
            {
                StartCoroutine(player.GetComponent<PlayerHealth>().Die());
            }
        }
        gaugeNeedle.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -1 * gaugeCount * (180 / 100.0f)); //replace "x" in (180 / x) with the maximum gaugeCount value before mario dies. also replace the if statement above
        screenEffect.color = new Color(screenEffect.color.r, screenEffect.color.g, screenEffect.color.b, (gaugeCount / 100f) * (100f /255) ); 
        

    }

    public void giveToSubmarine()
    {
        transform.localPosition = new Vector2(-0.0294f, -0.0487f);
        transform.localEulerAngles = new Vector3(0, 0, 90);
        transform.localScale = new Vector2(0.5f, 0.5f);
        player.GetComponent<Submarine>().vel = new Vector2(11, 11);
        gm.subSpawnPos = new Vector2(3.966293f, -41.28367f);
        InvokeRepeating("nuclearDecay", 0.5f, 0.07f);
    }
}
