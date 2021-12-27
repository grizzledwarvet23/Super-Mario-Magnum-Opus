using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dieAfterTime : MonoBehaviour
{
    private float time;
    public float lifeSpan;

    void Start()
    {
        time = Time.time;   
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= time + lifeSpan)
        {
            Destroy(gameObject);
        }
    }
}
