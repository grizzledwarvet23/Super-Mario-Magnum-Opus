using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollingBackground : MonoBehaviour
{
    public float speed = 4f;
    public Renderer bgRend;

    private void Start()
    {
        
        bgRend.sortingOrder = -10;
    }

    private void Update()
    {
        bgRend.material.mainTextureOffset += new Vector2(speed * Time.deltaTime, 0f);
    }
}
