using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BooSniper : MonoBehaviour, BossInterface
{
    public Vector3[] locations;

    private int lastChoice;
    private GameObject player;
    SpriteRenderer renderer;
    Animator animator;
    public Animator sniperAnimator;

    //same as Lakitu script
    // public AudioSource[] trackShift;
    public int[] trackShiftIndices; //the one at 0 should be the component index of dungeon, the one at 1 should be component index of boo theme
                                    //  public SoundControl soundScript;
    private GameObject music;
    public GameObject healthbar;

    public AudioSource laughSound;

    private int countAttack = 0;
    private int countReveal = 0;


    [System.Serializable]
    public struct objectOpacityPair
    {
        public GameObject obj;
        public float opacity;
    }
    public objectOpacityPair[] objectsToChangeOpacity;


    private void OnEnable()
    {
        foreach(objectOpacityPair pair in objectsToChangeOpacity)
        {

            if (pair.obj.GetComponent<SpriteRenderer>() != null)
            {
               SpriteRenderer spr = pair.obj.GetComponent<SpriteRenderer>();
                spr.color = new Vector4(spr.color.r, spr.color.g, spr.color.b, pair.opacity / 255.0f);
            }
            else if(pair.obj.GetComponent<Image>() != null)
            {
                Image spr = pair.obj.GetComponent<Image>();
                spr.color = new Vector4(spr.color.r, spr.color.g, spr.color.b, pair.opacity / 255.0f);
            }
            else if(pair.obj.GetComponent<TextMeshProUGUI>() != null)
            {
                TextMeshProUGUI spr = pair.obj.GetComponent<TextMeshProUGUI>();
                spr.color = new Vector4(spr.color.r, spr.color.g, spr.color.b, pair.opacity / 255.0f);
            }
            
        }

       GameObject.Find("Music").GetComponents<AudioSource>()[trackShiftIndices[0]].enabled = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        lastChoice = -1;

        renderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        music = GameObject.Find("Music");
        player = GameObject.Find("player1");
    }

    //BossInterface method
    public void onDamageTaken()
    {
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("boo_taunt"))
        {
            animator.Play("boo_stunned");
            StartCoroutine(initiateAttack(null, 3));
        }
    }

    public IEnumerator initiateAttack(SpriteRenderer childRenderer, bool initial) //referenced in sniperRifle script
    {
        //renderer.color = new Vector4(renderer.color.r, renderer.color.g, renderer.color.b, 0);
        if (initial)
        {
            animator.Play("boo_initialfadeout");
        }
        else
        {
            animator.Play("boo_fadeout");
        }


       
        yield return new WaitForSeconds((int) Random.Range(1, 4));
        StartCoroutine(teleport(childRenderer));
        

    }

    public IEnumerator initiateAttackOG() //referenced in sniperRifle script
    {
        //renderer.color = new Vector4(renderer.color.r, renderer.color.g, renderer.color.b, 0);
    //    yield return new WaitForSeconds(t);
        animator.Play("boo_fadeout");




        yield return new WaitForSeconds((int)Random.Range(1, 4));
        StartCoroutine(teleport(null));


    }

    //OVERLOADED VERSION OF THE ONE ABOVE
    public IEnumerator initiateAttack(SpriteRenderer childRenderer, float t) //referenced in sniperRifle script
    {
        //renderer.color = new Vector4(renderer.color.r, renderer.color.g, renderer.color.b, 0);
        yield return new WaitForSeconds(t);
        animator.Play("boo_fadeout");




        yield return new WaitForSeconds((int)Random.Range(1, 4));
        StartCoroutine(teleport(childRenderer));


    }

    public IEnumerator teleport(SpriteRenderer childRenderer)
    {
        if(childRenderer == null)
        {
            childRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
        }
        //locations should have length 6. Randomly chooses, tries not to do same one
        int index = (int)Random.Range(0, 6);
        if(index == lastChoice)
        {
            index = (int)Random.Range(0, 6);
            if(index == lastChoice)
            {
                index = (int)Random.Range(0, 6);
            }
        }
        
        lastChoice = index;
        

        Vector3 choice = locations[index];
        transform.parent.position = new Vector2(choice.x, choice.y);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, choice.z, transform.eulerAngles.z);




        //    renderer.color = new Vector4(renderer.color.r, renderer.color.g, renderer.color.b, 0.298f);
        animator.Play("boo_fadein");
        sniperAnimator.Play("sniper_fadein");
        yield return new WaitForSeconds(Random.Range(0.9f, 1.2f));

  //      childRenderer.color = new Vector4(renderer.color.r, renderer.color.g, renderer.color.b, 1);

        //range is (inclusive, exclusive)
        int chanceShoot = Random.Range(0, 100);
        if ( (chanceShoot >= 28 && countAttack < 4) || countReveal > 2 )//count ensures that it doesnt go too long without a vulnerability phase
        {
            countReveal = 0;
            countAttack++;
            sniperAnimator.Play("SniperShot");
        }
        else
        {
            countAttack = 0;
            countReveal++;
            gameObject.GetComponent<Animator>().Play("boo_taunt");
            sniperAnimator.Play("sniper_fadeout");
        }
      

    }

    public void laugh()
    {
        laughSound.Play();
    }

    void enableHealthbar()
    {
        healthbar.SetActive(true);
    }

    
    //referenced as an event in the intro animation of the boo
    public void setMusic()
    {

        music.GetComponents<AudioSource>()[trackShiftIndices[1]].enabled = true;
        music.GetComponents<AudioSource>()[trackShiftIndices[1]].volume = 0;
        StartCoroutine(SoundControl.startFade(music.GetComponents<AudioSource>()[trackShiftIndices[1]], 5, 0.48f));
    }

    /** Below methods used in "boo intro" 
     * 
     */
    public void lockPlayer()
    {
        player.GetComponent<Animator>().SetBool("isGrounded", true);
        player.GetComponent<PlayerHealth>().lockControl();
    }
    public void unlockPlayer()
    {
        player.GetComponent<PlayerHealth>().unlockControl();
    }


  

   


}
