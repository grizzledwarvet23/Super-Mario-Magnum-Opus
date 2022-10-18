using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWheelTrigger : MonoBehaviour
{
    public PanelSwitcher switcher;
    public GameObject[] colorWheel;
    public AudioSource collectSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            switcher.on = true;
            collectSound.Play();
            foreach(GameObject obj in colorWheel)
            {
                obj.SetActive(true);
            }
            Destroy(gameObject);
        }
        
    }
}
