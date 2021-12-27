using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    bool canAttack;
    public Animator animator;
    public AudioSource explosionSound;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        canAttack = false;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }




    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player" && canAttack)
        {
            col.gameObject.GetComponent<PlayerHealth>().takeDamage(20);
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.8f * Mathf.Sign(col.gameObject.transform.position.x - transform.position.x), 1.25f), ForceMode2D.Impulse);
            
        }
        
    }

    void OnCollisionEnter2D(Collision2D col) {
        if (col.collider.tag == "Ground" || col.collider.tag == "Player" || col.collider.tag == "BlockDestroyer" || col.collider.tag == "platform") {
            InitiateExplosion();
                }
    
    }
    IEnumerator DetonateSelf() {
        yield return new WaitForSeconds(0.2f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.transform.GetChild(0).GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(0.9f);
        Destroy(gameObject);
    }

    public void InitiateExplosion()
    {
        rb.bodyType = RigidbodyType2D.Static;
        canAttack = true;
        animator.SetBool("isExploding", true);
        explosionSound.Play();
        StartCoroutine("DetonateSelf");


    }
        

}
