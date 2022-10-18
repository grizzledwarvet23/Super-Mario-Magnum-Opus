using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class lockedDoor : MonoBehaviour
{
    GameObject player;
    GameMaster gm;

    

    [SerializeField]
    private Vector2 coords;

    private bool atDoor;

    [SerializeField]
    private bool changeBackground;

    public GameObject newBackground;
    public GameObject currentBackground;

    public GameObject ogCam;
    public GameObject tgCam;

    public GameObject blackFade;

    [SerializeField]
    private Material lightingMat;
    [SerializeField]
    private Tilemap groundMap;

    [SerializeField]
    private int musicToPlayComponentNum;
    [SerializeField]
    private int musicToStopComponentNum;
  //  [SerializeField]
    //private bool playMusic;

    [SerializeField]
    public GameObject buttonQueue;

    public GameObject queueText;

    [SerializeField]
    private Sprite openableSprite;
    
    public int neededKeys;

    private bool unlocked;

    public AudioSource defaultMusic;

    public bool controlsMusic;

    private void Start()
    {
        player = GameObject.Find("player1");
        gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        neededKeys = 2;
        if(gm.keyCount == neededKeys)
        {
            unlocked = true;

            changeQueueSprite();
            
        }
        /*this is because, in level 2, if u died during the dark dungeon area, then it would reset the tilemap and make it all bright
         * rather than stay dark
        */
        if(gm.currentGroundMat != null)
        {
            changeTilemapMat(groundMap, gm.currentGroundMat);
        }


        if(controlsMusic && gm.currentMusic != null && !gm.currentMusic.isPlaying )
        {
            gm.currentMusic.Play();
        }
        


    }

    private void Update()
    {
        if(!GameObject.Find("PlayerUI").GetComponent<PauseMenu>().isPaused && 
            atDoor && 
            GameObject.Find("player1").GetComponent<Move2D>().isGrounded && 
            (Input.GetKeyDown(KeyCode.O) || Input.GetButtonDown("Fire1")) && 
            gm.keyCount == neededKeys)
        {
            player.transform.position = new Vector3(coords.x, coords.y, player.transform.position.z);
            gm.lastCheckPointPos = coords;

            if (changeBackground)
            {
                GameMaster.enableAndDisable(newBackground, currentBackground);
            }

            ogCam.SetActive(false);
            tgCam.SetActive(true);

            blackFade.SetActive(false);
            blackFade.SetActive(true);


            gm.ogCam = this.ogCam.name;
            gm.tgCam = this.tgCam.name;

            if(groundMap != null && lightingMat != null)
            {
                gm.currentGroundMat = lightingMat;
                changeTilemapMat(groundMap, lightingMat);

            }
            if(musicToPlayComponentNum != -1)
            {

                GameObject.Find("Music").GetComponents<AudioSource>()[musicToPlayComponentNum].enabled = true; //MUSIC IN LEVEL 2 PRESERVES ON LEVEL RELOAD (AKA ITS LIKE GAMEMASTER)
                gm.currentMusic = GameObject.Find("Music").GetComponents<AudioSource>()[musicToPlayComponentNum];
            }
            if(musicToStopComponentNum != -1)
            {
                GameObject.Find("Music").GetComponents<AudioSource>()[musicToStopComponentNum].enabled = false;
            }


        }
    }

    public void changeQueueSprite()
    {
        if (buttonQueue != null)
        {
            buttonQueue.GetComponent<SpriteRenderer>().sprite = openableSprite;
        }
        unlocked = true;
    }

    public static void changeTilemapMat(Tilemap map, Material mat)
    {
        map.GetComponent<TilemapRenderer>().material = mat;
        //whenever calling this method, make sure to change gamemaster.currentgroundmat to the material 
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        atDoor = true;

        if(queueText!= null && unlocked)
        {
            queueText.SetActive(true);
        }
        if(buttonQueue!= null)
        {
            buttonQueue.GetComponent<Animator>().Play("increaseOpacity");  
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        atDoor = false;

        if (queueText != null && unlocked)
        {
            queueText.SetActive(false);
        }
        if (buttonQueue != null)
        {
            buttonQueue.GetComponent<Animator>().Play("decreaseOpacity");
        }
    }
}
