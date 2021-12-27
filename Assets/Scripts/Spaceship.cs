using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour
{
    public Vector2 vel;
    Rigidbody2D rb;
 //   public GameObject scrollTrail;
  //  public float scrollSpeed;
    public GameObject projectile;
    public Transform firepoint;

    [SerializeField]
    private float shotDelay;
    private float timeLastFired;

    public AudioSource fireSound;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    //    scrollTrail.GetComponent<Rigidbody2D>().velocity = new Vector2(0, scrollSpeed);
    }

    void Update()
    {
        if ((Input.GetButton("Fire1") || Input.GetKey(KeyCode.O)) && Time.timeSinceLevelLoad >= timeLastFired + shotDelay)
        {
            timeLastFired = Time.timeSinceLevelLoad;
            fire();
        }
    }

    void fire()
    {
        if(!fireSound.isPlaying)
        {
            fireSound.Play();
        }
        Instantiate(projectile, firepoint.position, firepoint.rotation);
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * vel.x,  Input.GetAxisRaw("Vertical") * vel.y);
    }   


}
