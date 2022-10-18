using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableWhenVisible : MonoBehaviour
{
    public MonoBehaviour[] components;
    public GameObject[] objects;

    private void OnBecameInvisible()
    {
  ///      Debug.Log("Invisible");
   //     foreach (GameObject obj in objects)
     //   {
       //     obj.SetActive(false);
       // }
        /*
        foreach (MonoBehaviour c in components)
        {
            c.enabled = false;
        }
        */
    }

    private void OnBecameVisible()
    {
  //      Debug.Log("Visible");
   //     foreach (GameObject obj in objects)
     //   {
         //   obj.SetActive(true);
       // }
        /*
        foreach (MonoBehaviour c in components)
        {
            c.enabled = true;
        }
        */
    }
}
