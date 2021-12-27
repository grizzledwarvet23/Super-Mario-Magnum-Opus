using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBlast : MonoBehaviour
{
    public GameObject bullet;
    public GameObject[] firePoints;
    public AudioSource gunSound;

    public void Fire()
    {
        gunSound.Play();
        foreach (GameObject point in firePoints)
        {
            Instantiate(bullet, point.transform.position, Quaternion.Euler(new Vector3(0, point.transform.eulerAngles.y, 0)));
            Instantiate(bullet, point.transform.position, Quaternion.Euler(new Vector3(0, point.transform.eulerAngles.y, -15)));
            Instantiate(bullet, point.transform.position, Quaternion.Euler(new Vector3(0, point.transform.eulerAngles.y, 15)));
        }

    }








}
