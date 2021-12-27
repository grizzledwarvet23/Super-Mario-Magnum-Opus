using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatform : MonoBehaviour
{

    public bool digitalMotion;
    public float bounds_x;
    public float bounds_y;

    public float velocity_x;
    public float velocity_y;

    public bool horizontal;

    Rigidbody2D rb;
    Vector3 origin;

    public float period;
    private bool movingRight;
  //  float timePassed;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(velocity_x, velocity_y);
        if(velocity_x >= 0)
        {
            movingRight = true;
        }
        else
        {
            movingRight = false;
        }
        origin = transform.position;
       // Debug.Log(origin.x);
        if (period == 0)
        {
            period = 1;
        }
    }

   

    void Update()
    {
        //   Debug.Log(Time.timeSinceLevelLoad);
        // timePassed = Time.time;
        if (digitalMotion)
        {
            if (horizontal)
            {
                if (transform.position.x >= bounds_x)
                {
                    transform.position = new Vector3(transform.position.x - 0.2f, transform.position.y, transform.position.z);
                    rb.velocity = new Vector2(rb.velocity.x * -1, velocity_y);
                    if (gameObject.tag == "Enemy")
                    {
                        flip();
                    }
                }
                else if (transform.position.x <= origin.x)
                {
                    transform.position = new Vector3(transform.position.x + 0.2f, transform.position.y, transform.position.z);
                    rb.velocity = new Vector2(rb.velocity.x * -1, velocity_y);
                    if (gameObject.tag == "Enemy")
                    {
                        flip();
                    }
                }
            }
            else
            {
                if (transform.position.y >= bounds_y)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);
                    rb.velocity = new Vector2(velocity_x, rb.velocity.y * -1);
                }
                else if (transform.position.y <= origin.y)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
                    rb.velocity = new Vector2(velocity_x, rb.velocity.y * -1);
                }
            }

        }
        else
        {
            if (horizontal)
            {
                rb.velocity = new Vector2(Mathf.Sin(Time.timeSinceLevelLoad / period) * velocity_x, velocity_y);

            }
            else
            {
                rb.velocity = new Vector2(velocity_x, Mathf.Sin(Time.timeSinceLevelLoad / period) * velocity_y);
            }
        }

    }

    private void flip()
    {
        if (movingRight)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        movingRight = !movingRight;
    }
}