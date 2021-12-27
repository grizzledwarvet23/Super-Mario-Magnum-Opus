using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour
{
    public LayerMask mask;
    private bool test; //MAKES SURE THAT IT ONLY DOES ONCE
    public int hammerDamage;

    public HammerBro bossScript;

    public void Update()
    {
        Collider2D hammerRange = Physics2D.OverlapCircle(transform.position, 3.7f, mask); //12 IS THE PLAYER LAYER
     //   Debug.Log(hammerRange);
        if (hammerRange != null && !test)
        {
            test = true;
            //    Debug.Log("hey");



            GameObject player = hammerRange.gameObject;
            if (!bossScript.facingRight) //facing the left
            {
                player.GetComponent<Move2D>().Knockback(new Vector2(-20, 37), 0.3f);
            }
            else if (bossScript.facingRight)
            {  
                player.GetComponent<Move2D>().Knockback(new Vector2(20, 37), 0.3f);
            }

            player.GetComponent<PlayerHealth>().takeDamage(hammerDamage);

                
            
         //   this.enabled = false;
        }
    }

    void OnEnable()
    {
        test = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 3.7f);
    }

}
