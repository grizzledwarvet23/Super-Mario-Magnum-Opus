using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public GameObject followObject;
    public Vector2 followOffset;
    private Vector2 threshold;
    public float speed = 3;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = followObject.GetComponent<Rigidbody2D>();
        threshold = calculateThreshold();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 follow = followObject.transform.position;
        //^^^ location of player

        float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
        float yDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.y);

        Vector3 newPosition = transform.position;
        if (Mathf.Abs(xDifference) >= threshold.x)
        {
            //if the x distance between player and camera exceeds threshold...

            newPosition.x = follow.x;
        }
        if (Mathf.Abs(yDifference) >= threshold.y)
        {
            //if y distance between player and camera exceeds threshold...
            newPosition.y = follow.y;
        }
        //the ? and the : below means if true then the value is the one after the ?, and the false one is the one followed by the :
        float moveSpeed = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed;
        transform.position = Vector3.MoveTowards(transform.position, newPosition, speed * Time.deltaTime);

    }

    //movement threshold at which camera starts moving
    private Vector3 calculateThreshold()
    {
        Rect aspect = Camera.main.pixelRect;
        Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
        t.x -= followOffset.x;
        t.y -= followOffset.y;
        return t;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = calculateThreshold();
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    }

}
