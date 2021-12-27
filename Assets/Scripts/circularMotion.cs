using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circularMotion : MonoBehaviour
{
    public Vector2 magnitude;
    public float period;
    private Rigidbody2D rb;

    private float timeSinceEnabled;
    private Vector2 spawnPos;

    private bool zeroed= false;

    private float timeStartZero, zeroedDuration = 0;

    private void Start()
    {
        spawnPos = transform.position;
        rb = gameObject.GetComponent<Rigidbody2D>();
        if(period == 0)
        {
            period = 1;
        }
    }

    private void OnEnable()
    {
        timeSinceEnabled = Time.timeSinceLevelLoad;
//        Debug.Log(Time.timeSinceLevelLoad);
        if (spawnPos != Vector2.zero)
        {
            transform.position = spawnPos;
        }
        timeStartZero = 0;
        zeroedDuration = 0;

    }

    void Update()
    {
        if (!zeroed)
        {
            float time = Time.timeSinceLevelLoad - timeSinceEnabled - zeroedDuration;
            //        Debug.Log(time);
            rb.velocity = magnitude * new Vector2(Mathf.Cos(period * time), Mathf.Sin(period * time));
        }
    }

    //used in spacekoopa attack animation
    void zeroSpeed()
    {
        timeStartZero = Time.timeSinceLevelLoad;
        rb.velocity = Vector2.zero;
        zeroed = true;
    }

    void unzero()
    {
        zeroedDuration += Time.timeSinceLevelLoad - timeStartZero;
        zeroed = false;
    }
}
