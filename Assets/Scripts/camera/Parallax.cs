using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;

    private float lengthY, startPosY;

    public GameObject cam;
    public float parallaxEffect;

    
    public bool verticalParallax = false;

    public bool smoothingX = false;
    public bool smoothingY = false;
    public float smoothingFactorX;
    public float smoothingFactorY;

    public bool childOfCam; //smooths parallax, helps remove jitter. used in lvl 5

    void Start()
    {
        startpos = transform.position.x;
        startPosY = transform.position.y;
        length = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        lengthY = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;

        if (childOfCam)
        {
            transform.parent = cam.transform;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));

        float dist = (cam.transform.position.x * parallaxEffect);

        

        Vector3 newPosition = new Vector3(startpos + dist, transform.position.y, transform.position.z);


        /*
        if (smoothingX)
        {
            transform.position = Vector2.Lerp(transform.position, newPosition, smoothingFactorX * Time.fixedDeltaTime);
        }
        else
        {
        */
            transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);
        //}

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;

        if(verticalParallax)
        {
            float tempY = (cam.transform.position.y * (1 - parallaxEffect));
            float distY = (cam.transform.position.y * parallaxEffect);

            Vector3 newPositionY = new Vector3(transform.position.x, startPosY + distY, transform.position.z);
            /*
            if (smoothingY)
            {
                transform.position = Vector2.Lerp(transform.position, newPositionY, smoothingFactorY * Time.fixedDeltaTime);
            }
            */
           // else
           // {
                transform.position = newPositionY;
           // }
            

            
            if (tempY > startPosY + lengthY) startPosY += lengthY;
            else if (tempY < startPosY - lengthY) startPosY -= lengthY;

        }
    }

    
}
