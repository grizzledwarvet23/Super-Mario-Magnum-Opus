using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPieces : MonoBehaviour
{

    public Rigidbody2D rb;
    float startTime;
    // Start is called before the first frame update
    void Start()
    {
       // rb.velocity = new Vector2(10, rb.velocity.y);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= startTime + 1.25f)
        {
            Destroy(gameObject);
        }
    }
}
