using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Patrol : MonoBehaviour
{
    public float speed;
    [SerializeField]
    private bool movingRight = true;

    public Transform groundDetection;
    public bool hasSideDetectPoint;
    public Transform sideDetectPoint; //must be filled only if above bool is true

    public Slider healthbar;

    public Rigidbody2D rb;

    public bool grounded = false;
    public bool onlyMoveWhenGround;

    private float movingPlatVel = 0;
    private bool onMovingPlat = false;
    private Transform originalParent;
    public bool waitForMovingPlat;


    public LayerMask mask;
    public LayerMask sideDetectMask;
    public LayerMask weaponMask;
    public bool useWeaponMask;
    public bool multipleFirepoints;

    [SerializeField]
    private Transform
        touchDamageCheck;

    [SerializeField]
    private LayerMask whatIsPlayer;

    [SerializeField]
    private float
        lastTouchDamageTime,
        touchDamageCooldown,
        touchDamage,
        touchDamageWidth,
        touchDamageHeight;

   // [SerializeField] never needed to input info, its assigned belwo
    private Vector2
        touchDamageBotLeft,
        touchDamageTopRight;

    [SerializeField]
    private float[] attackDetails = new float[2];

    private GameObject alive;
    public PlayerHealth PH;

    public Transform firePoint;
    public GameObject bullet;
    
    private float originalSpeed;

    [SerializeField]
    private float attackCooldown;

    float lastTimeAttacked;

    [SerializeField]
    private float range;

    public Animator animator;

    public bool autoAttack;
    public AudioSource gunSound;

    bool isFlipping;

    public int bulletDamage;
    //how many bullets the enemy fires
    public int timesToShoot;

  //  public bool wantToLog;
    public bool animatorControlsAttack;

    public bool affectedByLight;
    public bool flipOnPlayerTouch;

    public bool flipWithDelay;

    private Vector2 spawnPos;
    public bool fixedSpawnPoint;

    [System.NonSerialized]
    public int dir = 1;

    public bool alwaysFacePlayer = false;
    private bool allowUpdates = false; //usually, the update function checks if the enemy is falling before updates like raycast detection. when this is true, this bypasses the if statement. used for JB platform

    void Start()
    {
        lastTimeAttacked = 0;
        alive = gameObject;
        PH = GameObject.Find("player1").GetComponent<PlayerHealth>();
        originalSpeed = speed;
        isFlipping = false;
        originalParent = gameObject.transform.parent;
        if (fixedSpawnPoint)
        {
            spawnPos = transform.position;
        }
        if (bulletDamage == null) {
            bulletDamage = 20;
        }
        if(timesToShoot == 0 && !autoAttack)
        {
            timesToShoot = 2;
        }
        else if(timesToShoot == 0 && autoAttack)
        {
            timesToShoot = 3;
        }

       

    }

    private void OnEnable()
    {
        if (fixedSpawnPoint && spawnPos != Vector2.zero)
        {
            transform.position = spawnPos;
        }
    }
    void Update()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        if (transform.eulerAngles.z != 0)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
        }

//        canAttack();



        if (onMovingPlat)
        {
            //TEMPORARY SOLUTION FOR VELOCITY ON MOVING PLATFORM
            rb.velocity = new Vector2(movingPlatVel, rb.velocity.y);
            if (!movingRight)
            {
                transform.Translate((Vector2.right * Mathf.Abs((speed * 1.25f))) * Time.deltaTime);
            }
            else
            {
                transform.Translate((Vector2.right * Mathf.Abs((speed))) * Time.deltaTime);
            }
            
        }
        else if(!waitForMovingPlat && !onlyMoveWhenGround || (onlyMoveWhenGround && grounded))
        {
            transform.Translate(dir * (Vector2.right * Mathf.Abs((speed))) * Time.deltaTime);
        }
        
        
        CheckTouchDamage();
        if (rb.velocity.y == 0 || onMovingPlat && rb.velocity.y < 0.1f || allowUpdates)
        {
            RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, 2f, mask);
            RaycastHit2D sideDetect;
            if (!hasSideDetectPoint)
            {
                sideDetect = Physics2D.Raycast(groundDetection.position, Vector2.right, 0.1f, sideDetectMask);
            }
            else 
            { 
           //     GIVES IT A THICKNESS. USED BECAUSE SHYGUY WOULDNT TURN IN A SCENARIO WHERE A BLOCK IS BLOCKING HIS TOP HALF BUT NOT BOTTOM. FORMAT: BoxCast(pos, size, angle, direction, distance, mask)
                sideDetect = Physics2D.BoxCast(sideDetectPoint.position, new Vector2(0.1f, 0.4f), 0f, Vector2.right, 0.5f, sideDetectMask);
            }
            

            RaycastHit2D lineOfSight;
            RaycastHit2D lineOfSightUp;
            RaycastHit2D lineOfSightDown;

            RaycastHit2D LOSback;
            RaycastHit2D LOSbackUp;
            RaycastHit2D LOSbackDown;

            if (movingRight)
            {
                if (useWeaponMask)
                {
                    lineOfSight = Physics2D.Raycast(firePoint.position, Vector2.right, range, weaponMask);
                    lineOfSightUp = Physics2D.Raycast(firePoint.position, new Vector2(1, 0.2f), range, weaponMask);
                    lineOfSightDown = Physics2D.Raycast(firePoint.position, new Vector2(1, -0.2f), range, weaponMask);

                    LOSback = Physics2D.Raycast(firePoint.position, Vector2.left, range, weaponMask);
                    LOSbackUp = Physics2D.Raycast(firePoint.position, new Vector2(-1, 0.2f), range, weaponMask);
                    LOSbackDown = Physics2D.Raycast(firePoint.position, new Vector2(-1, 0.2f), range, weaponMask);
                }
                else
                {
                    lineOfSight = Physics2D.Raycast(firePoint.position, Vector2.right, range, mask);
                    lineOfSightUp = Physics2D.Raycast(firePoint.position, new Vector2(1, 0.2f), range, mask);
                    lineOfSightDown = Physics2D.Raycast(firePoint.position, new Vector2(1, -0.2f), range, mask);

                    LOSback = Physics2D.Raycast(firePoint.position, Vector2.left, range, mask);
                    LOSbackUp = Physics2D.Raycast(firePoint.position, new Vector2(-1, 0.2f), range, mask);
                    LOSbackDown = Physics2D.Raycast(firePoint.position, new Vector2(-1, 0.2f), range, mask);
                }

            }
            else
            {
                if (useWeaponMask)
                {
                    lineOfSight = Physics2D.Raycast(firePoint.position, Vector2.left, range, weaponMask);
                    lineOfSightUp = Physics2D.Raycast(firePoint.position, new Vector2(-1, 0.2f), range, weaponMask);
                    lineOfSightDown = Physics2D.Raycast(firePoint.position, new Vector2(-1, -0.2f), range, weaponMask);

                    LOSback = Physics2D.Raycast(firePoint.position, Vector2.right, range, weaponMask);
                    LOSbackUp = Physics2D.Raycast(firePoint.position, new Vector2(1, 0.2f), range, weaponMask);
                    LOSbackDown = Physics2D.Raycast(firePoint.position, new Vector2(1, 0.2f), range, weaponMask);
                }
                else
                {
                    lineOfSight = Physics2D.Raycast(firePoint.position, Vector2.left, range, mask);
                    lineOfSightUp = Physics2D.Raycast(firePoint.position, new Vector2(-1, 0.2f), range, mask);
                    lineOfSightDown = Physics2D.Raycast(firePoint.position, new Vector2(-1, -0.2f), range, mask);

                    LOSback = Physics2D.Raycast(firePoint.position, Vector2.right, range, mask);
                    LOSbackUp = Physics2D.Raycast(firePoint.position, new Vector2(1, 0.2f), range, mask);
                    LOSbackDown = Physics2D.Raycast(firePoint.position, new Vector2(1, 0.2f), range, mask);
                }
                
            }

            grounded = (bool)groundInfo.collider;
            //Debug.Log((bool)groundInfo.collider);
            if (!groundInfo.collider)
            {
                if (!flipWithDelay)
                {
                    flip();
                }
                else if(!isFlipping)
                {
                    StartCoroutine(flip(0.15f));
                }
            }
            if (sideDetect.collider != null)
            {
             //   Debug.Log(sideDetect.collider.tag);
                if (sideDetect.collider.tag == "Ground" || sideDetect.collider.tag == "Breakable" || (sideDetect.collider.tag == "Enemy" && sideDetect.collider.gameObject.GetInstanceID() != gameObject.GetInstanceID()))
                {
                    if (!isFlipping)
                    {
                        StartCoroutine(flip(0.05f)); //DONE BECAUSE IT WAS GLITCHY WHEN FAST FOR HAMMERBRO BOSS
                    }
                }
            }

            if(alwaysFacePlayer && (transform.position.x >= PH.gameObject.transform.position.x + 5 && movingRight || transform.position.x <= PH.gameObject.transform.position.x - 5 && !movingRight))
            {
                flip();
            }



            if (lineOfSight.collider != null)
            {
                if (lineOfSight.collider.tag == "Player" && canAttack())
                {
                  //  Debug.Log("middle");
                    beginAttack();
                }
            }
            else if (lineOfSightUp.collider != null)
            {
                if (lineOfSightUp.collider.tag == "Player" && canAttack())
                {
                  //  Debug.Log("up");
                    beginAttack();
                }
            }
            else if (lineOfSightDown.collider != null)
            {
                if (lineOfSightDown.collider.tag == "Player" && canAttack())
                {
                   // Debug.Log("down");
                    beginAttack();
                }
            }

            if (multipleFirepoints)
            {
                if (LOSback.collider != null)
                {
                    if (LOSback.collider.tag == "Player" && canAttack())
                    {
                        beginAttack();
                    }
                }
                if (LOSbackUp.collider != null)
                {
                    if (LOSbackUp.collider.tag == "Player" && canAttack())
                    {
                        beginAttack();
                    }
                }
                if (LOSbackDown.collider != null)
                {
                    if (LOSbackDown.collider.tag == "Player" && canAttack())
                    {
                        beginAttack();
                    }
                }
            }
        }
    }

    void beginAttack()
    {
        //Debug.Log("hey");
        animator.SetBool("attackingPlayer", true);
        lastTimeAttacked = Time.timeSinceLevelLoad;
        StartCoroutine(attackPlayer());
    }



    bool canAttack()
    {
        return (Time.timeSinceLevelLoad >= lastTimeAttacked + attackCooldown && !isFlipping);
    }
    private IEnumerator attackPlayer()
    {
        speed = 0;
        if (!animatorControlsAttack)
        {
            if (autoAttack == false)
            {
                for (int i = 0; i < timesToShoot; i++)
                {
                    GameObject firedBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
                //    Debug.Log(i);
                    firedBullet.GetComponent<Bullet>().Initialize(bulletDamage);
                    if (gunSound != null)
                    {
                        gunSound.Play();
                    }
                    yield return new WaitForSeconds(1.2f);
                    lastTimeAttacked = Time.timeSinceLevelLoad;
                }
            }
            else
            {
                for (int i = 0; i < timesToShoot; i++)
                {
                    GameObject firedBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
                    firedBullet.GetComponent<Bullet>().Initialize(bulletDamage);
                    gunSound.Play();
                    yield return new WaitForSeconds(0.3f);
                    lastTimeAttacked = Time.timeSinceLevelLoad;
                }
            }
            animator.SetBool("attackingPlayer", false);
            speed = originalSpeed;

        }
        yield return 0;
    }

    //used in animator, ex: octoombas in level 4.
    private void fire()
    {
        GameObject firedBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        firedBullet.GetComponent<Bullet>().Initialize(bulletDamage);
        if(gunSound != null)
        {
            gunSound.Play();
        }
    }


    void flip()
    {
        isFlipping = true;
        float currentSpeed = speed;
        speed = 0;
        if (movingRight)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            movingRight = false;
            if (healthbar != null)
            {
                healthbar.transform.Rotate(new Vector3(0, 180, 0));
            }
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            movingRight = true;
            if (healthbar != null)
            {
                healthbar.transform.Rotate(new Vector3(0, 180, 0));
            }
            
        }

        if(affectedByLight)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z * -1);
        }

        speed = currentSpeed;
        isFlipping = false;
    }

    IEnumerator flip(float t)
    {
        float currentSpeed = speed;
        isFlipping = true;
        speed = 0;
        yield return new WaitForSeconds(t);
        if (movingRight)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            movingRight = false;
            if (healthbar != null)
            {
                healthbar.transform.Rotate(new Vector3(0, 180, 0));
            }
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            movingRight = true;
            if (healthbar != null)
            {
                healthbar.transform.Rotate(new Vector3(0, 180, 0));
            }

        }

        if (affectedByLight)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, gameObject.transform.localScale.z * -1);
        }

        speed = currentSpeed;
        isFlipping = false;
    }

    private void CheckTouchDamage()
    {
        if(Time.timeSinceLevelLoad >= lastTouchDamageTime + touchDamageCooldown && PH.invulnerable == false)
        {
            
            touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

            Collider2D hit = Physics2D.OverlapArea(touchDamageBotLeft, touchDamageTopRight, whatIsPlayer);
            if (hit != null)
            {
                lastTouchDamageTime = Time.timeSinceLevelLoad;
                attackDetails[0] = touchDamage;
                attackDetails[1] = alive.transform.position.x;;
                PH.Damage(attackDetails);
                if(flipOnPlayerTouch)
                {
                    flip();
                }
                
            }
        }
    }

    /**
     * Use if the enemy's attacking script is being run by animator. For instance, Pokey's shotgun script that doesn't use the normal coroutine
     * 
     */
    public void stopAttacking()
    {
        lastTimeAttacked = Time.timeSinceLevelLoad;
        animator.SetBool("attackingPlayer", false);
        speed = originalSpeed;
    }
   

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("platform") && collision.gameObject.GetComponent<fallingPlatform>().movingHorizontal)
        {
            rb.velocity = new Vector2(collision.gameObject.GetComponent<Rigidbody2D>().velocity.x, rb.velocity.y);
            movingPlatVel = gameObject.GetComponent<Rigidbody2D>().velocity.x;
            transform.SetParent(collision.gameObject.transform);
            onMovingPlat = true;

        }
        else if(collision.gameObject.CompareTag("platform") && collision.gameObject.name.Equals("JBplatform"))
        {
            allowUpdates = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("platform"))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            movingPlatVel = 0;
            transform.SetParent(originalParent);
            onMovingPlat = false;
        }
    }
    public void OnDrawGizmos()
    {
        touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 topRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

        if (movingRight)
        {
            Gizmos.DrawRay(firePoint.position, Vector2.right * range);
            Gizmos.DrawRay(firePoint.position, new Vector2(1, 0.2f) * range);
            Gizmos.DrawRay(firePoint.position, new Vector2(1, -0.2f) * range);
        }
        else
        {
            Gizmos.DrawRay(firePoint.position, Vector2.left * range);
            Gizmos.DrawRay(firePoint.position, new Vector2(-1, 0.2f) * range);
            Gizmos.DrawRay(firePoint.position, new Vector2(-1, -0.2f) * range);
        }
        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }



}
