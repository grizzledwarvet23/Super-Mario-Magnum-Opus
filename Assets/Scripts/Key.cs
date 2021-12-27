using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Key : MonoBehaviour
{
    public Image keyUI;
    public Sprite fullKey;
    public Animator animator;

    public string animationName;

    public int gameMasterIndex;

    private AudioSource keyGetSound;

    public GameObject lockedDoor;


    private void Awake()
    {
        keyGetSound = gameObject.GetComponent<AudioSource>();
        if(GameObject.Find("GameMaster").GetComponent<GameMaster>().keysToDisable[gameMasterIndex])
        {
            keyUI.sprite = fullKey;
            gameObject.SetActive(false);
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // keyUI.sprite = fullKey;
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Debug.Log(keyGetSound);
                keyGetSound.Play();
            
            if (animationName != null)
            {
                animator.Play(animationName);
            }
            else
            {
                animator.Play("KeyGet");
            }
            GameObject.Find("GameMaster").GetComponent<GameMaster>().keyCount += 1;
            GameObject.Find("GameMaster").GetComponent<GameMaster>().keysToDisable[gameMasterIndex] = true;

            if(lockedDoor.GetComponent<lockedDoor>().neededKeys == GameObject.Find("GameMaster").GetComponent<GameMaster>().keyCount)
            {
                lockedDoor.GetComponent<lockedDoor>().changeQueueSprite();
            }

            StartCoroutine(GameMaster.disableObject(gameObject, 0.5f));
        }
    }

    
}
