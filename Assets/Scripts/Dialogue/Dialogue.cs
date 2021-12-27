using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialogue
{

    public string speaker;

    public AudioSource voice;


    [TextArea(3, 10)]
    public string[] sentences;
    


}
