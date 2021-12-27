using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public string ability;

    public int healAmount;
  
    void OnCollisionEnter2D(Collision2D col)
    {
//        Debug.Log(col.gameObject);
        if (col.gameObject.name == "player1" || col.gameObject.tag == "Player")
        {
            powerup(col.gameObject);
            Destroy(gameObject);


        }



    }
    void powerup(GameObject player)
    {
        if (ability == "heal" /*&& player.GetComponent<PlayerHealth>().health < 100*/)
        {
            player.GetComponent<PlayerHealth>().takeDamage(-1 * (healAmount));

        }




    }

}
