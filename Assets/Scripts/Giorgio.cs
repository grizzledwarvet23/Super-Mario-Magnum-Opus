using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Giorgio : MonoBehaviour, BossInterface
{

    /** This class was so cool to me
     * because I was imitating mario's movement
     * but with funky, simplified versions of the original logic
     * enjoy!
     */

    public GameObject[] UI;

    private Animator anim;

    
    public Transform firePoint;
    public GameObject projectile;
    public GameObject projectile2;
    public Transform projParent;
    public AudioSource fireSound;

    private int cycleCount;
    private int dir;

    public float walkSpeed;
    public float jumpSpeed;
    public AudioSource jumpSound;
    private Rigidbody2D rb;

    private bool walking = false;

    GameObject player;

    private bool initial = true;
    private bool canAttack = false;

    //THIS IS USED FOR ATTACK DECISION MAKING. its like (or is?) a decision tree
    private int leftBound = 0;
    private int rightBound = 2;

    public PlayableDirector[] outroTimelines;
    [TextArea(3, 10)]
    public string[] sentences;
    public string[] speakers;
    public DialogController dialogControl;

    // Start is called before the first frame update

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;
    }
    void Start()
    {
        tag = "Enemy"; //did this so mario only can attack after he starts
        player = GameObject.Find("player1");
        if (Mathf.Sign(transform.eulerAngles.y) == -1)
        {
            dir = -1;
        }
        else
        {
            dir = 1;
        }
        dir = -1;
        cycleCount = 0;
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        foreach (GameObject obj in UI)
        {
            obj.SetActive(true);
        }
        anim = GetComponent<Animator>();

        //initial attack
        Jump();
        StartCoroutine(ShootAerial("DOWN", 0.5f));
    }

    void Update()
    {
        anim.SetFloat("VerticalSpeed", rb.velocity.y);
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("giorgio_idle") && canAttack && player.GetComponent<PlayerHealth>().health > 0)
        {
            StartCoroutine(shootCycle());
        }
    }

    void FixedUpdate()
    {
        if (walking)
        {
            rb.velocity = new Vector2(dir * walkSpeed, rb.velocity.y);
        }
    }

    //first boolean is direction
    IEnumerator Walk(float time)
    {
        walking = true;
        anim.SetBool("Walking", true);
        yield return new WaitForSeconds(time);
        walking = false;
        anim.SetBool("Walking", false);
     //   rb.velocity = new Vector2(0, rb.velocity.y);
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0, jumpSpeed), ForceMode2D.Impulse);
        jumpSound.Play();

    }
    void flip()
    {
        dir *= -1;
        transform.Rotate(0, 180, 0);
    }

    IEnumerator shootCycle()
    {
        canAttack = false;

        if (cycleCount == 5)
        {
            leftBound = 0;
            rightBound = 2;
            cycleCount = 0;
            Jump();
            StartCoroutine(ShootAerial("DOWN", 0.5f));
        }
        else
        {
            int choice = Random.Range(leftBound, rightBound); //starts out as 0, 1
            //flip if the player is not facing the same direction as the boss
            if (Mathf.Sign(player.transform.position.x - transform.position.x) != dir)
            {
                flip();
            }

            if (choice <= 0)
            {
                rightBound+= 2; //makes the other option more likely
                anim.Play("giorgio_shoot");
                yield return new WaitForSeconds(2f);
                if (Mathf.Sign(player.transform.position.x - transform.position.x) != dir)
                {
                    flip();
                }
                float k = Random.Range(1, 2);
                StartCoroutine(Walk(k));
                if (Random.Range(0, 2) == 0)
                {
                    yield return new WaitForSeconds(0.5f);
                    Jump();
                    if(Mathf.Abs(player.transform.position.x - transform.position.x) < 4) 
                    {
                        //shoot downward
                        StartCoroutine(ShootAerial("DOWN_NORMAL", 0.5f));
                        yield return new WaitForSeconds(1.5f);
                    }
                }
                yield return new WaitForSeconds(k + 1f);
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else //if the choice is greater than or equal to 1
            {
                leftBound-= 2; //makes the other option more likely
                Jump();
                if (Random.Range(0, 2) == 0)
                {
                    StartCoroutine(ShootAerial("FORWARD", Random.Range(0.1f, 0.3f)));
                }
                else
                {
                    StartCoroutine(ShootAerial("UP", 0.25f));
                }
                yield return new WaitForSeconds(2);
            }

            cycleCount++;
            canAttack = true;
        }
    }

    IEnumerator ShootAerial(string direction, float inTime)
    {
        yield return new WaitForSeconds(inTime);
        switch (direction)
        {
            case "DOWN":
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -90);
                
                //shoots upward
                anim.Play("giorgio_airshoot");
                anim.SetBool("Shooting", true);
                yield return new WaitForSeconds(0.3f);
                rb.AddForce(new Vector2(0, 250), ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.8f);
                anim.SetBool("Shooting", false);

                //resets angle
                yield return new WaitForSeconds(1f);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

                //stays in the air & makes it so that the player cant shoot the boss while in the air
                rb.bodyType = RigidbodyType2D.Static;
                GetComponent<Collider2D>().enabled = false;
                GetComponent<AICollision>().enabled = false;

                yield return new WaitForSeconds(1);

                if (!initial)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        fireSound.Play();
                        yield return new WaitForSeconds(0.2f);
                    }
                    int choice = Random.Range(0, 3);
                    switch (choice)
                    {
                        //left
                        case 0:
                            for (int i = 0; i < 8; i++)
                            {
                                Instantiate(projectile2, Vector2.Lerp(new Vector2(979, 20), new Vector2(950, 20), i / 7.0f), Quaternion.Euler(0, 0, -90), projParent);
                                yield return new WaitForSeconds(0.3f);
                            }
                            break;

                        //middle
                        case 1:
                            for (int i = 0; i < 4; i++)
                            {
                                Instantiate(projectile2, Vector2.Lerp(new Vector2(938, 20), new Vector2(950, 20), i / 3.0f), Quaternion.Euler(0, 0, -90), projParent);
                                Instantiate(projectile2, Vector2.Lerp(new Vector2(979, 20), new Vector2(964, 20), i / 3.0f), Quaternion.Euler(0, 0, -90), projParent);
                                yield return new WaitForSeconds(0.3f);
                            }
                            break;
                        //right     
                        case 2:
                            for (int i = 0; i < 8; i++)
                            {
                                Instantiate(projectile2, Vector2.Lerp(new Vector2(938, 20), new Vector2(964, 20), i / 7.0f), Quaternion.Euler(0, 0, -90), projParent);
                                yield return new WaitForSeconds(0.3f);
                            }
                            break;
                    }
                }
                else
                {
                    initial = false;
                }

                yield return new WaitForSeconds(1);

                //goes back down
                if (player.transform.position.x < 955) { transform.position = new Vector3(Random.Range(962, 975), transform.position.y, transform.position.z); }
                else { transform.position = new Vector3(Random.Range(942, 950), transform.position.y, transform.position.z); }
                GetComponent<Collider2D>().enabled = true;
                GetComponent<AICollision>().enabled = true;
                rb.bodyType = RigidbodyType2D.Dynamic;
                yield return new WaitForSeconds(3);
                canAttack = true;
                break;
            case "DOWN_NORMAL":
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -90);

                //shoots upward
                anim.Play("giorgio_airshoot");
                anim.SetBool("Shooting", true);
                yield return new WaitForSeconds(0.3f);
                rb.AddForce(new Vector2(0, 90), ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.8f);
                anim.SetBool("Shooting", false);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                break;
            case "UP":
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90);
                anim.Play("giorgio_airshoot");
                anim.SetBool("Shooting", true);
                yield return new WaitForSeconds(0.3f);
                rb.AddForce(new Vector2(0, -200), ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.3f);
                anim.SetBool("Shooting", false);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                break;
            case "FORWARD":
                anim.Play("giorgio_airshoot");
                anim.SetBool("Shooting", true);
                yield return new WaitForSeconds(0.3f);
                rb.AddForce(new Vector2(-dir * 60, 0), ForceMode2D.Impulse);
                yield return new WaitForSeconds(0.6f);
                anim.SetBool("Shooting", false);
                break;                
        }
    }

    void Shoot()
    {
        Quaternion rotation = firePoint.rotation;
        rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, Random.Range(rotation.eulerAngles.z - 5, rotation.eulerAngles.z + 5));
        GameObject shot = Instantiate(projectile2, firePoint.position, rotation, projParent);
        if (!fireSound.isPlaying)
        {
            fireSound.Play();
        }
    }

    IEnumerator ShootBurst()
    {
        for (int i = 1; i <= 5; i++)
        {
            Quaternion rotation = firePoint.rotation;
            float rotZ = 70 * (i / 5.0f);
            rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, Random.Range(rotZ - 5, rotZ + 5));
            Instantiate(projectile, firePoint.position, rotation, projParent);
            if (fireSound.isPlaying)
            {
                fireSound.Stop();
            }
            fireSound.Play();
            yield return new WaitForSeconds(0.25f);
        }
    }

    public void disable()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
    }

    public void dieAnimation()
    {
        anim.Play("giorgio_die");
    }

    public void onDamageTaken()
    {
        if (GetComponent<AIDamage>().health == 0)
        {
            StopAllCoroutines();
            Destroy(projParent.gameObject);

            foreach (GameObject obj in UI)
            {
                obj.SetActive(false);
            }

            GetComponent<AICollision>().enabled = false;
            walking = false;
            anim.SetBool("Walking", false);

            dialogControl.timelines = outroTimelines;
            dialogControl.sentences = sentences;
            dialogControl.speakers = speakers;

            StartCoroutine(dialogControl.outro(5, gameObject));
        }
    }

    
}
