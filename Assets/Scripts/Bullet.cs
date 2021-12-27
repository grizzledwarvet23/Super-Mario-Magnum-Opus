using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Bullet : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    public int damage;
    public float shotDuration;
    private float timeSinceFired;

    public GameObject effectOnDeath;
    public Vector3 effectDisplacement;

    public List<string> tagsToDamage; //OPTIONAL: Used for the torpedo, to attack the enemy tag

    public float waitDuration = 0;

    public bool hasVerticalVel = false;
    public float verticalVelocity = 0;

    public bool ricochet; //level 4
    
    // Start is called before the first frame update
    void Start()
    {
        timeSinceFired = Time.time;
        //  rb.velocity = transform.right * speed;
        Invoke("move", waitDuration);


    }

    void move()
    {
        rb.velocity = transform.right * speed;
        
        if(hasVerticalVel)
        {            
            rb.velocity = new Vector2(rb.velocity.x, verticalVelocity);   
        }
    }

    

    void Update()
    {
        
        if (Time.time >= timeSinceFired + shotDuration)
        {
            if(effectOnDeath != null)
            {
             //used in Shy Guy gas bullets   
                Instantiate(effectOnDeath, transform.position, transform.rotation);
            }
            Destroy(gameObject);
        }



    }

    public void addVelocity(float velocity)
    {
        speed += velocity;
        rb.velocity = transform.right * speed;
    }
    public void Initialize(int bulletDamage, float extraVelocity)
    {
        damage = bulletDamage;
        speed += extraVelocity;

    }

    public void Initialize(int bulletDamage)
    {
        damage = bulletDamage;


    }

    public void instantiateEffect()
    {
        if (effectOnDeath != null)
        {
            Instantiate(effectOnDeath, transform.position + effectDisplacement, transform.rotation);
        }
    }
 
    
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.tag == "Ground")
        {
            if(!ricochet) 
            { 
                instantiateEffect();
                Destroy(gameObject);
            }
        }
        if(tagsToDamage != null)
        {
            if (tagsToDamage.Contains("Enemy") && hitInfo.tag == "Enemy")
            {
                hitInfo.GetComponent<AIDamage>().TakeDamage(damage);
                instantiateEffect();
                Destroy(gameObject);
            }
            else if (tagsToDamage.Contains("Breakable") && hitInfo.tag == "Breakable")
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (ricochet && (col.gameObject.tag == "Ground" || col.gameObject.tag == "Projectile"))
        {
            var lastVelocity = new Vector2(speed, verticalVelocity);
            //  Debug.Log(lastVelocity);
            var newSpeed = lastVelocity.magnitude;
            var direction = Vector2.Reflect(lastVelocity.normalized, col.contacts[0].normal);
            rb.velocity = direction * Mathf.Max(newSpeed, 0f);
            
            speed = rb.velocity.x;
            verticalVelocity = rb.velocity.y;

        }

    }




}
