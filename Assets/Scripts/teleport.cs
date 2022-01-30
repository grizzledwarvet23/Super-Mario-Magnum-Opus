using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**this class is basically the pipe warp function, 
but I needed to create it so I could use it in level 4
by itself
**/
public class teleport : MonoBehaviour
{
    [SerializeField]
    private Vector2 teleportPos;

    [SerializeField]
    private Vector2 respawnPos;

    public GameObject targetCam;
    public GameObject originalCam;
    public GameObject dialogBox;

    public GameObject[] toDisable;
    public GameObject[] toEnable;



    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.transform.position = teleportPos;
            GameMaster gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
            gm.lastCheckPointPos = respawnPos;
            gm.checkPointReached = true;
            gm.ogCam = "CinemachineBrain (LiftReal)";
            gm.tgCam = "CinemachineBrain (Boss)";

            col.GetComponent<Animator>().SetBool("isGrounded", true);
            col.GetComponent<Animator>().SetBool("isShooting", false);
            col.GetComponent<PlayerHealth>().lockControl(!col.GetComponent<Move2D>().facingRight);
            col.transform.eulerAngles = new Vector3(0, 0, 0);
            GameMaster.enableAndDisable(targetCam, originalCam);
            dialogBox.SetActive(true);
            StartCoroutine(loadObjects());
            
        }
    }

    private IEnumerator loadObjects()
    {
        foreach (GameObject obj in toDisable)
        {
            if (obj.tag == "platform" && obj.transform.Find("player1"))
            {
                obj.transform.Find("player1").SetParent(null);
            }
            obj.SetActive(false);
        }
        yield return new WaitForSeconds(0.05f);
        foreach (GameObject obj in toEnable)
        {
            obj.SetActive(true);
        }
        
    }
}
