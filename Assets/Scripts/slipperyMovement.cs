using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class slipperyMovement : MonoBehaviour
{
    public bool onIce;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedWith = collision.gameObject;
        if (collidedWith != null)
        {
            if (collidedWith.GetComponent<Tilemap>() != null && collidedWith.tag == "Ground" && 
                collidedWith.GetComponent<Rigidbody2D>() != null  && 
                collidedWith.GetComponent<Rigidbody2D>().sharedMaterial.name == "Ice_Slippery")
            {
                //PRECONDITION: OBJECT MUST BE PLAYER, WITH MOVE2D
                onIce = true;
                gameObject.GetComponent<Move2D>().enableSlippery = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.gameObject.GetComponent<Tilemap>() != null && collision.gameObject.tag == "Ground" &&
            collision.gameObject.GetComponent<Rigidbody2D>() != null &&
            collision.gameObject.GetComponent<Rigidbody2D>().sharedMaterial.name == "Ice_Slippery")
        {
            onIce = false;
            gameObject.GetComponent<Move2D>().enableSlippery = false;
        }
    }
   

    
}
