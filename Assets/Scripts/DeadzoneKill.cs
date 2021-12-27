using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadzoneKill : MonoBehaviour
{
   






    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy" || col.gameObject.layer == 9 || col.tag == "Shell")
        {
            Destroy(col.gameObject);
        }
        if(col.tag == "Player")
        {
            StartCoroutine(col.gameObject.GetComponent<PlayerHealth>().Die());
        }



    }
}
