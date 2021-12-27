using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Shoot : MonoBehaviour
{
    public GameObject projectile;
    public LayerMask weaponMask;
    public Transform firePoint;
    [SerializeField]
    private float range;

    [Tooltip("Affects how angled the raycast detection is")]
    public float raycast_y;

    private float lastTimeAttacked;
    public float attackCooldown;

    public Animator animator;

    public AudioSource shootSound;
    public AudioSource reloadSound; //not mandatory

    public bool shootVertical = false;
    
    //Similar to AI_Collision, this script was made as it was usually integrated into Patrol. 
    // Start is called before the first frame update
    void Start()
    {
        lastTimeAttacked = 0;   
        if(raycast_y == 0)
        {
            raycast_y = 0.2f;
        }
    }

    
    void Update()
    {
        RaycastHit2D LOSmiddle;
        RaycastHit2D LOSup;
        RaycastHit2D LOSdown;

        LOSmiddle = Physics2D.Raycast(firePoint.position, transform.right.x *  Vector2.right, range, weaponMask);
        LOSup = Physics2D.Raycast(firePoint.position, transform.right.x * new Vector2(1, raycast_y), range, weaponMask);
        LOSdown = Physics2D.Raycast(firePoint.position, transform.right.x * new Vector2(1, -raycast_y), range, weaponMask);
        if(shootVertical)
        {
            LOSmiddle = Physics2D.Raycast(firePoint.position, transform.right.y * Vector2.up, range, weaponMask);
            LOSup = Physics2D.Raycast(firePoint.position, transform.right.y * new Vector2(raycast_y, 1), range, weaponMask);
            LOSdown = Physics2D.Raycast(firePoint.position, transform.right.y * new Vector2(-raycast_y, 1), range, weaponMask);
        }

        if(LOSmiddle.collider != null && LOSmiddle.collider.tag == "Player" && canAttack() )
        {
            beginAttack();
        }

        else if(LOSup.collider != null && LOSup.collider.tag == "Player" && canAttack())
        {
            beginAttack();
        }

        else if(LOSdown.collider != null && LOSdown.collider.tag == "Player" && canAttack())
        {
            beginAttack();
        }
    }

    bool canAttack()
    {
        return (Time.timeSinceLevelLoad >= lastTimeAttacked + attackCooldown);
    }

    void beginAttack()
    {
        animator.SetBool("attackingPlayer", true);
        lastTimeAttacked = Time.timeSinceLevelLoad;
       // StartCoroutine(attackPlayer());
    }

    void stopAttacking()
    {
        animator.SetBool("attackingPlayer", false);
        lastTimeAttacked = Time.timeSinceLevelLoad;
    }

    void fire()
    {
        Instantiate(projectile, firePoint.position, firePoint.rotation);
        if(shootSound != null)
        {
            shootSound.Play();
        }
    }

    void playReload()
    {
        reloadSound.Play();
    }

    private void OnDrawGizmos()
    {
        if (shootVertical)
        {
            Gizmos.DrawRay(firePoint.position, transform.right.y *  Vector2.up * range);
            Gizmos.DrawRay(firePoint.position, transform.right.y * new Vector2(raycast_y, 1) * range);
            Gizmos.DrawRay(firePoint.position, transform.right.y * new Vector2(-raycast_y, 1) * range);
        }
        else
        {
            Gizmos.DrawRay(firePoint.position, transform.right.x * Vector2.right * range);
            Gizmos.DrawRay(firePoint.position, transform.right.x * new Vector2(1, raycast_y) * range);
            Gizmos.DrawRay(firePoint.position, transform.right.x * new Vector2(1, -raycast_y) * range);
        }
    }
}
