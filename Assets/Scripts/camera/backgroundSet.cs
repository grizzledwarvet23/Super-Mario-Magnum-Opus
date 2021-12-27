using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backgroundSet : MonoBehaviour
{
    new Transform camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Main Camera").transform;
    }

    void Awake()
    {
        Set();
        /*
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = renderer.sprite.bounds.size;

        Vector2 scale = transform.localScale;
        if (cameraSize.x >= cameraSize.y)
        {
            scale *= cameraSize.x / spriteSize.x / 2;
        }
        else
        {
            scale *= cameraSize.y / spriteSize.y / 2;
        }

        transform.localScale = scale;
        */

    }



    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(camera.position.x, camera.position.y, camera.position.z + 5.5f);
    }
  public  void Set()
    {
        SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
        float cameraHeight = Camera.main.orthographicSize * 2;
        Vector2 cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        Vector2 spriteSize = renderer.sprite.bounds.size;

        Vector2 scale = transform.localScale;
        if (cameraSize.x >= cameraSize.y)
        {
            scale *= cameraSize.x / spriteSize.x / 2;
        }
        else
        {
            scale *= cameraSize.y / spriteSize.y / 2;
        }

        transform.localScale = scale;
    }
}
