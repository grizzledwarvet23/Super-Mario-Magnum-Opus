using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaShell : MonoBehaviour
{
    public Transform firepointLeft;
    public Transform firepointRight;
    public GameObject bulletPrefab;

    public AudioSource gunSound;

    public LayerMask bounceMask;

    public BoxCollider2D childCollider;

    public GameObject[] weaponSprites;
    Rigidbody2D rb;
    Animator animator;

    bool isMoving;
    float direction;

    public GameObject deadSprite;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        isMoving = false;
        animator = gameObject.GetComponent<Animator>();
    }

    void FixedUpdate() {
        RaycastHit2D leftRay = Physics2D.Raycast(firepointLeft.position, Vector2.left, 1.5f, bounceMask);
        RaycastHit2D rightRay = Physics2D.Raycast(firepointRight.position, Vector2.right, 1.5f, bounceMask);
        if (leftRay.collider != null)
        {


            if (leftRay.collider.CompareTag("Ground"))
            {
                direction *= -1;
                rb.velocity = new Vector2(12 * direction, rb.velocity.y);
            }
            else if (leftRay.collider.CompareTag("Enemy") && isMoving) {
                Destroy(leftRay.collider.gameObject);
            }
            


        }
        else if (rightRay.collider != null) {
            if (rightRay.collider.CompareTag("Ground")) {
                direction *= -1;
                rb.velocity = new Vector2(12 * direction, rb.velocity.y);

            }
            else if (rightRay.collider.CompareTag("Enemy") && isMoving)
            {
                Destroy(rightRay.collider.gameObject);
            }




        }


    }





    void shootLeft() {
        weaponSprites[0].SetActive(true);
        gunSound.Play();
        GameObject bullet = Instantiate(bulletPrefab, firepointLeft.position, firepointLeft.rotation);
        if (rb.velocity.x <= 0)
        {
            bullet.GetComponent<Bullet>().Initialize(10, (int) -rb.velocity.x);
        }
        

    }
    void shootRight() {
        weaponSprites[1].SetActive(true);
        gunSound.Play();
        GameObject bullet = Instantiate(bulletPrefab, firepointRight.position, firepointRight.rotation);

        if (rb.velocity.x >= 0)
        {
            bullet.GetComponent<Bullet>().Initialize(10, (int)rb.velocity.x);
        }
    }

    void setSpritesInvisible() {
        foreach (GameObject g in weaponSprites) {
            g.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D col) {
//        Debug.Log(col.collider.gameObject);
        if (col.collider.gameObject.layer == LayerMask.NameToLayer("Player") && !isMoving) {
            Move2D moveScript = col.collider.gameObject.GetComponent<Move2D>();
            moveScript.Jump(moveScript.jumpHeight, true);
            GameObject obj = Instantiate(deadSprite, transform.position, transform.rotation);
            obj.GetComponent<Rigidbody2D>().AddForce(new Vector2(5, 9), ForceMode2D.Impulse);
            Destroy(gameObject);

            /*
            if (transform.position.x - col.collider.gameObject.transform.position.x >= 0) {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
            rb.velocity = new Vector2(16 * direction, rb.velocity.y);
            gameObject.layer = 18;
            childCollider.gameObject.layer = 18;
            setSpritesInvisible();
            animator.SetBool("isMoving", true);
            */




        }
    
    }


    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(firepointLeft.position, new Vector2(-1, 0));
        Gizmos.DrawRay(firepointRight.position, new Vector2(1, 0));




    }





    }
