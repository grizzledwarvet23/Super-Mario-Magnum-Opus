using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotate : MonoBehaviour
{

    private Transform player;
    public float XOffset;
    public float YOffset;

    


    // Start is called before the first frame update
    void Start()
    {
        
       // player = GameObject.Find("player1").transform;
    }

    // Update is called once per frame
    void Update()
    {/*
        if (transform.position.y <= -0.272456f)
        {
            gameObject.GetComponent<CinemachineVirtualCamera>().Follow = null;
        }

        */
        /*
        if (player != null)
        {
           // transform.position = new Vector3(player.position.x + XOffset, player.position.y + YOffset, player.position.z - 10);
            transform.position = new Vector3(player.position.x + XOffset, -1.55f, player.position.z - 10);
        }
        */
        }
}
