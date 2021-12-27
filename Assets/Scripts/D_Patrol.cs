using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class D_Patrol : MonoBehaviour
{
    public float speed;
    public LayerMask mask;
    private Vector2 dir;
    // Start is called before the first frame update
    void Start()
    {
        dir = Vector2.right;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D sideDetect = Physics2D.Raycast(transform.position, dir, 0.5f, mask);
        Debug.DrawRay(transform.position, Vector2.right * 1, Color.red);
        if (sideDetect.collider != null)
        {
            if ((sideDetect.collider.tag == "Ground" || sideDetect.collider.tag == "Breakable"))
            {
                speed *= -1;
                dir *= -1;
            }
        }

    }
 }
