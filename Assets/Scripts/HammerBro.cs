using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerBro : MonoBehaviour, BossInterface
{
    public float velocity_x;
    public Rigidbody2D rb;
    public GameObject player;

    public float leftBound;
    public float rightBound;

    public LayerMask attackMask;
    public Animator anim;
    public Transform attackDetectPos;

    [System.NonSerialized]
    public bool attacking;
    private bool returningToCenter;

    public float centerBound_L;
    public float centerBound_R;

    public AudioSource attackSound;

    
    public bool facingRight = false; //change according to intial orientation

    [System.NonSerialized]
    public float lastTimeJumped;
    public float interval = 15;

    private float lastTimeAttacked;
    public float attackInterval = 2;

    private float timeAfterAttack = 3f;


    private void Start()
    {
        lastTimeJumped = Time.timeSinceLevelLoad;
        lastTimeAttacked = Time.timeSinceLevelLoad;
    }
    private void FixedUpdate()
    {
        if (!attacking)
        {
            rb.velocity = -transform.right * velocity_x;
        }
    }

    

    private void Update()
    {
        if (!attacking)
        {
            checkFlip();
        }
        if (transform.position.x < centerBound_R && transform.position.x > centerBound_L)
        {
            returningToCenter = false;
        }
        Collider2D attackTrigger = Physics2D.OverlapCircle(attackDetectPos.position, 5, attackMask);
        if (attackTrigger != null && attackTrigger.gameObject.tag == "Player" && !attacking && Time.timeSinceLevelLoad >= lastTimeAttacked + attackInterval)
        {
            StartCoroutine(attack());
        }

        if (!attacking && Time.timeSinceLevelLoad >= lastTimeJumped + interval && anim.GetCurrentAnimatorStateInfo(0).IsName("sledge_walk"))
        {
            lastTimeJumped = Time.timeSinceLevelLoad;
            attacking = true;
            anim.SetBool("jumpFromLeft", !anim.GetBool("jumpFromLeft"));
            anim.Play("sledge_flyup");
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
        }

    }


    void checkFlip()
    {
        if (!returningToCenter && (facingRight && transform.position.x > player.transform.position.x + 12 || !facingRight && transform.position.x < player.transform.position.x - 12))
        {
      
            flip();
       
        }
        else if (!returningToCenter && (transform.position.x <= leftBound /* && player.transform.position.x < leftBound */|| 
            transform.position.x >= rightBound /* && player.transform.position.x >  rightBound*/))
        {
            returningToCenter = true;
            flip();
            
        }
    }
    public void flip()
    {
        transform.Rotate(0, 180f, 0f);
        facingRight = !facingRight;
    }

    IEnumerator attack()
    {
       // lastTimeAttacked = Time.timeSinceLevelLoad;
        attacking = true;
        rb.velocity = Vector2.zero;
        
        
        anim.Play("sledge_attack_test");
        yield return new WaitForSeconds(0.2f);
        Jump();
        yield return new WaitForSeconds(0.3f);
        attackSound.Play();

        yield return new WaitForSeconds(timeAfterAttack);
        attacking = false;
        lastTimeAttacked = Time.timeSinceLevelLoad;
        anim.Play("sledge_walk");
       
    }

    public void onDamageTaken()
    {
        if (GetComponent<AIDamage>().health == 800)
        {
            StopAllCoroutines();

            anim.Play("sledge_transition");
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
      //      anim.SetFloat("attackSpeed", 1.25f);
      //      timeAfterAttack = 2.2f;
            //this.enabled = false;
        }

        else if (GetComponent<AIDamage>().health == 400)
        {
            StopAllCoroutines();
            anim.Play("sledge_transition2");
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = Vector2.zero;
            anim.SetFloat("attackSpeed", 1.25f);
            timeAfterAttack = 2.2f;

        }
    }

    void Jump()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(new Vector2(0f, 8f), ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackDetectPos.position, 5);
    }
}
