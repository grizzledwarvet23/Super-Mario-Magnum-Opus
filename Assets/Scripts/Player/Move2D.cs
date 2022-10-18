using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2D : MonoBehaviour
{




  //  public float moveSpeed = 5f;
    public float jumpHeight; //23 ORIGINAL VALUE
    public bool isGrounded;
    public float velocity = 0f;
    public float sprintAmp = 1f;
    public bool isMoving = false;
    public int items = 0;

    public float fallingGrav = 3.7f, normalGrav = 3f;


    public Animator animator;
    Rigidbody2D rb;
    public bool facingRight = true;

    public bool attacking;
    public float rigidSpeed;
    public float maxAirSpeed;

    public Weapon WeaponScript;


    private bool knockback;
    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;
    private float originalKnockbackDuration;
    private bool usedCustomKnockback;

    [SerializeField]
    private Vector2 knockbackSpeed;

    Vector3 breakHitboxPos;

    GroundCheck gc;
    private GameMaster gm;

 //   public slipperyMovement slipScript;

    private bool locked;
    public bool enableSlippery;
    [System.NonSerialized]
    public bool isJumping;
    private float jumpTimeCounter;
    public float jumpTime;

    public Transform checkStuck;
    public LayerMask stuckMask;

    private bool jumpedFromMoving;

    private float adjustedJumpHeight = 0;
    
   // private bool isOnIce;


    
    void Start()
    {
        originalKnockbackDuration = knockbackDuration;
        if (GameObject.FindGameObjectWithTag("GM") != null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
        if (gm != null)
        {
            if (gm.checkPointReached)
            {
                transform.position = gm.lastCheckPointPos;
            }
            gm.cameraReset();
        }
        
        rb = gameObject.GetComponent<Rigidbody2D>();
        breakHitboxPos = gameObject.transform.GetChild(2).gameObject.transform.localPosition;
        gc = gameObject.GetComponent<GroundCheck>();

        if(jumpTime == 0)
        {
            jumpTime = 0.35f;
        }

    }

    // Update is called once per frame
    void LateUpdate() {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Aerial_Shoot") && transform.eulerAngles.z != 0)
        {

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            gameObject.transform.GetChild(2).gameObject.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
            gameObject.transform.GetChild(2).gameObject.transform.localPosition = breakHitboxPos;
        //    WeaponScript.CancelInvoke("correctRotation");


        }


    }
    void Update()
    {
 
        if (transform.eulerAngles.z == 0 && !animator.GetCurrentAnimatorStateInfo(0).IsName("Aerial_Shoot") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot"))
        {
            isGrounded = gc.checkIfGrounded();
        }

        spriteUpdate();
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot") || animator.GetCurrentAnimatorStateInfo(0).IsName("Upward_Shoot") || animator.GetCurrentAnimatorStateInfo(0).IsName("Downward_Shoot"))
        {
            animator.SetFloat("Speed", 0f);
        }
        if (!isGrounded)
        {
            animator.SetFloat("VerticalSpeed", Mathf.Sign(rb.velocity.y));
        }
        CheckKnockback();
        if (!attacking)
        {
            Jump(jumpHeight, false);
        }

        if (checkStuck != null) //THIS IS TO CHECK IF MARIO GETS STUCK IN GROUND
        {
            RaycastHit2D check = Physics2D.Raycast(checkStuck.position, Vector2.up, 0.2f, stuckMask);
            if (check.collider != null)
            {
                if (check.collider.tag == "Ground" && !check.collider.gameObject.name.Contains("OneWay") && !attacking)
                {
                    //       Debug.Log("hey");
                    transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f);
                }
            }
            
        }

    }



    IEnumerator slipTest()
    {
        locked = true;
        yield return new WaitForSeconds(0.4f);
        locked = false;
    }
    void FixedUpdate()
    {
  
            checkAttacking();
        if (!isGrounded)
        {
          //  Debug.Log(Input.GetButton("Jump"));
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = fallingGrav; //3.7f
            }
            else
            {
                rb.gravityScale = normalGrav; //3f
            }
        }
        float moveDirection = Input.GetAxisRaw("Horizontal");



       
        if (isGrounded /* &&  WeaponScript.recoilFinished */ || (!isGrounded && animator.GetCurrentAnimatorStateInfo(0).IsName("Aerial_Shoot") && WeaponScript.directionInput[1] == 0))
        {
            if (moveDirection < 0 && facingRight)
            {
                flip();
            }
            else if (moveDirection > 0 && !facingRight)
            {
                flip();
            }
       
        }
       




        //MOVEMENT CONTROL
        if (!animator.GetBool("isShooting") && WeaponScript.recoilFinished && !knockback) //done for hamer bro boss
        {
            float sprintSpeed = Input.GetAxis("Horizontal");
            //Gets the left and right input (hence input on x axis)
            // velocity = Input.GetAxis("Horizontal") * moveSpeed;

            if ( (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ) && animator.GetCurrentAnimatorStateInfo(0).IsName("moving_animation"))
            {
                sprintSpeed *= sprintAmp;
                animator.speed = 2;

            }
            else
            {
                animator.speed = 1;
            }
         

            //this statement determines air control
            if (!isGrounded && Input.GetAxisRaw("Horizontal") == 0 && (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)))
            {
                rb.velocity = new Vector2(2f * Mathf.Sign(rb.velocity.x), rb.velocity.y);
            }
            else if (Input.GetAxis("Horizontal") != 0 && !isGrounded && rb.velocity.x != 0)
            {
                
                if (Mathf.Abs(rb.velocity.x) < maxAirSpeed || rb.velocity.x * Input.GetAxis("Horizontal") < 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x + Input.GetAxisRaw("Horizontal") / 1.65f, rb.velocity.y);
                }
              
                
            }
            

            else if (Input.GetAxis("Horizontal") != 0)
            {
                if (gc.onMovingPlatform)
                {
                    rb.velocity = new Vector2(gameObject.transform.parent.GetComponent<Rigidbody2D>().velocity.x +
                        (moveDirection * rigidSpeed * Mathf.Abs(sprintSpeed)), rb.velocity.y);
                }
                else
                {
               
                    if (!enableSlippery)
                    {
                        
                        rb.velocity = new Vector2(moveDirection * rigidSpeed * Mathf.Abs(sprintSpeed), rb.velocity.y);
                    }
                    else
                    {
                        if(Mathf.Abs(rb.velocity.x) < 10)
                        {
//                            Debug.Log(moveDirection);
                            rb.AddForce(new Vector2(0.9f * moveDirection * Mathf.Abs(sprintSpeed), 0), ForceMode2D.Impulse);
                        }
                    }
              
                }


                animator.SetFloat("Speed", 1f);
            }

        }



        if (Mathf.Abs(rb.velocity.x) > 0.3)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        if (rb.velocity.y < -20)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + 1);
        }


        






    }


    public void Knockback(int direction)
    {
        usedCustomKnockback = false;
        knockbackDuration = originalKnockbackDuration;
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
     //   Debug.Log("Occured");
    }

    //  potential overload
    public void Knockback(Vector2 vector, float customDuration)
    {
        usedCustomKnockback = true;
        knockbackDuration = customDuration;
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = vector;
    }
    
    private void CheckKnockback()
    {
        if ((Time.time >= knockbackStartTime + knockbackDuration && knockback))
        {
            knockback = false;
            if (!usedCustomKnockback)
            {
                rb.velocity = new Vector2(0.0f, rb.velocity.y);
            }
            knockbackDuration = originalKnockbackDuration;
            usedCustomKnockback = false;


        }
        
        
    }

     


    public void Jump(float height, bool enemyBounced)
    {
        if ( (Input.GetButtonDown("Jump") && isGrounded && rb.velocity.y > -10f && !animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot") && WeaponScript.recoilFinished) || enemyBounced)
        {
            if (gc.onMovingPlatform)
            {
                adjustedJumpHeight = rb.velocity.y + jumpHeight;
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + jumpHeight);
                
               
                jumpedFromMoving = true;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                jumpedFromMoving = false;
            }
            gameObject.GetComponent<SoundControl>().PlayJump();
            isJumping = true;
            jumpTimeCounter = jumpTime;
           
        }
        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimeCounter > 0  && !gameObject.GetComponentInChildren<checkIfColliding>().colliding)
            {
                if(!jumpedFromMoving)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
                }
                else
                {
                    rb.velocity = new Vector2(rb.velocity.x, adjustedJumpHeight);
                }
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
                jumpedFromMoving = false;
            }
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumping = false;
        }
    }
    
    void spriteUpdate()
    {

        if (!isGrounded)
        {
            animator.SetBool("isGrounded", false);
        }

        if (Input.GetAxisRaw("Horizontal") != 0 && isGrounded && WeaponScript.animationDone )
        {
            animator.SetBool("isGrounded", true);
            animator.SetFloat("Speed", 1f);
        }

        if (Input.GetAxisRaw("Horizontal") == 0 && (isGrounded))
        {
            animator.SetBool("isGrounded", true);
            animator.SetFloat("Speed", 0f);
        }

    }
    void checkAttacking()
    {

        if (((animator.GetCurrentAnimatorStateInfo(0).IsName("Player_Shoot") || animator.GetCurrentAnimatorStateInfo(0).IsName("Aerial_Shoot") || animator.GetCurrentAnimatorStateInfo(0).IsName("Upward_Shoot")) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) || 
            (animator.GetCurrentAnimatorStateInfo(0).IsName("Downward_Shoot")) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            animator.SetBool("isShooting", false);
            animator.SetInteger("VertAttackDir", 0);
        }

        attacking = animator.GetBool("isShooting");




    }



    public void flip()
    {
        if (!knockback)
        {
            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    public void flipCutscene()
    {
        transform.eulerAngles = new Vector3(0, 0, 0);
        facingRight = true;
    }
    public void fireAnimation()
    {
        animator.SetBool("isShooting", true);
        attacking = animator.GetBool("isShooting");
        animator.speed = 1;
        if (isGrounded)
        {
            if (!gc.onMovingPlatform && !enableSlippery)
            {
                rb.velocity = Vector2.zero;
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        if (checkStuck != null)
        {
            Gizmos.DrawRay(checkStuck.position, new Vector2(0, 0.2f));
        }
    }





}