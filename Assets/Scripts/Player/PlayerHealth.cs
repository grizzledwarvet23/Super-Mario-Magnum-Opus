using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health; //public float health;
    public Collider2D[] playerColliders;

    private Move2D move2D;
    private Submarine sub;
    private Spaceship ship;

    private SpriteRenderer renderer;
    Color originalColor;
    public bool invulnerable;
    public Slider healthbar;
    public Image healthFill;
    public Image healthBackground;
    
    public Animator doomAnimator;

    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    public Sprite[] spriteColors;
    public AudioSource damageSound;
    public AudioSource deathSound;
    public AudioSource healSound;
    public string deathAnimationName; //"marioFalling" by default
    GameObject cineHolder;

    Rigidbody2D rb;
    Animator animator;

    private AudioSource[] allAudioSources;

    public bool noForceOnDeath;
    public bool isDying; //DO NOT EDIT FROM INSPECTOR

    public int timesFlash; //times to flash when invulnerable

    [System.NonSerialized]
    public bool locked = false;


    void Start()
    {
        health = 100;
        animator = gameObject.GetComponent<Animator>();
        cineHolder = GameObject.Find("CinemachineHolder");
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (gameObject.GetComponent<Move2D>() != null)
        {
            move2D = gameObject.GetComponent<Move2D>();
        }
        else if (gameObject.GetComponent<Submarine>() != null)
        {
            sub = gameObject.GetComponent<Submarine>();
        }
        else if (gameObject.GetComponent<Spaceship>() != null)
        {
            ship = gameObject.GetComponent<Spaceship>();
        }
        renderer = gameObject.GetComponent<SpriteRenderer>();
        originalColor = renderer.color;
        invulnerable = false;
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
        isDying = false;
    }

  


    public void takeDamage(float damage)
    {
//        Debug.Log("damage taken");
        if (!invulnerable || (invulnerable && damage < 0))
        {
//            Debug.Log(damage);
            Physics2D.IgnoreLayerCollision(12, 9, true);
            Physics2D.IgnoreLayerCollision(12, 13, true);

         //  if (damage >= 0)
           // {
                invulnerable = true;
            //}

            health -= damage;
            if(health > 100) //if healing
            {
                health = 100;
            }
            // healthbar.value -= damage;
            if (damage < 0 && healSound != null)
            {
                healSound.Play();
            }
            else if(damageSound != null)
            {
                damageSound.Play();
            }
            doomAnimator.SetInteger("health", (int)health);
            if (health >= 70)
            {
                fullHeart = spriteColors[0];
                halfHeart = spriteColors[1];

            }
            else if (health < 70 && health > 30)
            {
                fullHeart = spriteColors[2];
                halfHeart = spriteColors[3];

            }
            else if (health <= 30) {
                fullHeart = spriteColors[4];
                halfHeart = spriteColors[5];
 
            }
            for (int i = 0; i < hearts.Length; i++) {
          
                 if (i + 1 == (health / 20) + 0.5f)
                {
                    hearts[i].sprite = halfHeart;
                }
                else if (i < health / 20)
                {

                    hearts[i].sprite = fullHeart;
                }
                else {
                    hearts[i].sprite = emptyHeart;
                }



                if (i < numOfHearts) {
                    hearts[i].enabled = true;
                }
                else
                {
                    hearts[i].enabled = false;
                }
            }
            StartCoroutine(Flash(0.06f));
            if (health <= 0)
            {
                StartCoroutine(Die());

            }
        }
    }

    public void Damage(float[] attackDetails)
    {
        int direction;
        takeDamage(attackDetails[0]);
        //Damage player here using attackDetails[0];


        if (attackDetails[1] < transform.position.x)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
        if (move2D != null)
        {
            move2D.Knockback(direction);
        }
        
    }

    public IEnumerator Die()
    {
        isDying = true;
        //  AudioListener.pause = true;
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources) {
            audioS.Stop();
        }
        Scene scene = SceneManager.GetActiveScene();

        //nullify user input
        if (move2D != null)
        {
            move2D.enabled = false;
        }
        else if (sub != null)
        {
            sub.enabled = false;
        }
        else if(ship != null)
        {
            ship.enabled = false;
            ship.gameObject.GetComponentInChildren<ParticleSystem>().gameObject.SetActive(false);
        }
        if (gameObject.GetComponent<Weapon>() != null)
        {
            gameObject.GetComponent<Weapon>().enabled = false;
        }
        foreach (Collider2D col in playerColliders)
        {
            col.enabled = false;
        }
        // BELOW 2 REPLACES ABOVE FOR LOOP
     //   gameObject.GetComponent<BoxCollider2D>().enabled = false;
     //   gameObject.transform.GetChild(2).GetComponent<CapsuleCollider2D>().enabled = false;

        rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        if (!noForceOnDeath)
        {

            rb.AddForce(new Vector2(0, 15), ForceMode2D.Impulse);
        }

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        
        if (deathAnimationName != null && deathAnimationName.Length > 0) //done for submarine level
        {
            //Debug.Log(deathAnimationName);
            animator.Play(deathAnimationName);
        }
        else
        {
            animator.Play("marioFalling");
        }
        deathSound.Play();

 
        cineHolder.SetActive(false);
        

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(scene.name);
    //    AudioListener.pause = false;
    }

    public void lockControl()
    {
        //I MADE HIM FLIP FOR BOO BOSS. CHANGE THIS LATER SURE
        move2D.flip();
        move2D.enabled = false;
        animator.SetFloat("Speed", 0.0f);
        gameObject.GetComponent<Weapon>().enabled = false;
        gameObject.GetComponent<SoundControl>().disabled = true;

        locked = true;
    }
    
    //OVERLOADED VERSION OF ABOVE
    public void lockControl(bool flip)
    {
        //I MADE HIM FLIP FOR BOO BOSS. CHANGE THIS LATER SURE
        if (flip)
        {
            move2D.flip();
        }
        move2D.enabled = false;
        animator.SetFloat("Speed", 0.0f);
        gameObject.GetComponent<Weapon>().enabled = false;
        gameObject.GetComponent<SoundControl>().disabled = true;

        locked = true;
    }

    public void unlockControl()
    {
        move2D.enabled = true;
        gameObject.GetComponent<Weapon>().enabled = true;
        gameObject.GetComponent<SoundControl>().disabled = false;

        locked = false;
    }


    IEnumerator Flash(float flashSpeed)
    {
        if(timesFlash == 0)
        {
            timesFlash = 4;
        }
        for (int i = 0; i < timesFlash; i++)
        {
            renderer.enabled = false;
            yield return new WaitForSeconds(flashSpeed);
            renderer.enabled = true;
            yield return new WaitForSeconds(flashSpeed);
        }
        invulnerable = false;




    }
}
