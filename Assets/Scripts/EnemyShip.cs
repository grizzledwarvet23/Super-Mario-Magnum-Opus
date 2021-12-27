using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShip : MonoBehaviour
{
    public GameObject projectile;
    public Transform[] firePoints;
    public float fireRate;
    public AudioSource fireSound;

    public Vector2 velocity;
    Rigidbody2D rb;

    public float introDuration = 1;

    
    [Tooltip("0 = Linear Movement \n 1 = Horizontal Sinusodial Movement \n 2 = Vertical Sinusodial Movement")]
    public int behavior;

    [SerializeField]
    private float phaseShift, period;

    private float spawnTime;

    private bool introActive = true;
    [Tooltip("Starting and ending positions for the intro")]
    public Vector2 startPos, endPos;


    private float t;


    void Start()
    {
        spawnTime = Time.timeSinceLevelLoad;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(introActive)
        {
            t += Time.deltaTime / introDuration;
            transform.localPosition = Vector2.Lerp(startPos, endPos, t);
            if(t >= 1.0f)
            {
                introActive = false;
                InvokeRepeating("fire", 0.5f, fireRate);
                spawnTime = Time.timeSinceLevelLoad;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!introActive)
        {
            switch (behavior)
            {
                //Linear Movement
                case 0:
                    rb.velocity = velocity;
                    break;
                //Horizontal sinusodial Movement
                case 1:
                    rb.velocity = new Vector2(velocity.x * Mathf.Sin((Time.timeSinceLevelLoad - spawnTime + phaseShift) / period), velocity.y);
                    break;
                //Vertical sinusodial movement
                case 2:
                    rb.velocity = new Vector2(velocity.x, velocity.y * Mathf.Sin((Time.timeSinceLevelLoad - spawnTime + phaseShift) / period));
                    break;
                default:
                    Debug.Log("Invalid Behavior!");
                    break;
            }
        }
    }

    private void fire()
    {
        foreach(Transform firePoint in firePoints)
        {
            Instantiate(projectile, firePoint.position, firePoint.rotation);
        }
        if (fireSound != null && !fireSound.isPlaying)
        {
            fireSound.Play();
        }

    }


}
