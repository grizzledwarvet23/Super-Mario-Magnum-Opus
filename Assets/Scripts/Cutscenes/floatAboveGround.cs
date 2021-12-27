using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floatAboveGround : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    public float period;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if(period == 0)
        {
            period = 4;
        }

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Sin(period * Time.timeSinceLevelLoad) * 1.5f);
    }
}
