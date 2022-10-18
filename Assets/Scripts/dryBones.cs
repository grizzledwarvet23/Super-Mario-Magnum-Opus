using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dryBones : MonoBehaviour
{
    public GameObject vcam;
    private Animator anim;
    private int currentDir;
    private bool activated;
    private Rigidbody2D rb;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Material mat;
    private AudioSource explodeSound;

    private BoxCollider2D cd;

    [SerializeField]
    private Vector2 colliderOffset;
    [SerializeField]
    private Vector2 colliderSize;

    public double range = 8;
    private bool canExplode;
    


    private void Start()
    {
        canExplode = false;
        anim = gameObject.GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        cd = gameObject.GetComponent<BoxCollider2D>();
        currentDir = 1;
        explodeSound = gameObject.GetComponent<AudioSource>();
        if(vcam == null)
        {
            GameObject temp = GameObject.Find("CinemachineBrain (2)");
            if(temp != null)
            {
                vcam = temp;
            }
            else
            {
                Debug.Log("dryBones doesn't have VCAM!");
            }
        }
    }
    void Update()
    {
        if(activated)
        {
            gameObject.layer = 0; //default layer is 0
            chase();
        }
        
        else if( Mathf.Abs(vcam.transform.position.x - transform.position.x) <= range && Mathf.Abs(vcam.transform.position.y - transform.position.y) <= range)
        {
            //TRIGGER BONES TO RISE
            cd.offset = colliderOffset;
            cd.size = colliderSize;
            gameObject.tag = "Enemy"; //so it can be destroyed by the deadzone
            anim.SetBool("inRange", true);
        }
    }

    public void activate()
    {
        activated = true;
        currentDir = -1;
    }

    private void chase()
    {
        if (vcam.transform.position.x < transform.position.x - 2)
        {
            currentDir = -1;
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(vcam.transform.position.x > transform.position.x + 2)
        {
            currentDir = 1;
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

        if (rb.velocity.y < -0.1f) //so it doesnt get stuck on walls if it is falling into a pit
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(currentDir * speed, rb.velocity.y);
        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            canExplode = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(activated && collision.gameObject.CompareTag("Player") && canExplode)
        {
         //   StartCoroutine(blowUp(collision));
            //explode
            
            activated = false;
            anim.Play("dryBonesExplode");

            collision.gameObject.GetComponent<PlayerHealth>().takeDamage(40);

            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(4f * Mathf.Sign(collision.gameObject.transform.position.x - transform.position.x), 15f), ForceMode2D.Impulse);


            gameObject.GetComponent<SpriteRenderer>().material = mat;

            rb.bodyType = RigidbodyType2D.Static;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;


            explodeSound.Play();
        //    StartCoroutine(selfDestruct());
           
        }
    }
    /*
    private IEnumerator blowUp(Collider2D collision)
    {
        //explode
        activated = false;
        anim.Play("dryBonesExplode");

        yield return new WaitForSeconds(0.0f);

        collision.gameObject.GetComponent<PlayerHealth>().takeDamage(40);
        Debug.Log(0.8f * Mathf.Sign(collision.gameObject.transform.position.x - transform.position.x));
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(4f * Mathf.Sign(collision.gameObject.transform.position.x - transform.position.x), 15f), ForceMode2D.Impulse);


        gameObject.GetComponent<SpriteRenderer>().material = mat;

        rb.bodyType = RigidbodyType2D.Static;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;


        explodeSound.Play();
        StartCoroutine(selfDestruct());
    }
    */
    //referenced in drybones explosion as an event
    public void selfDestruct()
    {
        Destroy(gameObject);
    }
    /*
    private IEnumerator selfDestruct()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    */
    
}
