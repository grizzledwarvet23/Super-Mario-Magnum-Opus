using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class nuclearPassage : MonoBehaviour
{
    public GameObject leftHole;
    public GameObject rightHole;

    public Animator liftAnim;

    public GameObject nuclearGauge;
    public GameObject nuclearNeedle;
    public Image radiationEffect;
    private Color originalRadiationColor;

    private GameMaster gm;

    public GameObject leftRod;
    public GameObject rightRod;

    public GameObject originalCam;
    public GameObject targetCam;
    public GameObject blackFade;
    public GameObject snowEffector;

    public Vector2 location;

    public AudioSource liftSound;
    public AudioSource[] tracks = new AudioSource[2];
    public GameObject boss;
    private void Start()
    {
        originalRadiationColor = radiationEffect.color;
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();

        if (gm.nuclearRodsUsed[0] && gm.nuclearRodsUsed[1])
        {
            leftHole.SetActive(true);
            rightHole.SetActive(true);
            leftRod.SetActive(false);
            rightRod.SetActive(false);
            liftAnim.enabled = true;
            liftSound.Play(); 
        }
        else
        {
            if (gm.nuclearRodsUsed[0]) //Indicating the left rod slot
            {
                leftHole.SetActive(true);
                leftRod.SetActive(false);
            }
            else if (gm.nuclearRodsUsed[1])
            {
                rightHole.SetActive(true);
                rightRod.SetActive(false);
            }

            liftAnim.enabled = false;
        }
        
           
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject player = collision.gameObject;
            if (player.GetComponentInChildren<nuclearRod>() != null)
            {
                GameObject rod = player.GetComponentInChildren<nuclearRod>().gameObject;
                if (rod.name == "nuclear rod (1)") // from the submarine, which is on the left
                {
                    leftHole.SetActive(true);
                    gm.nuclearRodsUsed[0] = true;
                    gm.loadSubmarine = false;
                    gm.tgCam = originalCam.name;
                    gm.lastCheckPointPos = new Vector2(-4.22f, 5.98f); 
                }
                else if (rod.name == "nuclear rod")
                {
                    rightHole.SetActive(true);
                    gm.nuclearRodsUsed[1] = true;
                    gm.lastCheckPointPos = new Vector2(-4.22f, 5.98f);
                }
                Destroy(player.GetComponentInChildren<nuclearRod>().gameObject);
                radiationEffect.color = originalRadiationColor;


                nuclearGauge.SetActive(false);
                nuclearNeedle.transform.rotation = new Quaternion(0, 0, 0, 0);
                nuclearNeedle.SetActive(false);

                if (rightHole.activeSelf && leftHole.activeSelf)
                {
                    liftAnim.enabled = true;
                    liftAnim.Play("nuclear_passage_lift");
                    liftSound.Play();
                }
            }
        }
    }

    public void transportPlayer(GameObject player)
    {
        player.transform.position = location;
        //player.GetComponent<PlayerHealth>().unlockControl();
        player.GetComponent<SpriteRenderer>().sortingOrder = 0;
        originalCam.SetActive(false);
        targetCam.SetActive(true);
        snowEffector.SetActive(false);
        blackFade.SetActive(false);
        blackFade.SetActive(true);
        SoundControl.trackShift(tracks);
        boss.SetActive(true);



    }

    

}
