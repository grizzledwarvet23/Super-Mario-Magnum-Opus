using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.SceneManagement;


public class DialogController : MonoBehaviour
{
    public bool wantToStartAtBeginning;

    public PlayableDirector[] timelines;

    public PlayableDirector outroTimeline; //used in level 4 to transition into fight with JB

    [TextArea(3, 10)]
    public string[] sentences;
    public string[] speakers;
    int currentTimeline;
    int currentSentence;

    public AudioSource voice;
    public AudioSource optionalVoiceClip;

    bool doneWithSentence;

    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI speakerText;

    public GameObject DialogActivator;

    float startTime;

    public int timelineStart;
    public int sentenceStart;

    public Sprite[] JBTalkingAnimation;

    public GameObject JB;
    bool JBisSpeaking;


    // Start is called before the first frame update
    void Start()
    {
        GameObject gm = GameObject.FindGameObjectWithTag("GM");
        if (gm != null && gm.GetComponent<GameMaster>().finishedCutscene)
        {
            timelineStart = 12;
            sentenceStart = 12;
        }

        JBisSpeaking = false;
        doneWithSentence = false;
        if (DialogActivator.GetComponent<DialogActivator>().isLevel4 && timelineStart >= timelines.Length)
        {
            fightIntro();
        }
        else
        {
            timelines[timelineStart].Play();
        }
        currentTimeline = timelineStart;
        currentSentence = sentenceStart;
        if (wantToStartAtBeginning)
        {
            StartCoroutine(typeSentence());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && (!(timelines[currentTimeline].state == PlayState.Playing) || startTime + timelines[currentTimeline].duration < Time.time) && doneWithSentence) || (timelines[currentTimeline].tag == "TransitionTimeline" && !(timelines[currentTimeline].state == PlayState.Playing)))
        {

            if (currentTimeline + 1 >= timelines.Length) {
                if (DialogActivator.GetComponent<DialogActivator>().isLevel4)
                {
                    timelines[currentTimeline].Stop();
                    currentTimeline += 1;
                    fightIntro();
                }
                else
                {
                    MainMenu.PlayGame("Menu");
                }
            }
            dialogText.text = "";
            speakerText.text = "";
            DialogActivator.SetActive(false);
            timelines[currentTimeline].Stop();
            currentTimeline += 1;
            timelines[currentTimeline].Play();
            startTime = Time.time;

        }

    }

    //used in level 4
    private void fightIntro()
    {
        outroTimeline.Play();
        GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>().finishedCutscene = true;
        GameObject player = GameObject.Find("player1");
        player.GetComponent<PlayerHealth>().unlockControl();
        gameObject.GetComponent<DialogController>().enabled = false;
    }

    public IEnumerator typeSentence() {
        //   Debug.Log("type");
        int index = 0;
        dialogText.text = "";
        speakerText.text = "";
        if (speakers[currentSentence].Contains("JB") /*== "JB"*/)
        {
            JBisSpeaking = true;
            speakerText.color = Color.yellow;
            if (speakers[currentSentence].Contains("Angry"))
            {
                index = 2;
                JB.GetComponent<SpriteRenderer>().sprite = JBTalkingAnimation[index];
                speakers[currentSentence] = "JB";
            }
            else
            {
                index = 0;
            }

        }
        else if (speakers[currentSentence] == "???") {
            speakerText.color = Color.yellow;
        }
        else
        {
            speakerText.color = Color.green;
        }
        if (optionalVoiceClip != null) {
            optionalVoiceClip.Play();
            yield return new WaitForSeconds(3f);
            optionalVoiceClip = null;
        }
        doneWithSentence = false;
        int counter = 0;
        speakerText.text = speakers[currentSentence];
        bool openMouth = true;
        foreach (char c in sentences[currentSentence].ToCharArray()) {


            dialogText.text += c;
            if ( openMouth && JBisSpeaking && c != '!')
            {
                JB.GetComponent<SpriteRenderer>().sprite = JBTalkingAnimation[index+1];
            }
            else if ( !openMouth && JBisSpeaking) {
                JB.GetComponent<SpriteRenderer>().sprite = JBTalkingAnimation[index];
            }
                if (counter % 3 == 0) {
                voice.Play();
            }
            counter++;
            if (c == ' ') {
                openMouth = !openMouth;
            }
            if (c == '.')
            {
                yield return new WaitForSeconds(0.15f);
            }
            else if (c == '!') 
            {
                yield return new WaitForSeconds(0.15f);
            }
            else
            {
                yield return new WaitForSeconds(0.02f);
            }


        }

        JB.GetComponent<SpriteRenderer>().sprite = JBTalkingAnimation[index];
        JBisSpeaking = false;
        doneWithSentence = true;
        currentSentence += 1;
    
    
    
    
    }
}
