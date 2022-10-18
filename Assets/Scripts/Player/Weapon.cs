using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Weapon : MonoBehaviour
{
    public Transform firePoint;
    public Move2D moveScript;
  //  private float delay;
    public Animator animator;
    public int damage = 50;
   // public bool readyForShot;
    public float range;
    public float recoilForce;
    public int recoilDirection;
    public Rigidbody2D rb;
    public LayerMask hitLayer;
    public int Ammo;
    public float[] directionInput = new float[2];
    private bool canChangeAerialDir;
    public bool doneResetting = true;
    public int MagSize;
    public bool recoilFinished;
    public bool animationDone;
    private float lastTimeShot;
    public float recoilDuration;
    private float ogRecoilDuration; //added for icelevel.

    public slipperyMovement slipScript;

    private SpriteRenderer renderer;
    public Color outOfAmmoColor;

    public AudioSource noDamageSound;

    public GameObject hitEffect;

    public PanelSwitcher switcher; //flips everytime you shoot. used for PanelSwitcher on level 4

    public LineRenderer lineRenderer;
    public bool lightningOn = false; //lightning effect in level 4

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
        Ammo =  MagSize;
        ogRecoilDuration = recoilDuration;
        canChangeAerialDir = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (slipScript != null && slipScript.onIce)
        {
            recoilDuration = 1; //TUNE THIS
        }
        else
        {
            recoilDuration = ogRecoilDuration;
        }
        checkRecoilFinished();
        if (moveScript.isGrounded)
        {
            Ammo = MagSize;
            if (!lightningOn)
            {
                renderer.color = Color.white;
            }
            else if (renderer.color.r >= 0.7f || renderer.color.Equals(outOfAmmoColor))
            {
                renderer.color = new Color(0.7f, 0.7f, 0.7f);
            }
        }
        


        //Debug.DrawRay(firePoint.position, firePoint.right);
        if ( (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.P)) && !(animator.GetCurrentAnimatorStateInfo(0).IsName("Aerial_Shoot") || animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot") || animator.GetCurrentAnimatorStateInfo(0).IsName("Upward_Shoot") || animator.GetCurrentAnimatorStateInfo(0).IsName("Downward_Shoot")) && Ammo > 0)
        {


            recoilDirection = -1;
            // readyForShot = false;
            canChangeAerialDir = true;
            
            directionInput[0] = Input.GetAxisRaw("Horizontal");
            directionInput[1] = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(KeyCode.P))
            {
                directionInput[0] = 0;
                directionInput[1] = -1;
            }
            

            moveScript.fireAnimation();
            moveScript.isJumping = false;
            if (!moveScript.isGrounded)
            {


                //    transform.Rotate(0f, 0f, 90f * directionInput[1]);
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90f * directionInput[1]);
                

                if (directionInput[1] == 1)
                {
                    gameObject.transform.GetChild(2).gameObject.transform.Rotate(0f, 0f, -90f * directionInput[1]); // x = 0.1651, y = -0.05
                    gameObject.transform.GetChild(2).gameObject.transform.localPosition = new Vector3(0.1651f, -0.05f, gameObject.transform.GetChild(2).gameObject.transform.position.z);
                    //      gameObject.transform.GetChild(2).gameObject.transform.localPosition = new Vector3(0.181f, -0.096f, gameObject.transform.GetChild(2).gameObject.transform.position.z); - how it was before
                }
                else if (directionInput[1] == -1)
                {
                 
                    gameObject.transform.GetChild(2).gameObject.transform.Rotate(0f, 0f, 90f * directionInput[1]); // x = -0.192, y = -0.05
                    gameObject.transform.GetChild(2).gameObject.transform.localPosition = new Vector3(-0.192f, -0.05f, gameObject.transform.GetChild(2).gameObject.transform.position.z);
                    //     gameObject.transform.GetChild(2).gameObject.transform.localPosition = new Vector3(-0.137f, -0.096f, gameObject.transform.GetChild(2).gameObject.transform.position.z); - how it was before
                }
                else
                {
                    gameObject.transform.GetChild(2).gameObject.transform.localPosition = new Vector3(0f, 0.016f, gameObject.transform.GetChild(2).gameObject.transform.position.z);
                }
            
            
            
            }
            else
            {
                animator.SetInteger("VertAttackDir", (int)directionInput[1]);
                InvokeRepeating("correctRotation", 0f, 0.05f);
            }

            
             Invoke("Shoot", 0.4f);
            

        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Aerial_Shoot"))
        {
            if(Input.GetAxisRaw("Vertical") != 0 && canChangeAerialDir)
            {
                if (directionInput[1] == 0)
                {
                    directionInput[1] = Input.GetAxisRaw("Vertical");
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, directionInput[1] * 90);
                }
                
            }
            
            
            
        }

    



    }




    void Shoot()
    {

        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot") || animator.GetCurrentAnimatorStateInfo(0).IsName("Aerial_Shoot") || animator.GetCurrentAnimatorStateInfo(0).IsName("Upward_Shoot") || animator.GetCurrentAnimatorStateInfo(0).IsName("Downward_Shoot"))
        {

            
            gameObject.GetComponent<SoundControl>().FirePlay();

            //   RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, firePoint.right, range, hitLayer);
            RaycastHit2D hitInfo = Physics2D.CircleCast(firePoint.position, 0.5f, firePoint.right, range, hitLayer, -Mathf.Infinity, Mathf.Infinity);



            if (hitInfo)
            {
                

                AIDamage enemy = hitInfo.transform.GetComponent<AIDamage>();
                if (enemy != null && enemy.isActiveAndEnabled)
                {


                    if ((hitInfo.transform.parent != null && hitInfo.transform.parent.name.Contains("Koopas")))
                    {
                        if (!(Mathf.Abs((int)transform.eulerAngles.y - (int)hitInfo.transform.eulerAngles.y) == 180))
                        {
                            noDamageSound.Play();
                            enemy.playHitEffect();
                        }
                        else
                        {
                            enemy.TakeDamage(damage);
                        }

                    }
                    else if (enemy.name == "Sledge_Boss" && (Mathf.Abs((int)transform.eulerAngles.y - (int)hitInfo.transform.eulerAngles.y) == 180) && (transform.position.x < hitInfo.transform.position.x && moveScript.facingRight || transform.position.x > hitInfo.transform.position.x && !moveScript.facingRight))
                    { 
                        noDamageSound.Play();
                        enemy.playHitEffect();
                    }
                    else
                    {
                        enemy.TakeDamage(damage);
                    }
                }
                else
                {
                    //    Debug.Log(hitInfo.collider.tag);
                    switch (hitInfo.collider.tag)
                    {
                        case "Enemy":
                            GameObject child = hitInfo.collider.gameObject;
                            child.transform.parent.gameObject.GetComponent<AIDamage>().TakeDamage(damage * 2);
                            break;
                        case "Breakable":
                            Vector3 hitPoint = Vector3.zero;
                            hitPoint.x = hitInfo.point.x - 0.01f * hitInfo.normal.x;
                            hitPoint.y = hitInfo.point.y - 0.01f * hitInfo.normal.y;
                            StartCoroutine(hitInfo.collider.gameObject.GetComponent<BreakAnimation>().Hit(hitInfo.collider.gameObject.GetComponent<Tilemap>().WorldToCell(hitPoint)));
                            break;
                        case "Ground":
                            if(hitEffect != null)
                            {
                                Instantiate(hitEffect, hitInfo.point, transform.rotation);
                            }
                            break;
                        case "Explosive":
                            hitInfo.collider.gameObject.GetComponent<Explosion>().InitiateExplosion();
                            break;
                    }
                }

            }

            if (lineRenderer != null)
            {
                StartCoroutine(renderLine(hitInfo));
            }





            if (!moveScript.facingRight)
            {
                recoilDirection = 1;
            }

            //Recoil Application

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot") || animator.GetCurrentAnimatorStateInfo(0).IsName("Downward_Shoot") || animator.GetCurrentAnimatorStateInfo(0).IsName("Upward_Shoot"))
            {
                if (directionInput[1] == 0 || ( directionInput[0] == 0 && directionInput[1] == 0 ))
                {
                   
                    rb.AddForce(new Vector2(recoilForce / 3 * recoilDirection, 0f), ForceMode2D.Impulse);
                    lastTimeShot = Time.time;
                }
                else
                {
                   
                    rb.AddForce(new Vector2(0f, recoilForce * -directionInput[1] / 1.5f), ForceMode2D.Impulse);
                    if (directionInput[1] == -1)
                    {
                        animator.SetBool("isShooting", false);
                    }
                }
            }
            else //(animator.GetCurrentAnimatorStateInfo(0).IsName("Aerial_Shoot"))
            {
                
                if ( (directionInput[0] != 0  && directionInput[1] == 0 ) || (directionInput[0] == 0 && directionInput[1] == 0))
                {
                   
                    rb.AddForce(new Vector2(recoilForce * recoilDirection / 1.75f, 0f), ForceMode2D.Impulse);
                    
                }
                else if (directionInput[0] * directionInput[1] != 0)
                {

                    vertVelocityReset();
                    rb.AddForce(new Vector2(0f, recoilForce * -directionInput[1] * 1.25f), ForceMode2D.Impulse);
//                    Debug.Log("Wassup0");
                    doneResetting = false;
                }
                else // if (directionInput[1] != 0)
                {
                    vertVelocityReset();
                    if (directionInput[1] == 1)
                    {
                        rb.AddForce(new Vector2(0f, recoilForce * -directionInput[1] * 1.8f), ForceMode2D.Impulse);
                    }
                    else
                    {
                        rb.AddForce(new Vector2(0f, recoilForce * -directionInput[1] * 1.25f), ForceMode2D.Impulse);
                    }
                    doneResetting = false;
                    
                }
                Ammo -= 1;
                if (Ammo == 0) {
                //    if (!lightningOn)
                //    {
                        gameObject.GetComponent<SpriteRenderer>().color = outOfAmmoColor;
                //    }
                }
            }

            if (moveScript.isGrounded)
            {
                CancelInvoke("correctRotation");
            }
            if(switcher != null)
            {
                switcher.Switch();
            }
        }

        
    }

    private IEnumerator renderLine(RaycastHit2D hitInfo)
    {
        lineRenderer.SetPosition(0, new Vector2(firePoint.position.x, firePoint.position.y - 0.1f)); 
        if (hitInfo.collider != null)
        {
            lineRenderer.SetPosition(1, new Vector2(hitInfo.point.x, hitInfo.point.y - 0.1f));
        }
        else
        {
            Vector2 vect = firePoint.position + (range * firePoint.right);
            lineRenderer.SetPosition(1, vect);
        }

        
        lineRenderer.enabled = true;

        yield return new WaitForSeconds(0.02f);
        lineRenderer.enabled = false;
        
        
    }

    void correctRotation()
    {
 //       Debug.Log("Hello");
        if (!moveScript.isGrounded)
        {

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 90f * directionInput[1]);
        }
    }

    void cancelCorrection()
    {
        CancelInvoke("correctRotation");
    }
    void vertVelocityReset()
    {
        if (rb.velocity.y < 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }

    void checkRecoilFinished()
    {
        
        if (Time.time >= lastTimeShot + ogRecoilDuration && !animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot"))
        {
            animationDone = true;
            if (Time.time >= lastTimeShot + recoilDuration)
            {
                recoilFinished = true;
            }
            else
            {
                recoilFinished = false;
            }

        }

        else
        {
            recoilFinished = false;
            animationDone = false;
        }

    }
    



}
