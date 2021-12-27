using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineIntro : MonoBehaviour
{
    private void OnEnable()
    {
        gameObject.GetComponent<Animator>().enabled = true;
        gameObject.GetComponent<Animator>().Play("submarine_intro");

    }

    public void stopAnimation()
    {
 //       Debug.Log("LOL");
        gameObject.GetComponent<Animator>().enabled = false;
    }
}
