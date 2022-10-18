using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextCutscene : MonoBehaviour
{
    [TextArea(5, 10)]
    public string[] dialogue;
    public TextMeshProUGUI textbox;
    private int index;
    private bool finishedTyping;

    public string sceneName;
    public AudioSource music;

    public GameObject enterKey;

    public int exitTime = 4;

    public bool finalLevel;
    public TextMeshProUGUI endingtext;
    void Start()
    {
        index = 0;
        StartCoroutine(SoundControl.startFade(music, 4f, 1f));
        StartCoroutine(typeText(6f));
    }

    IEnumerator typeText(float time)
    {
        List<string> words = new List<string> (dialogue[index].Split(' '));

        finishedTyping = false;
        yield return new WaitForSeconds(time);
        textbox.text = "";
//        finishedTyping = false;
        //yield return new WaitForSeconds(0.2f); //make text empty before moving to next 
        foreach(string str in words)
        {
            //if(str == "red")

            foreach(char c in str)
            {
                textbox.text += c;
                yield return new WaitForSeconds(0.06f);
            }

      //      Debug.Log(str);
            //Debug.Log(textbox.text.Replace(str, "<color=green> green </color>"));
            if (str.Contains("green"))
            {
                textbox.text = textbox.text.Replace("green", "<color=green>green</color>");
            }
            else if (str == "red" || str == "red.")
            {
                textbox.text = textbox.text.Replace("red", "<color=red>red</color>");
            }
            else if(str.Contains("Giorgio"))
            {
                textbox.text = textbox.text.Replace("Giorgio", "<color=blue>Giorgio</color>");
            }
            else if(str.Contains("child"))
            {
                textbox.text = textbox.text.Replace("child", "<color=blue>child</color>");
            }

            textbox.text += " ";
            yield return new WaitForSeconds(0.06f);
        }

        yield return new WaitForSeconds(0.4f); //give some delay so its not too fast
        enterKey.SetActive(true);
        finishedTyping = true;
        index++;

    }

    private void Update()
    {
        if( (Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape)) && finishedTyping)
        {
            enterKey.SetActive(false);
            if (index < dialogue.Length)
            {
                StartCoroutine(typeText(0));
            }
            else if(index >= dialogue.Length)
            {
                
                finishedTyping = false;
                textbox.text = ""; 
                StartCoroutine(SoundControl.startFade(music, exitTime, 0f));
                if (finalLevel)
                {
                    StartCoroutine(finalMessage());
                }
                else
                {
                    Invoke("beginLevel", exitTime);
                }
            }
        }
    }

    IEnumerator finalMessage()
    {
        endingtext.color = new Color(1, 1, 1, 0);
        yield return new WaitForSeconds(5f);
        for (float i = 0; i <= 60; i++)
        {
            endingtext.color = new Color(1, 1, 1, i / 60.0f);
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(3);
        for (float i = 60; i >= 0; i--)
        {
            endingtext.color = new Color(1, 1, 1, i / 60.0f);
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(3);
        beginLevel();
        
    }

    void beginLevel()
    {
        MainMenu.PlayGame(sceneName);
    }




}
