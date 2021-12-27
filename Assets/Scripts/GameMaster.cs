using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;
    public Vector2 lastCheckPointPos;

    public bool checkPointReached;

    public string ogCam;
    public string tgCam;
    private GameObject cmHolder;
    private int lastLoadedLevel = -1; //dont want to use 0 as default
    private bool hasReloaded = false;

    public bool hasKeys;
 //   public string[] keys;
    public bool[] keysToDisable;
    public int keyCount;

    [System.NonSerialized]
    public Material currentGroundMat;

    public AudioSource currentMusic;

    public bool loadPlayer = false;
    public bool hasSub;
    public bool loadSubmarine;
    public Vector2 subSpawnPos;

    public bool[] nuclearRodsUsed = new bool[2];
    public bool finishedCutscene;

    public int levelNumber;

 //   private bool hasLoaded;



    void Awake()
    {
        if (instance == null)
        {
            
            instance = this;
            DontDestroyOnLoad(instance);
            // hasLoaded = true;
        }
        else {
            if(instance.levelNumber != levelNumber)
            {
//                Debug.Log("Hello");
                instance.hasReloaded = hasReloaded;
                instance.hasKeys = hasKeys;
                instance.keysToDisable = keysToDisable;
                instance.keyCount = keyCount;
                instance.currentGroundMat = currentGroundMat;
                instance.lastCheckPointPos = lastCheckPointPos;
                instance.checkPointReached = checkPointReached;
              //  Debug.Log(currentMusic);
                instance.currentMusic = currentMusic;
                instance.levelNumber = levelNumber;
                instance.hasSub = hasSub;
                instance.subSpawnPos = subSpawnPos;
                instance.ogCam = ogCam;
                instance.tgCam = tgCam;
                instance.loadPlayer = loadPlayer;

                instance.nuclearRodsUsed[0] = nuclearRodsUsed[0];
                instance.nuclearRodsUsed[1] = nuclearRodsUsed[1];
                instance.finishedCutscene = finishedCutscene;
                //COPYING VARIABLES WHEN SCENE CHANGES
                
            }
//            Debug.Log(instance.currentMusic);
            Destroy(gameObject);
         //   Destroy(gameObject);
        }
     


    }

    


    private void OnLevelWasLoaded(int level)
    {
        /** Resets the spawn point and cameras when loading a new level, 
         *  otherwise the player might spawn at a coordinate from a previous level, but in a new level
         */
      //  Debug.Log(lastLoadedLevel == level);
        
        if (!(lastLoadedLevel == level) )
        {
//            Debug.Log("New Level");
            if (GameObject.FindGameObjectWithTag("Player") != null && hasReloaded)
            {
                lastCheckPointPos.x = GameObject.FindGameObjectWithTag("Player").transform.position.x;
                lastCheckPointPos.y = GameObject.FindGameObjectWithTag("Player").transform.position.y;
                ogCam = null;
                tgCam = null;
            }
       //     enteredNewScene = true;
            hasReloaded = false;
        }
        else
        {
            hasReloaded = true;
        }
    
        lastLoadedLevel = level;
        

    }

    public void cameraReset()
    {
        //this is being called in Move2D

        /** When the game resets,
    * it activates the "current camera"
    * (current camera is decided in Pipe script when player
    * gets a checkpoint)
    * 
    */

        cmHolder = GameObject.Find("CinemachineHolder");
        foreach (Transform item in cmHolder.transform)
        {

            if (item.name.Equals(ogCam))
            {
                item.gameObject.SetActive(false);
            }
            if (item.name.Equals(tgCam))
            {
                item.gameObject.SetActive(true);
            }
        }

    }

    //Static method, since this could be useful in any game object. Disables object after a time
    public static IEnumerator disableObject(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }

    //Like disableObject, but destroys
    public static IEnumerator destroyObject(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(obj);
    }



    //activates one object, disables another
    /* @param a the GameObject to activate
     * @param d the GameObject to deactivate
     */
    public static void enableAndDisable(GameObject a, GameObject d)
    {
        a.SetActive(true);
        d.SetActive(false);
    }

    public static void enableObject(GameObject obj)
    {
        obj.SetActive(true);
    }















}
