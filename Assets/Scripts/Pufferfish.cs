using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pufferfish : MonoBehaviour
{
    Rigidbody2D rb;
    //public Transform firepoint1;
    public Transform[] firepoints;
    public GameObject bulletPrefab;
    public LayerMask detectMask;
    public LayerMask groundMask;
    private bool canAttack;
    private Animator animator;

    private GameObject submarine;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        canAttack = true;
        submarine = GameObject.Find("submarine");
    }

    private void Update()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 10, detectMask);
        RaycastHit2D path = Physics2D.Linecast(transform.position, submarine.transform.position, groundMask);
        if(col != null && col.tag == "Player" && canAttack && path.collider == null)
        {
            canAttack = false;
            animator.Play("pufferfish_transition");
        }
    }

    IEnumerator spin() //refrences in transition animation
    {
        Vector2 originalSpeed = rb.velocity;
        Vector3 originalRotation = transform.eulerAngles;

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 120;
        
        gameObject.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(3f);

        rb.angularVelocity = 0;
        transform.eulerAngles = originalRotation;
        animator.Play("pufferfish_idle");
        rb.velocity = originalSpeed;

        gameObject.GetComponent<AudioSource>().Stop();
        yield return new WaitForSeconds(4.25f); //delay until can attack  again
        canAttack = true;
    }
    private void fire()
    {
        foreach(Transform firepoint in firepoints)
        {
            Instantiate(bulletPrefab, firepoint.position, new Quaternion(firepoint.rotation.x, firepoint.rotation.y, firepoint.rotation.z, firepoint.rotation.w));
        }
        //Instantiate(bulletPrefab, firepoint1.position, firepoint1.rotation);
        //Instantiate(bulletPrefab, firepoint2.position, firepoint2.rotation);
    }

    

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 10);
    }

}
