using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkIfColliding : MonoBehaviour
{
    public bool colliding;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        colliding = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        colliding = false;
    }
}
