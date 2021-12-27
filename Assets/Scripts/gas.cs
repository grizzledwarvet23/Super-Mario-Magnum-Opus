using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gas : MonoBehaviour
{
    public float lifeTime;
    private float timeStarted;
    private float timeLastDamaged;
    public int damage;
    public float period;

    public bool infiniteTime;

    private GameObject collidedObject;
    private void Start()
    {
        timeStarted = Time.time;
        timeLastDamaged = Time.time;
    }

    private void Update()
    {
        if (!infiniteTime)
        {
            if (Time.time >= timeStarted + lifeTime)
            {
                Destroy(gameObject);
            }
        }
        if (collidedObject != null)
        {
//            Debug.Log(collidedObject);
            if (Time.time >= timeLastDamaged + period)
            {
                collidedObject.GetComponent<PlayerHealth>().takeDamage(damage);
                timeLastDamaged = Time.time;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collidedObject = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collidedObject = null;
        }
    }
    /*
     * if(collision.gameObject.tag == "Player" && Time.time >= timeLastDamaged + period)
        {
            collision.gameObject.GetComponent<PlayerHealth>().takeDamage(damage);
            timeLastDamaged = Time.time;
        }
     * 
     */
}
