using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActivator : MonoBehaviour
{

    public GameObject DialogBox;
    DialogController dc;

    public bool isLevel4;
    // Start is called before the first frame update
    void Start()
    {
        dc = DialogBox.GetComponent<DialogController>();
        if (dc == null)
        {
            Debug.Log("DialogBox doesn't have a dialog controller!");
        }
    }

    // Update is called once per frame
    
    void OnEnable() {
        if(dc == null && isLevel4)
        {
            dc = DialogBox.GetComponent<DialogController>();
        }
        StartCoroutine(dc.typeSentence());
    }


}
