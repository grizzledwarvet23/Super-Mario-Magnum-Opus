using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class piranhaPlant : MonoBehaviour
{
    public float emergeDelay;
    private float lastTimeEmerged;

    private bool aiming;
    private bool attacking;

    private Animator anim;

    [SerializeField]
    private ParticleSystem fire;
    //private PolygonCollider2D fireArea;

    public AudioSource flamethrowerSound;

//    private bool collidingWithPlayer;
    private GameObject player;

    private Vector2 startPos;
    private Vector2 endPos;


    public Transform left_middle;
    public Transform right_middle;
    private float rotation;
    //public float vert_middle;

 //   public Vector2[] upPath;
 //   public Vector2[] TLPath;
 //   public Vector2[] TRPath;
 //   public Vector2[] BLPath;
 //    public Vector2[] BRPath;

    private void Start()
    {
        lastTimeEmerged = 0;
        aiming = false;
        attacking = false;
        anim = GetComponent<Animator>();
        player = GameObject.Find("player1");
        if (player == null) { Debug.Log("Player not found"); }
        rotation = transform.parent.eulerAngles.z;
        startPos = new Vector2(0, 0.03f);
        endPos = new Vector2(0, 0.26f);
        transform.localPosition = startPos;

      //  left_middle = transform.position.x - 0.5f;
      //  right_middle = transform.position.x + 0.5f;
      //  vert_middle = transform.position.y + 2;

     //   fireArea = GetComponent<PolygonCollider2D>();

        

        fire.Stop();
    }

    
    private void Update()
    {
        if (Time.timeSinceLevelLoad > lastTimeEmerged + emergeDelay && !(aiming || attacking) && player.transform.position.x > transform.position.x - 23 && player.transform.position.x < transform.position.x + 23 )
        {
            attacking = true;
            StartCoroutine(attack());
            
        }
        if (aiming) //this is active after plant ascends
        {
            //logic for aiming at player before its attack
            Vector2 playerPos = player.transform.position;
            if ( (rotation == 0 && playerPos.x < left_middle.position.x) ||
                 (rotation == 180 && playerPos.x > left_middle.position.x) ||
                 ( (rotation == 90 && playerPos.y < left_middle.position.y ||
                 rotation == 270 && playerPos.y > left_middle.position.y)  ) )  
                {
                    anim.SetInteger("horizontalDir", -1);
                    if ( (rotation == 0 && playerPos.y > left_middle.position.y) ||
                         (rotation == 180 && playerPos.y < left_middle.position.y) || 
                         (rotation == 90 && playerPos.x < left_middle.position.x) || 
                         (rotation == 270 && playerPos.x > left_middle.position.x)) //top left
                    {
                        fire.transform.localEulerAngles = new Vector3(-135, 90, 0);
                        fire.transform.localPosition = new Vector2(-0.124f, 0.108f);
                        anim.SetInteger("verticalDir", 1);
                    //    fireArea.SetPath(0, TLPath);
                    }
                    else //if (playerPos.y <= vert_mi) //bottom left
                    {
                        fire.transform.localEulerAngles = new Vector3(-204, 90, 0);
                        fire.transform.localPosition = new Vector2(-0.124f, -0.023f);
                        anim.SetInteger("verticalDir", -1);
                    //    fireArea.SetPath(0, BLPath);
                    }
                }

            else if ( (rotation == 0 && playerPos.x > right_middle.position.x) ||
                      (rotation == 180 && playerPos.x < right_middle.position.x) ||
                      ((rotation == 90 && playerPos.y > right_middle.position.y || 
                      rotation == 270 && playerPos.y < right_middle.position.y)  )
                      )
                {
                    anim.SetInteger("horizontalDir", 1);
                    if ( (rotation == 0 && playerPos.y > left_middle.position.y) ||
                         (rotation == 180 && playerPos.y < left_middle.position.y) ||
                         (rotation == 90 && playerPos.x < right_middle.position.x) ||
                         (rotation == 270 && playerPos.x > right_middle.position.x)) // top right
                    {
                        fire.transform.localEulerAngles = new Vector3(-135, -90, 0);
                        fire.transform.localPosition = new Vector2(0.127f, 0.117f);
                        anim.SetInteger("verticalDir", 1);
                    //    fireArea.SetPath(0, TRPath);
                    }
                    else //if (playerPos.y <= vert_middle) // bottom right
                    {
                        fire.transform.localEulerAngles = new Vector3(-200, -90, 0);
                        fire.transform.localPosition = new Vector2(0.13f, -0.025f);
                        anim.SetInteger("verticalDir", -1);
                    //    fireArea.SetPath(0, BRPath);
                    }
                }
            else //else, just aim up
            {
                fire.transform.localEulerAngles = new Vector3(-90, 90, 0);
                fire.transform.localPosition = new Vector2(0.029f, 0.199f);
                anim.SetInteger("horizontalDir", 0);
                anim.SetInteger("verticalDir", 0);

            //    fireArea.SetPath(0, upPath);
            }
                
        }
    }
    private IEnumerator attack()
    {
        //first part is ascending out of pipe
        
        for (int i = 0; i < 20; i++)
        {
            transform.localPosition = Vector2.Lerp(startPos, endPos, i / 20.0f);
            yield return new WaitForSeconds(0.02f);
        }
        aiming = true;
        anim.SetBool("aiming", true);
        yield return new WaitForSeconds(1.5f);

        //actual attack
        aiming = false;
        anim.SetBool("aiming", false);
        anim.SetBool("attacking", true);
        yield return new WaitForSeconds(1f);
        fire.Play();
        flamethrowerSound.Play();
        yield return new WaitForSeconds(0.5f);
    //    fireArea.enabled = true;
        //Debug.Log(fireArea.enabled);
        yield return new WaitForSeconds(2.0f);

        //attack ends
        fire.Stop();
        if (flamethrowerSound.isPlaying)
        {
            flamethrowerSound.Stop();
        }
        anim.SetBool("attacking", false);
        anim.SetInteger("horizontalDir", 0);
        anim.SetInteger("verticalDir", 0);
    //    fireArea.enabled = false;
        for (int i = 0; i < 20; i++)
        {
            transform.localPosition = Vector2.Lerp(endPos, startPos, i / 20.0f);
            yield return new WaitForSeconds(0.02f);
        }
        lastTimeEmerged = Time.timeSinceLevelLoad;
        attacking = false;
        
    }

    


}
