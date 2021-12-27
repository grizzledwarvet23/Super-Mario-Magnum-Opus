using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIDisplay : MonoBehaviour
{
    public TextMeshProUGUI txt;
    GameObject player;
  //  public GameObject childObject;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player1");
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        txt = gameObject.GetComponent<TextMeshProUGUI>();
        

    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            txt.text = "   X " + player.GetComponent<CoinCollection>().items;
        }
    }
}
