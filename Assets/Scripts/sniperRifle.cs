using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sniperRifle : MonoBehaviour
{

    public BooSniper parentScript;
    public Sprite defaultSprite;
    SpriteRenderer renderer;
    AudioSource shotSound;
    // Start is called before the first frame update
    void Start()
    {
        renderer = gameObject.GetComponent<SpriteRenderer>();
        shotSound = gameObject.GetComponent<AudioSource>();
    }



    public void teleportParent()
    {
        //renderer.color = new Vector4(renderer.color.r, renderer.color.g, renderer.color.b, 0);
        //   renderer.sprite = defaultSprite;
        //gameObject.GetComponent<Animator>().Play("sniper_fadeout");
        transform.parent.transform.position = new Vector2(0, 0);
        StartCoroutine(parentScript.initiateAttack(renderer, false));
    }

    public void initialTeleportParent()
    {
        //renderer.color = new Vector4(renderer.color.r, renderer.color.g, renderer.color.b, 0);
        //   renderer.sprite = defaultSprite;
        //gameObject.GetComponent<Animator>().Play("sniper_fadeout");
        transform.parent.transform.position = new Vector2(0, 0);
        StartCoroutine(parentScript.initiateAttack(renderer, true));
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().takeDamage(50);
        }

        else if(collision.gameObject.name == "bone platform (boss)")
        { //when hits bone platform
            gameObject.GetComponent<Animator>().Play("sniperShot_blocked");
        }

    }

    public void playSound()
    {
        shotSound.Play();
    }

}
