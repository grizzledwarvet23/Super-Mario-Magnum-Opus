using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStandsTrigger : MonoBehaviour
{
    private bool playerStanding;
    public GameObject player;
    public Animator liftAnim;

    public AudioSource liftSound;
    public AudioSource currentMusic;

    private bool running;
    //WHEN PLAYER STANDS ON SOMETHING

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player")
        {
            
            //Debug.Log("Hello!!");
            GameObject player = collision.gameObject;
            if (player.GetComponent<GroundCheck>().checkIfGrounded())
            {
                
                playerStanding = true;
                if (!running)
                {
                    StartCoroutine(lower());
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerStanding = false;
        }
    }

    IEnumerator lower()
    {
        running = true;
        yield return new WaitForSeconds(0.85f);
        if (playerStanding)
        {
            playerStanding = false;

            
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.transform.position = new Vector2(-3.927828f, 12.13071f);
            player.GetComponent<Move2D>().isGrounded = true;
            player.GetComponent<Move2D>().attacking = false;
            if (!player.GetComponent<Move2D>().facingRight)
            {
                player.GetComponent<PlayerHealth>().lockControl(true);
            }
            else
            {
                player.GetComponent<PlayerHealth>().lockControl(false);
            }

            player.GetComponent<SpriteRenderer>().sortingOrder = -3;
            liftAnim.Play("nuclear_passage_unlift");
            if (liftSound != null && !liftSound.isPlaying)
            {
                liftSound.Play();
                StartCoroutine(SoundControl.startFade(liftSound, 2f, 0));
                StartCoroutine(SoundControl.startFade(currentMusic, 3.5f, 0));
            }

            yield return new WaitForSeconds(4f); //replace with the length of nuclear_passage_unlift
            transform.parent.parent.gameObject.GetComponent<nuclearPassage>().transportPlayer(player);

        //    yield return new WaitForSeconds()
         // transform.parent.parent.gameObject.GetComponent<nuclearPassage>().transportPlayer(player);
        }
        running = false;
    }

    
}
