using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeBar : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Slider>().value = AudioListener.volume;
    }
}
