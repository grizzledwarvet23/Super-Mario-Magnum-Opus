using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class triggerSpritemaskUse : MonoBehaviour
{
    public Tilemap obj;
    // Start is called before the first frame update
    void Start()
    {
        obj.GetComponent<TilemapRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;   
    }


}
