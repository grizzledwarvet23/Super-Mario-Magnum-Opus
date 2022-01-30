using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lakitu : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed;
    float timeSinceLastAttacked;
    public float attackDelay;
    public Animator animator;

    public GameObject grenade;
    public Transform throwPosition;

    GameObject player;

    float currentDirection;

    public SoundControl soundScript;
    public AudioSource[] trackShift;

    public GameObject mushroom;
    public GameObject healthbar;

    private GameObject spawnedMushroom;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        timeSinceLastAttacked = Time.time;
        player = GameObject.Find("player1");
        currentDirection = 1;
        rb.AddForce(new Vector2(currentDirection * speed, 0f), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {

            if (Mathf.Abs(player.transform.position.x - transform.position.x) > 2)
            {


                currentDirection = Mathf.Sign(player.transform.position.x - transform.position.x);

            }
            //   Debug.Log(Mathf.Sign(rb.velocity.x));

            //flipping
            if (Mathf.Sign(rb.velocity.x) == -1 && Mathf.Abs(rb.velocity.x) > 1)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("grenadeThrow") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
            {
                animator.SetBool("attacking", false);
                rb.AddForce(new Vector2(currentDirection * speed, 0f), ForceMode2D.Impulse);
            }

            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("grenadeThrow"))
            {
                if (Mathf.Sign(rb.velocity.x) != currentDirection)
                {
                    rb.AddForce(new Vector2(1.4f * currentDirection * speed, 0f), ForceMode2D.Impulse);
                }
                else
                {
                    rb.AddForce(new Vector2(currentDirection * speed, 0f), ForceMode2D.Impulse);
                }
            }
            if ((Mathf.Abs(player.transform.position.x - transform.position.x) < 4) && timeSinceLastAttacked + attackDelay < Time.time)
            {
                timeSinceLastAttacked = Time.time;
                rb.velocity = new Vector2(0f, rb.velocity.y);
                animator.SetBool("attacking", true);
                Invoke("throwGrenade", 0.4f);
            }



        }
    }

    void OnEnable()
    {

        //    soundScript.trackShift(trackShift);
        SoundControl.trackShift(trackShift);
        healthbar.SetActive(true);

    }

    void throwGrenade() {
        float randomNum = Random.Range(0, 10);
        if (player.GetComponent<PlayerHealth>().health <= 20 && randomNum > 6 && spawnedMushroom == null)
        {
            spawnedMushroom = Instantiate(mushroom, throwPosition.position, throwPosition.rotation); 
        }
        else
        {
            Instantiate(grenade, throwPosition.position, throwPosition.rotation);
        }
    }

 
 

  

}
