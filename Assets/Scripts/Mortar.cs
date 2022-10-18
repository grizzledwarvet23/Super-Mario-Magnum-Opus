using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Mortar : MonoBehaviour
{
    public Sprite sprite;
    public GameObject[] UI;
    public Helicopter helicopter;
    private bool activated = false;

    public GameObject missile;
    public Vector2 missileStart;
    public Vector2 missileEnd;

    public AudioSource explosionSound;
    public Light2D explodeLight;
    public GameObject dialogBox;

    public GameObject ogCam;
    public GameObject tgCam;

    public GameObject[] toDisable;
    public GameObject[] toEnable;

    private GameMaster gm;
    
    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        
        if (gm != null && gm.finishedCutscene)
        {
            GetComponent<Collider2D>().enabled = false;
            helicopter.gameObject.SetActive(false);
            foreach (GameObject obj in toDisable)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in toEnable)
            {
                obj.SetActive(true);
            }
            dialogBox.SetActive(true);
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && !activated)
        {
            activated = true;
            StartCoroutine(activate(col.gameObject));
        }
    }

    private IEnumerator activate(GameObject player)
    {
        PlayerHealth PH = player.GetComponent<PlayerHealth>();
        PH.lockControl(player.GetComponent<Move2D>().facingRight); //make him face left
        PH.invincible = true;
        Animator anim = player.GetComponent<Animator>();
        anim.SetBool("isGrounded", true);
        anim.SetBool("isShooting", false);
        //  col.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.transform.eulerAngles = new Vector3(player.transform.eulerAngles.x, player.transform.eulerAngles.y, 0); //make him face left
        foreach (GameObject obj in UI)
        {
            obj.SetActive(false);
        }

        GetComponent<Animator>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = sprite;
        StartCoroutine(SoundControl.startFade(helicopter.musicSource, 2, 0));

        helicopter.descending = true; //did this so it doesnt continue attacking
        if (helicopter.gunSound.isPlaying) { helicopter.gunSound.Stop(); }
        if (helicopter.attackRoutine != null)
        {
            helicopter.StopCoroutine(helicopter.attackRoutine);
        }

        yield return new WaitUntil(new System.Func<bool>(() => { return !helicopter.getSweeping();  } )); //waits until the helicopter has stopped sweeping
        if (!helicopter.left)
        {
            //if the helicopter just moved to the right, it invokes a right to left movement so that the mortar shot makes sense
            yield return StartCoroutine(helicopter.rightToLeft(false, 0.03f));
        }
        StartCoroutine(SoundControl.startFade(helicopter.helicopterSound, 2, 0));

        missile.SetActive(true);
        helicopter.gameObject.SetActive(false);
        for (int i = 0; i < 40; i++)
        {
            missile.transform.position = Vector2.Lerp(missileStart, missileEnd, i / 39.0f);
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(2);

        explosionSound.Play();
        explodeLight.enabled = true;
        for (int x = 0; x < 25; x++)
        {
            explodeLight.intensity+= 4;
            yield return new WaitForSeconds(0.02f);
        }
        for (int x = 0; x < 50; x++)
        {
            explodeLight.intensity-= 2;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(2);
        dialogBox.SetActive(true); //dialog takes care of it from here
        gm.lastCheckPointPos = new Vector2(947, 0);
        gm.ogCam = ogCam.name;
        gm.tgCam = tgCam.name;
        PH.invincible = false;
    }
}
