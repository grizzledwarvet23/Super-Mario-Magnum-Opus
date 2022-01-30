using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObject : MonoBehaviour
{
    public Camera cam;
    public float parallaxFactor;

    private Vector2 prevPos, newPos;
    // Start is called before the first frame update
    void Start()
    {
        prevPos = cam.transform.position;
        newPos = cam.transform.position;
      //  Debug.Log(newPos);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        newPos = cam.transform.position;

        Vector2 disp = (newPos - prevPos);
        transform.position += new Vector3(disp.x, disp.y, 0) * parallaxFactor;
        prevPos = newPos;
       // transform.position += new Vector3(player_rb.velocity.x * Time.deltaTime, player_rb.velocity.y * Time.deltaTime, 0) * parallaxFactor;
    }
}
