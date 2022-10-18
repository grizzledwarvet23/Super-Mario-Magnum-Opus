using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class CutoutMaskUI : Image
{
    /** This script is used in level 4 during 
    the outro cutscene in order to create an
    image mask that creates a transparent hole
    (esque of james bond openings). this is for
    the cutscene part where JB is sniped
    **/
    public override Material materialForRendering
    {
        get
        {
            Material material = new Material(base.materialForRendering);
            material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            return material;
        }
    }
}
