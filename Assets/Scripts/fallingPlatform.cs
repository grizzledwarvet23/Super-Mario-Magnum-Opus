using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class fallingPlatform : MonoBehaviour
{
    public bool movingHorizontal;
    public float h_vel;
    private Rigidbody2D rb;

    public bool newCamBounds;

    public GameObject ogCam;
    public GameObject tgCam;

    public GameObject[] activateOnCollision;

    public bool respawn;

    private Vector3 originalPos;

    public bool hasVerticalVel = false;
    public float verticalVel;

    public GameObject[] boosters;

    public float delay = 0;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        originalPos = transform.position;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") && collision.collider.gameObject.GetComponent<GroundCheck>().checkIfGrounded())
        {
            if (!movingHorizontal)
            {
                if (!hasVerticalVel)
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.mass = 8;
                    rb.gravityScale = 2.5f;
                    StartCoroutine(vanish(8.0f));
                }
                else
                {
                    foreach(GameObject obj in activateOnCollision)
                    {
                        obj.SetActive(true);
                    }
                    Invoke("setVelocity", delay);
                }
            }
            else
            {
                rb.velocity = new Vector2(h_vel, rb.velocity.y);
              //  vanish(8.0f);
            }
            if(newCamBounds)
            {
                ogCam.SetActive(false);
                tgCam.SetActive(true);
                
            }
            
        }
            
        
        

    }

    void setVelocity()
    {
        if(hasVerticalVel)
        {
            rb.velocity = new Vector2(rb.velocity.x, verticalVel);
            foreach(GameObject obj in boosters)
            {
                obj.SetActive(true);
            }
        }

    }

    IEnumerator vanish(float time)
    {
        yield return new WaitForSeconds(time);
        
        if (respawn)
        {
//            Debug.Log("yup");
            rb.bodyType = RigidbodyType2D.Static;
            rb.velocity = new Vector2(0, 0);
            transform.position = originalPos;
        }
        
        else
        {
            Destroy(gameObject);
        }
    }
}
