using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class JBBoss : MonoBehaviour, BossInterface
{

    public GameObject projectile;
    public Transform projectileParent;
    public GameObject mushroom;
    private GameObject spawnedShroom;
    private bool canAttack;
    public float attackCooldown; // random default value


    public Transform firepoint;

    public RuntimeAnimatorController newController;
    
    [SerializeField]
    private Animator anim;

    private bool teleporting = false;
    private bool moving = false;

    private Vector2 moveVector;
    private GameObject player;

    [SerializeField]
    private Vector2 teleportRangeX;
    [SerializeField]
    private Vector2 teleportRangeY;
    [SerializeField]
    private AudioSource teleportSound;
    [SerializeField]
    private AudioSource throwSound;

    [System.NonSerialized]
    public int coloredJBCount = 3;

    public GameObject redJB;
    public GameObject greenJB;
    public GameObject blueJB;

    public PanelSwitcher switcher;

    public GameObject[] objectsToEnable;

    public PlayableDirector[] outroTimelines;
    [TextArea(3, 10)]
    public string[] sentences;
    public string[] speakers;
    public DialogController dialogControl; //used to transition from fight to ending cutscene
    

    // Start is called before the first frame update
    void Start()
    {
        anim.runtimeAnimatorController = newController;
        canAttack = false;
        player = GameObject.Find("player1");
        
        foreach(GameObject obj in objectsToEnable)
        {
            obj.SetActive(true);
        }
        switcher.on = true;
        moveVector = new Vector2(0, 0);
        
        StartCoroutine(setCanAttack(attackCooldown));
        StartCoroutine(teleport());
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if( !(moveVector.y > 0 && transform.parent.position.y > 355) && !(moveVector.y < 0 && transform.parent.position.y < 330) && (Mathf.Abs(transform.position.x - player.transform.position.x) >= 2 || Mathf.Abs(transform.position.y - player.transform.position.y) >= 2))
            transform.parent.Translate(25 * moveVector * Time.deltaTime);    
        }
    }


    public void onDamageTaken()
    {
        if (GetComponent<AIDamage>().health == 650 || GetComponent<AIDamage>().health == 300)
        {
            StopAllCoroutines();
            moving = false;
            teleporting = false;
            anim.Play("JB_Split");
        }
        else if (GetComponent<AIDamage>().health == 0) //dying into outro cutscene
        {
            StopAllCoroutines();
            foreach (GameObject obj in objectsToEnable)
            {
                obj.SetActive(false); //get rid of UI elements
            }
            //gets rid of any rubiks cubes in there
            Destroy(projectileParent.gameObject);

            dialogControl.timelines = outroTimelines;
            dialogControl.sentences = sentences;
            dialogControl.speakers = speakers;

            StartCoroutine(dialogControl.outro());

        }
        else if (!moving && !teleporting)
        {
            StopCoroutine(teleport());
            StartCoroutine(teleport());
        }

    }

    public IEnumerator enableColoredJB()
    {
        coloredJBCount = 3;

        redJB.GetComponent<AIDamage>().health = 50;
        greenJB.GetComponent<AIDamage>().health = 50;
        blueJB.GetComponent<AIDamage>().health = 50;

        redJB.transform.parent.position = transform.parent.position;
        greenJB.transform.parent.position = transform.parent.position;
        blueJB.transform.parent.position = transform.parent.position;
        yield return new WaitForSeconds(1);

        redJB.transform.parent.gameObject.SetActive(true);
        greenJB.transform.parent.gameObject.SetActive(true);
        blueJB.transform.parent.gameObject.SetActive(true);

        redJB.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
        greenJB.transform.parent.gameObject.GetComponent<Animator>().enabled = true;
        blueJB.transform.parent.gameObject.GetComponent<Animator>().enabled = true;

        redJB.transform.parent.GetComponent<Animator>().Play("JBRed_SplitLeft");
        greenJB.transform.parent.GetComponent<Animator>().Play("JBGreen_SplitCenter");
        blueJB.transform.parent.GetComponent<Animator>().Play("JBBlue_SplitRight");


        yield return new WaitForSeconds(0.67f);

        

        redJB.GetComponent<Animator>().Play("JB_Idle");
        greenJB.GetComponent<Animator>().Play("JB_Idle");
        blueJB.GetComponent<Animator>().Play("JB_Idle");

        StartCoroutine(teleportColored(redJB));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(teleportColored(greenJB));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(teleportColored(blueJB));

        redJB.transform.parent.gameObject.GetComponent<Animator>().enabled = false;
        greenJB.transform.parent.gameObject.GetComponent<Animator>().enabled = false;
        blueJB.transform.parent.gameObject.GetComponent<Animator>().enabled = false;

    }

    public void reappear()
    {
        anim.Play("JB_Appear");
    }

    private IEnumerator setCanAttack(float time)
    {
        yield return new WaitForSeconds(time);
        if (!teleporting)
        {
            canAttack = true;
        }
    }

    IEnumerator teleport()
    {
        teleporting = true;
        canAttack = false;
        StopCoroutine(setCanAttack(attackCooldown));
        anim.Play("JB_Disappear");
        teleportSound.Play();
        yield return new WaitForSeconds(Random.Range(1.5f, 2)); //adjust based on disappear length
        
        transform.parent.position = new Vector2(Random.Range(teleportRangeX.x, teleportRangeX.y), 
                                                Random.Range(teleportRangeY.x, teleportRangeY.y));

        anim.Play("JB_Appear");
        yield return new WaitForSeconds(0.7f);
        
        yield return StartCoroutine(followPlayer(player, 0.5f));

        if (transform.position.x < player.transform.position.x)
        {
            anim.Play("throwCubeRight");
        }
        else
        {
            anim.Play("throwCube");
        }
        yield return new WaitForSeconds(0.5f);
        teleporting = false;
        yield return new WaitForSeconds(2f); //teleport after long enough time
        if (!teleporting)
        {
            StartCoroutine(teleport());
        }
    }

    IEnumerator teleportColored(GameObject JB) //for colored JB's
    {
        JB.GetComponent<Animator>().Play("JB_Disappear");
        JB.GetComponents<AudioSource>()[0].Play(); //jb disappear



        yield return new WaitForSeconds(Random.Range(1.5f, 2));

        switch (JB.name)
        {
            case "JB_Red":
                JB.transform.parent.position = new Vector2(Random.Range(teleportRangeX.x, teleportRangeX.x + (teleportRangeX.y-teleportRangeX.x)/3),
                                                Random.Range(teleportRangeY.x, teleportRangeY.y));
                break;
            case "JB_Green":
                JB.transform.parent.position = new Vector2(Random.Range(teleportRangeX.x + (teleportRangeX.y - teleportRangeX.x) / 3, teleportRangeX.x + 2 * (teleportRangeX.y - teleportRangeX.x) / 3),
                                                Random.Range(teleportRangeY.x, teleportRangeY.y));
                break;
            case "JB_Blue":
                JB.transform.parent.position = new Vector2(Random.Range(teleportRangeX.x + 2 * (teleportRangeX.y - teleportRangeX.x) / 3, teleportRangeX.y),
                                                Random.Range(teleportRangeY.x, teleportRangeY.y));
                break;
            default:
                JB.transform.parent.position = new Vector2(Random.Range(teleportRangeX.x, teleportRangeX.y),
                                                Random.Range(teleportRangeY.x, teleportRangeY.y));
                break;
        }
        

        JB.GetComponent<Animator>().Play("JB_Appear");
        yield return new WaitForSeconds(0.7f);

        if (JB.transform.position.x < player.transform.position.x)
        {
            JB.GetComponent<Animator>().Play("throwCubeRight");
        }
        else
        {
            JB.GetComponent<Animator>().Play("throwCube");
        }
        yield return new WaitForSeconds(0.34f);
        throwCube(JB);
        yield return new WaitForSeconds(2.2f);
        if (JB.transform.parent.gameObject.activeSelf)
        {
            StartCoroutine(teleportColored(JB));        
        }
        

    }

    
    IEnumerator followPlayer(GameObject player, float duration)
    {
        moving = true;
        float angle = angleToPlayer(gameObject, player);
        moveVector = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle));
        yield return new WaitForSeconds(duration);
        
        moving = false;
        yield return new WaitForSeconds(0.4f);
        
    }
    

    public void throwCube() //called in animation 
    {
        float randomNum = Random.Range(0, 10);
        if (player.GetComponent<PlayerHealth>().health <= 20 && randomNum > 7 && spawnedShroom == null) //shroom if low on health
        {
            spawnedShroom = Instantiate(mushroom, firepoint.position, firepoint.rotation);
        }
        else
        {
            GameObject cube = projectile;
            float cubeSpeed = 21;
            float angle = angleToPlayer(gameObject, player);

            cube.GetComponent<Bullet>().speed = cubeSpeed * Mathf.Cos(Mathf.Deg2Rad * angle);
            cube.GetComponent<Bullet>().verticalVelocity = cubeSpeed * Mathf.Sin(Mathf.Deg2Rad * angle);


            Instantiate(cube, firepoint.position, firepoint.rotation).transform.SetParent(projectileParent);
            throwSound.Play();
        }
    }

    public void throwCube(GameObject JB) //overloaded for colored JB
    {
        GameObject cube = projectile;
        float cubeSpeed = 23;
        float angle = angleToPlayer(JB, player);

        cube.GetComponent<Bullet>().speed = cubeSpeed * Mathf.Cos(Mathf.Deg2Rad * angle);
        cube.GetComponent<Bullet>().verticalVelocity = cubeSpeed * Mathf.Sin(Mathf.Deg2Rad * angle);

        if (JB.transform.parent.gameObject.activeSelf)
        {
            Instantiate(cube, JB.transform.position, JB.transform.rotation).transform.SetParent(projectileParent);
            JB.GetComponents<AudioSource>()[1].Play(); // throw sound
        }
    }

    private float angleToPlayer(GameObject from, GameObject player)
    {
        float angle = Vector2.Angle(new Vector2(1, 0), player.transform.position - from.transform.position);
        if (player.transform.position.y < from.transform.position.y)
        {
            angle = 360 - angle;
        }
        return angle;
    }
}
