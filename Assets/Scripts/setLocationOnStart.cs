using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setLocationOnStart : MonoBehaviour
{
    [SerializeField]
    private Vector3 coords;

 //   [SerializeField]
 //   private GameObject lightToEnable;


    
    void Start()
    {
        transform.localPosition = coords;
    }

    /*
    private void OnDisable()
    {
        //used because spotlight in level 2 in dungeon area usually goes away when Mario dies. I didn't want that to happen.
        if(gameObject.GetComponent<Light>() != null)
        {
            GameMaster.enableObject((GameObject) lightToEnable);

        }
    }
    */

    

}
