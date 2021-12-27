using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public float Direction;
    public float moveSpeed = 2f;
    Vector3 movement;
    GameObject fellowEnemy;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Direction = 1f;
        movement = new Vector3(1f, 0f, 0f);
        transform.position += movement * Time.deltaTime * moveSpeed * Direction;





    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.collider.tag == "Player")
        {
            Destroy(collision.collider.gameObject);
        }
        /*
        else if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
            fellowEnemy = collision.collider.gameObject;
            Direction *= -1 * fellowEnemy.GetComponent<AIMovement>().Direction;
        }
        */
        else if (collision.collider.tag == "Border")
        {
            Direction *= -1;
        }
    
    
    }


}
