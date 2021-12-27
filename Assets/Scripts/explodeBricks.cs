using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explodeBricks : MonoBehaviour
{
    void Start()
    {
        
        StartCoroutine(GameObject.Find("Breakables").GetComponent<BreakAnimation>().destroyMultiple(gameObject.GetComponent<Collider2D>()));
        
    }

}
