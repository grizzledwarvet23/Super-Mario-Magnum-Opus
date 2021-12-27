using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateDirection : MonoBehaviour
{
    public Rigidbody2D rb;

    public Vector2 timeRange;

    public Patrol script;


    private void Start()
    {
        StartCoroutine(alternate());
    }
    IEnumerator alternate()
    {
        yield return new WaitForSeconds(Random.Range(timeRange.x, timeRange.y));
        script.dir *= -1;
        StartCoroutine(alternate());
    }


}
