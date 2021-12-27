using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICollision : MonoBehaviour
{
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

    [SerializeField]
    private Vector2
        touchDamageBotLeft,
        touchDamageTopRight;

    [SerializeField]
    private float[] attackDetails = new float[2];

    private GameObject alive;
    public PlayerHealth PH;
    public string playerObjectName;

    [SerializeField]
    private float attackCooldown;

    float lastTimeAttacked;


    void Start() {
        lastTimeAttacked = 0;
        alive = gameObject;
        //       PH = GameObject.Find("player1").GetComponent<PlayerHealth>();
        if (PH == null)
        {
            if (playerObjectName != null)
            {
                PH = GameObject.Find(playerObjectName).GetComponent<PlayerHealth>();
            }
            else
            {
                PH = GameObject.Find("player1").GetComponent<PlayerHealth>();
            }
        }


    }
    void Update()
    {
        CheckTouchDamage();
    }

    private void CheckTouchDamage()
    {
        if (Time.timeSinceLevelLoad >= lastTouchDamageTime + touchDamageCooldown && PH.invulnerable == false)
        {

            touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

            Collider2D hit = Physics2D.OverlapArea(touchDamageBotLeft, touchDamageTopRight, whatIsPlayer);
            if (hit != null)
            {
                lastTouchDamageTime = Time.timeSinceLevelLoad;
                attackDetails[0] = touchDamage;
                attackDetails[1] = alive.transform.position.x;
                // Debug.Log("Damage" + attackDetails[0] + " " + attackDetails[1]);
                PH.Damage(attackDetails);

            }
        }
    }

    public void OnDrawGizmos()
    {
        touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 topRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

 
        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);

    }
}
