using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magikoopa : MonoBehaviour
{

    public LayerMask detectMask;
    public GameObject missile;
    private Animator anim;
    public AudioSource fireSound;

    public float attackCooldown;

    public Transform firePoint;
    public float speed;

    public Transform sideDetectPoint;
    public Transform groundDetectPoint;

    public LayerMask groundMask;
    public LayerMask sideMask;
    public LayerMask pathMask;

    public GameObject Weakspot;

    [System.NonSerialized]
    public bool canFlip = true;
    bool movingRight = false;

    private void Start()
    {
        movingRight = transform.eulerAngles.y != 180;
        anim = GetComponent<Animator>();
    }

    private bool canAttack = true;
    private void Update()
    {
        if (!anim.GetBool("attackingPlayer"))
        {
            transform.Translate(Vector2.right * -speed * Time.deltaTime);
        }
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetectPoint.position, Vector2.down, 2f, groundMask);
        RaycastHit2D sideDetect = Physics2D.Raycast(sideDetectPoint.position, Vector2.down, 0.1f, sideMask);

        if(!groundInfo.collider)
        {
            //Debug.Log("ey");
            flip();
        }
        if(sideDetect.collider != null) //try changing to else if to see changes
        {
            if(sideDetect.collider.tag == "Ground" || sideDetect.collider.tag == "Breakable" || 
                (sideDetect.collider.tag == "Enemy" && sideDetect.collider.gameObject.GetInstanceID() != gameObject.GetInstanceID()))
            {
                flip();
            }
        }


        Collider2D col = Physics2D.OverlapCircle(transform.position, 12, detectMask);
        if (col != null && col.tag == "Player" && canAttack)
        {
            GameObject player = col.gameObject;
            RaycastHit2D path = Physics2D.Linecast(firePoint.position, player.transform.position, pathMask);
            if (path.collider == null)
            {
                canAttack = false;
                anim.SetBool("attackingPlayer", true);
                StartCoroutine(fire(player));
            }
        }
    }   

    void flip()
    {
        if (canFlip)
        {
            if (!movingRight)
            {
                transform.eulerAngles = new Vector3(0, 180f, 0);
            }
            else
            {
                transform.eulerAngles = Vector3.zero;
            }

            movingRight = !movingRight;
        }
    }

    void stopAttacking()
    {
        StartCoroutine(setCanAttack());
        anim.SetBool("attackingPlayer", false);
    }

    IEnumerator setCanAttack()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    public void setVulnerability(bool vulnerable) //yes means vulnerable, no means invulnerable
    {
        if(!vulnerable)
        {
            SpriteRenderer rend = GetComponent<SpriteRenderer>();
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0.55f);
            //     GetComponent<AIDamage>().enabled = false;
            gameObject.layer = LayerMask.NameToLayer("NoPlayerCollision");
            gameObject.GetComponent<AICollision>().enabled = false;
            Weakspot.SetActive(false);

        }
        else
        {
            SpriteRenderer rend = GetComponent<SpriteRenderer>();
            rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 1f);
            gameObject.layer = LayerMask.NameToLayer("Enemy");
            gameObject.GetComponent<AICollision>().enabled = true;
            Weakspot.SetActive(true);
            //   GetComponent<AIDamage>().enabled = true;
        }
    }
    private IEnumerator fire(GameObject player)
    {
        yield return new WaitForSeconds(0.65f); //match time with animation: 1.1
        fireSound.Play();
        yield return new WaitForSeconds(0.10f);
        Vector3 direction = transform.right;
        Vector3 pos = player.transform.position - firePoint.position;
        
        float angle = Vector3.Angle(pos, new Vector3(1,0,0));
        //Debug.Log(angle);
        if (player.transform.position.y < firePoint.position.y)
        {
            angle = 360 - angle;
        }
        firePoint.rotation = Quaternion.Euler(0, 0, angle);
        GameObject projectile = Instantiate(missile, firePoint.position, firePoint.rotation);
        
        
        
       StartCoroutine(followForDuration(projectile, player, 14, direction));
    }

    private IEnumerator followForDuration(GameObject projectile, GameObject player, float duration, Vector3 direction)
    {
        for(int x = 0; x < duration; x++)
        {
     
            if (projectile != null && !player.GetComponent<PlayerHealth>().isDying)
            {
                Vector2 pos = player.transform.position - projectile.transform.position;
                float angle = Vector2.Angle(pos, new Vector2(1,0));
                if (player.transform.position.y < projectile.transform.position.y)
                {
                    angle = 360 - angle;
                }
                projectile.transform.rotation = Quaternion.Euler(0, 0, angle);
                projectile.GetComponent<Bullet>().addVelocity(0);
                if(projectile.GetComponent<Bullet>().hasVerticalVel)
                {
                    projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(projectile.GetComponent<Rigidbody2D>().velocity.x, projectile.GetComponent<Rigidbody2D>().velocity.y + projectile.GetComponent<Bullet>().verticalVelocity);
                }
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 12);
    }
}
