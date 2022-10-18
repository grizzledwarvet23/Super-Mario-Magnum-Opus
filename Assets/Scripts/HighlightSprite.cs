using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightSprite : MonoBehaviour
{
    public Image image;
    public Sprite defaultSprite;
    public Sprite highlighted;

    void Unhighlight()
    {
        image.sprite = defaultSprite;
    }
    void Highlight()
    {
        image.sprite = highlighted;
    }

}
