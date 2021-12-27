using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollection : MonoBehaviour
{
    public int items = 0;
    private GameObject formerCoinName;


    // Start is called before the first frame update



    void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log(collision.gameObject.name + " " + formerCoinName);
        if (collision.gameObject.tag == "Coin")
        {
            coinCollect(collision);
        }
        if (collision.gameObject.name == "Bullet(Clone)" || collision.gameObject.tag == "Projectile")
        {
            gameObject.GetComponent<PlayerHealth>().takeDamage(collision.gameObject.GetComponent<Bullet>().damage);
            if(collision.gameObject.GetComponent<Bullet>().effectOnDeath != null)
            {
                collision.gameObject.GetComponent<Bullet>().instantiateEffect();
            }
            Destroy(collision.gameObject);
        }

    }
    IEnumerator deleteCoin(GameObject coin)
    {
        yield return new WaitForSeconds(0.75f);
        Destroy(coin);
    }

    public void coinCollect(Collider2D collision)
    { //moved from ontriggerenter2D
        if (collision.gameObject != formerCoinName)
        {
            formerCoinName = collision.gameObject;

            collision.gameObject.GetComponent<Animator>().Play("coin_sparkle");
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;

            StartCoroutine(deleteCoin(collision.gameObject));

            gameObject.GetComponent<SoundControl>().CoinPlay();

            items++;



        }
    }
}
