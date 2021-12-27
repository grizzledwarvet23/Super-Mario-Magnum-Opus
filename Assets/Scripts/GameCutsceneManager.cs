using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameCutsceneManager : MonoBehaviour
{
    GameObject screenFadeObject;
    Animator screenFadeAnimator;

    public GameObject victoryMario;
    Animator victoryMarioAnimator;

    
    GameObject HUD_elements;
    GameObject player;

    AudioSource victoryTheme;

    public GameObject oldCamera;
    public GameObject bossCamera;

    public string level3PipeName;
    private GameObject level3Pipe;
    public string spaceshipName;
    private GameObject spaceship;

    void Start()
    {
        screenFadeObject = GameObject.Find("BlackFade");
        screenFadeAnimator = screenFadeObject.GetComponent<Animator>(); 
        victoryMarioAnimator = victoryMario.GetComponent<Animator>();
        HUD_elements = GameObject.Find("PlayerUI");
        
   
        player = GameObject.Find("player1");
        
        victoryTheme = gameObject.GetComponent<AudioSource>();
        if(level3PipeName.Length > 0)
        {
            level3Pipe = GameObject.Find(level3PipeName);
        }
        if(spaceshipName.Length > 0)
        {
            spaceship = GameObject.Find(spaceshipName);
        }
        Debug.Log(level3Pipe);
    }



    void fadeWhite() {
        screenFadeAnimator.Play("whiteFade");

    }

    void blackFadeOut()
    {
        screenFadeAnimator.Play("blackFadeOut");
    }

    void exitLevelScene(string sceneToLoad) {
        SceneManager.LoadScene(sceneToLoad);
    
    }

    void disableHUD() {
        for (int x = 0; x < HUD_elements.transform.GetChildCount(); x++)
        {
            if(HUD_elements.transform.GetChild(x).name != screenFadeObject.name)
            {
                HUD_elements.transform.GetChild(x).gameObject.SetActive(false);
            }
        }
    }

    void enableVictoryMario()
    {
        victoryMario.SetActive(true);
        player.SetActive(false);
        if(bossCamera != null)
        {
            oldCamera.SetActive(false);
            bossCamera.SetActive(true);
            
        }

    }

    void setMarioWalk()
    {
     //   victoryMario.GetComponent<Animator>().SetFloat("Speed", 1.0f);
        StartCoroutine(marioTranslate());
    }

    IEnumerator marioTranslate() //for level 3
    {
        victoryMario.GetComponent<Animator>().Play("level3_transition"); // mario walks towards pipe
        victoryTheme.Play();

        yield return new WaitForSeconds(7.9f);
        victoryMario.GetComponents<AudioSource>()[0].Play(); // pipe sound
        yield return new WaitForSeconds(2.1f);


        screenFadeAnimator.Play("blackFadeOut");
        yield return new WaitForSeconds(2f);
        screenFadeAnimator.Play("FadeIn");
        bossCamera.SetActive(false);
        GameObject.Find("CinemachineHolder").transform.GetChild(4).gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        victoryMario.GetComponent<Animator>().Play("level3_transition2");
        victoryMario.GetComponents<AudioSource>()[0].Play(); // pipe sound
        victoryMario.GetComponents<AudioSource>()[1].Play(); // jump sound
        yield return new WaitForSeconds(4.7f);
        victoryMario.GetComponents<AudioSource>()[1].Play(); // jump sound
        yield return new WaitForSeconds(5.3f);
        spaceship.GetComponent<Animator>().Play("spaceship_lift");
        yield return new WaitForSeconds(5);
        screenFadeAnimator.Play("blackFadeOut");
        yield return new WaitForSeconds(3);
        exitLevelScene("Menu");




    }

    void activatePipe()
    {
        level3Pipe.GetComponent<Animator>().Play("pipe_expand");
    }

    void disablePlayerControls() {
      
        player.GetComponent<Move2D>().enabled = false;
      


    }

    void enableVictoryAnimation()
    {
        victoryMarioAnimator.Play("reloading");
        victoryTheme.Play();
    }
}
