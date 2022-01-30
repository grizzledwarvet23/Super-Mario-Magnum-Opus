using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Submarine : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    public Vector2 vel;
   // public Vector2 spawnPos;

    public GameObject snowEffector;
//    GameObject player;
    public CinemachineVirtualCamera vcam;
    public GameObject bulletPrefab;
    public Transform firePoint;
    private bool facingRight;

    private float timeLastFired;
    public float shotDelay;

    public PlayerHealth healthScript;
    public CoinCollection coinScript;

    [System.NonSerialized]
    public GameMaster gm;

    public GameObject player;

    public AudioSource music;
    private float originalVol;

    public bool introDone;
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        timeLastFired = -3;
        gameObject.GetComponent<Animator>().Play("submarine_idle");
        healthScript.health = player.GetComponent<PlayerHealth>().health;
        
        
    }
    void OnEnable()
    {
        music.volume = 0.33f;
//        Debug.Log("hey");
        gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        gm.loadSubmarine = true;
        
        player.GetComponent<PlayerHealth>().Invoke("lockControl", 0.1f);
     //   healthScript.health = player.GetComponent<PlayerHealth>().health;
        snowEffector.SetActive(false);
        vcam.Follow = gameObject.transform;
        
        if (gm.subSpawnPos != Vector2.zero)
        {
            transform.localPosition = gm.subSpawnPos;
        }
        if(facingRight)
        {
            flip();
        }
        

    }

    void OnDisable()
    {
       // gm.loadSubmarine = false;   
        if (player != null)
        {
            music.volume = 0.61f;
            player.GetComponent<PlayerHealth>().unlockControl();
            player.GetComponent<PlayerHealth>().health = healthScript.health;
        }
    }

    private void Update()
    {
        if (introDone)
        {
            if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.O)) && Time.time >= timeLastFired + shotDelay)
            {
                timeLastFired = Time.time;
                fire();
            }
        }
    }




    void FixedUpdate()
    {
        if (introDone)
        {
            float dir = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(dir * vel.x, Input.GetAxisRaw("Vertical") * vel.y);
            if (dir < 0 && facingRight || dir > 0 && !facingRight)
            {
                flip();
            }
        }
    }

    void fire()
    {
        GameObject torpedo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    void flip()
    {
        
        facingRight = !facingRight;
//        Debug.Log(transform.rotation);
        transform.Rotate(0f, 180f, 0f);
        
        // transform.eulerAngles = new Vector3(0f, 180f - (Mathf.Sign(transform.eulerAngles.y) * 180f), 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Bullet(Clone)" || collision.gameObject.tag == "Projectile")
        {
            healthScript.takeDamage(collision.GetComponent<Bullet>().damage);
        }
        else if (collision.gameObject.tag == "Coin")
        {
            coinScript.coinCollect(collision);
        }
    }

    public void transferRod()
    {
        if (gameObject.GetComponentInChildren<nuclearRod>() != null)
        {
            nuclearRod nr = gameObject.GetComponentInChildren<nuclearRod>();
            nr.player = player;
            nr.gameObject.transform.parent = player.transform;
            nr.transform.localPosition = new Vector2(0, -0.1f);
            nr.transform.localScale = new Vector2(0.808705f, 0.8513883f);
            nr.transform.localEulerAngles = Vector3.zero;
            nr.gameObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
          


        }
    }

    public void receiveRod(nuclearRod nr)
    {
        nr.player = gameObject;
        nr.gameObject.transform.parent = gameObject.transform;
        nr.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        nr.giveToSubmarine();
    }
}
