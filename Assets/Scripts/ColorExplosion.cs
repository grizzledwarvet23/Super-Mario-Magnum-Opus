using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorExplosion : MonoBehaviour
{
    Collider2D collider;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {            
            col.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(4 * Mathf.Sign(col.gameObject.transform.position.x - transform.position.x), 3f * Mathf.Sign(col.gameObject.transform.position.y - transform.position.y)), ForceMode2D.Impulse);
        }
    }
}
