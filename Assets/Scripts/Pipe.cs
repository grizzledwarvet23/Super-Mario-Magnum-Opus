using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Tilemaps;

public class Pipe : MonoBehaviour
{
    // Start is called before the first frame update
    public float warp_x;
    public float warp_y;

    public float customSpawnPoint_x;
    public float customSpawnPoint_y;


    public GameObject blackFade;
    bool canWarp;
    float rotation;

    public GameObject originalCam;
    public GameObject targetCam;

    public AudioSource pipeSound;

    GameObject player;
    public GameObject[] objectsToEnable;
    public GameObject[] objectsToDisable;

    private GameMaster gm;

    public Tilemap map;
    public Material newMapMat;

    public bool changeRotation;
    public Vector3 angles;

    public GameObject submarine;
    public GameObject stopsEntry; //if the player has this object, it halts the ability to enter the pipe
  //  public CinemachineVirtualCamera vcam;
    // Update is called once per frame
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        canWarp = false;
        rotation = transform.parent.gameObject.transform.rotation.eulerAngles.z;
        player = GameObject.Find("player1");

        if(gm.loadSubmarine && submarine != null)
        {
            submarine.SetActive(true);
        ///    Debug.Log("Hey!");
            //submarine.GetComponent<SubmarineIntro>().stopAnimation();
            submarine.GetComponent<SubmarineIntro>().stopAnimation();
            submarine.GetComponentInChildren<Submarine>().introDone = true;
          //  submarine.GetComponentInChildren<SubmarineIntro>().activateParent();
            activateAndDeactivate(true);

        }
        //backgroundScript = GameObject.Find("Background").GetComponent<backgroundSet>();
    }
    void Update()
    {

        if (Input.GetAxisRaw("Vertical") == -1 && canWarp && rotation == 0)
        {

            pipeActions();

        }
        else if (Input.GetAxisRaw("Horizontal") == 1 && canWarp && rotation == 90)
        {

            pipeActions();
        }
        else if (Input.GetAxisRaw("Vertical") == 1 && canWarp && rotation == 180)
        {

            pipeActions();
        }
        else if (Input.GetAxisRaw("Horizontal") == -1 && canWarp && rotation == 270)
        {

            pipeActions();

        }
    }

    void pipeActions() {
        blackFade.SetActive(false);
        blackFade.SetActive(true);
        player.transform.position = new Vector3(warp_x, warp_y, player.transform.position.z);
        if(changeRotation)
        {
            player.transform.eulerAngles = angles;
        }

        if (!(customSpawnPoint_x < 1 && customSpawnPoint_x > -1))
        {
            gm.lastCheckPointPos = new Vector2(customSpawnPoint_x, customSpawnPoint_y);
            gm.ogCam = originalCam.name;
            gm.ogCam = targetCam.name;
        }
        else
        {
            gm.lastCheckPointPos = new Vector2(warp_x, warp_y);
            gm.ogCam = originalCam.name;
            gm.tgCam = targetCam.name;
        }
        gm.checkPointReached = true;

        originalCam.SetActive(false);
        targetCam.SetActive(true);






        pipeSound.Play();
        activateAndDeactivate(false);;
        

        

        if(map != null && newMapMat != null)
        {
            lockedDoor.changeTilemapMat(map, newMapMat);
            gm.currentGroundMat = newMapMat;
        }

        if (gm.hasSub)
        {
            if (GameObject.Find("submarine") != null)
            {
                gm.lastCheckPointPos = new Vector2(-218.2f, 54.3f);
                gm.loadSubmarine = true;
            }
            else if (player.transform.GetChild(player.transform.GetChildCount() - 1).name == "nuclear rod (1)")
            {
                gm.lastCheckPointPos = new Vector2(-218.2f, 54.3f);
                gm.loadSubmarine = true;
                gm.tgCam = originalCam.name;
                gm.ogCam = targetCam.name;
            }
            else
            {
                gm.loadSubmarine = false;
            }
        }

         //   objectToEnable.SetActive(true);




    }

    void activateAndDeactivate(bool activatedFromStart)
    {
        foreach (GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
            if (obj.name == "submarine_intro" && player.GetComponentInChildren<nuclearRod>() != null)
            {
                //   Debug.Log("yup");
                if (!activatedFromStart)
                {
                    obj.GetComponentInChildren<Submarine>().receiveRod(player.GetComponentInChildren<nuclearRod>());
                    obj.transform.GetChild(0).transform.localPosition = new Vector2(0, 0.025f);
                }
                else
                {
                    submarine.GetComponent<SubmarineIntro>().stopAnimation();
                    submarine.GetComponentInChildren<Submarine>().introDone = true;
                }

            }
            
        }

        foreach (GameObject obj in objectsToDisable)
        {
            if (obj.name == "submarine_intro")
            {
                obj.GetComponentInChildren<Submarine>().transferRod();
            }
            obj.SetActive(false);
        }
        

    }


    void OnTriggerEnter2D(Collider2D player)
    {
        
        if (player.tag == "Player")
        {
//            Debug.Log( (stopsEntry != null) + " " + (stopsEntry.transform.parent != player));
            if (stopsEntry == null || (stopsEntry != null && (stopsEntry.transform.parent == null || stopsEntry.transform.parent.tag != "Player" ) ) )
            {
                canWarp = true;
            }
         
                


            }

        }
    void OnTriggerExit2D(Collider2D player)
    {
        if (player.tag == "Player")
        {
            canWarp = false;
        }
        }

    



    }





