using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{

    private BoxCollider2D playerCollider;
    public float bounceHeight;
    public LayerMask layermask;
    public float RayOffset;
    public PlayerHealth PH;

    Rigidbody2D rb;
    Transform movingPlatform;

   public  bool onMovingPlatform;
    void Start()
    {
        onMovingPlatform = false;
           
        playerCollider = gameObject.GetComponent<BoxCollider2D>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        }

    // Update is called once per frame
 
    public bool checkIfGrounded()
    {
        RaycastHit2D midRay = Physics2D.Raycast(playerCollider.bounds.center, Vector2.down, playerCollider.bounds.extents.y + RayOffset, layermask); ;
        RaycastHit2D rightRay = Physics2D.Raycast(new Vector2(playerCollider.bounds.center.x + playerCollider.bounds.extents.x - 0.22f, playerCollider.bounds.center.y), Vector2.down, playerCollider.bounds.extents.y + RayOffset, layermask); ;
        RaycastHit2D leftRay = Physics2D.Raycast(new Vector2(playerCollider.bounds.center.x - playerCollider.bounds.extents.x + 0.22f, playerCollider.bounds.center.y), Vector2.down, playerCollider.bounds.extents.y + RayOffset, layermask); ;
   
        RaycastHit2D[] castArray = new RaycastHit2D[] { leftRay, midRay, rightRay };
     //  if (!(leftRay.collider == null && midRay.collider == null && rightRay.collider == null))
     //  {
            for (int i = 0; i < castArray.Length; i++)
            {
            if (castArray[i].collider != null)
            {


                if (((castArray[i].collider.CompareTag("Ground") || castArray[i].collider.CompareTag("Breakable")) && rb.velocity.y < 0.1))
                {
                    onMovingPlatform = false;
                    return true;
                }
                else if (castArray[i].collider.CompareTag("platform"))
                {
                    movingPlatform = castArray[i].collider.gameObject.transform;
                    gameObject.transform.SetParent(movingPlatform);
                    onMovingPlatform = true;
                    return true;
                }
                else if (castArray[i].collider.CompareTag("Enemy") && !PH.invulnerable)
                {
                    GameObject child = castArray[i].collider.gameObject;
                    GameObject enemy = child.transform.parent.gameObject;
                    if (enemy.GetComponent<AIDamage>().damagedPrefab != null)
                    {
                        Instantiate(enemy.GetComponent<AIDamage>().damagedPrefab, enemy.transform.position, enemy.transform.rotation);
                    }
                    
                        Destroy(child.transform.parent.gameObject);
                    
                        enemyBounce();
                    onMovingPlatform = false;
                    return false;
                }
            }
            }
        //}
        onMovingPlatform = false;
        return false;
        







  
        
 
        



    }

    void enemyBounce()
    {
    //    Debug.Log("hey");

       

        //it is proportional to falling velocity; maintains proportionality
        bounceHeight = 15f;
        if (Input.GetButton("Jump"))
        {
            gameObject.GetComponent<Move2D>().Jump(bounceHeight, true);
            
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, 0f);
            gameObject.GetComponent<Move2D>().Jump(bounceHeight * 0.75f, true);
        }
        }

    void OnCollisionExit2D(Collision2D col) {
        if (col.collider.tag == "platform") {
            gameObject.transform.SetParent(null);


        }



    }
    
    
}
