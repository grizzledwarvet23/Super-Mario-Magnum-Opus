using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class DialogController : MonoBehaviour
{
    public bool wantToStartAtBeginning;

    public PlayableDirector[] timelines;

    public PlayableDirector fightIntroTimeline; //used in level 4 to transition into fight with JB

    [TextArea(3, 10)]
    public string[] sentences;
    public string[] speakers;
    int currentTimeline;
    int currentSentence;

    public AudioSource voice;
    public AudioSource voice2; //for giorgio speak in level 4
    public AudioSource optionalVoiceClip;

    bool doneWithSentence;

    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI speakerText;

    private float textSpeed = 0.02f;

    public GameObject DialogActivator;

    float startTime;

    public int timelineStart;
    public int sentenceStart;

    public int respawnTimeline;

    public Sprite[] JBTalkingAnimation;

    public GameObject JB;
    bool JBisSpeaking;

    public Animator blackFade; //used in level4

    // Start is called before the first frame update
    void Start()
    {
        textSpeed = 0.02f;
        GameObject gm = GameObject.FindGameObjectWithTag("GM");
        if (gm != null && gm.GetComponent<GameMaster>().finishedCutscene)
        {
            timelineStart = respawnTimeline; // 12;
            sentenceStart = respawnTimeline; // 12;
        }

        JBisSpeaking = false;
        doneWithSentence = false;
        if ( (DialogActivator.GetComponent<DialogActivator>().isLevel4 ||  DialogActivator.GetComponent<DialogActivator>().isLevel5) 
            && timelineStart >= timelines.Length)
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
        if (((Input.anyKeyDown && !Input.GetKeyDown(KeyCode.Escape)) && (!(timelines[currentTimeline].state == PlayState.Playing) 
            || startTime + timelines[currentTimeline].duration < Time.time) && doneWithSentence) 

            || (timelines[currentTimeline].tag == "TransitionTimeline" && (timelines[currentTimeline].state != PlayState.Playing || 
            (timelines[currentTimeline].extrapolationMode == DirectorWrapMode.Hold && startTime + timelines[currentTimeline].duration < Time.time))))
        {

            if (currentTimeline + 1 >= timelines.Length)
            {
                if (DialogActivator.GetComponent<DialogActivator>().isLevel4 || DialogActivator.GetComponent<DialogActivator>().isLevel5)
                {
                    if (timelines[timelines.Length - 1].tag == "TransitionTimeline") //outro out of level 4
                    {
                        if (DialogActivator.GetComponent<DialogActivator>().isLevel4)
                        {
                            SaveSystem.saveLevel(4);
                        }
                        if (DialogActivator.GetComponent<DialogActivator>().isLevel5)
                        {
                            SaveSystem.saveLevel(5);
                            MainMenu.PlayGame("Epilogue");
                        }
                        else
                        {
                            MainMenu.PlayGame("Menu");
                        }
                    }
                    else {
                        timelines[currentTimeline].Stop();
                        currentTimeline += 1;
                        if (gameObject.name == "Luigi_DialogBox")
                        {
                            luigiOut();
                        }
                        else
                        {
                            fightIntro();
                        }
                    }
                }
                else
                {
                    MainMenu.PlayGame("Menu");
                }
            }
            else
            {
                dialogText.text = "";
                speakerText.text = "";
                DialogActivator.SetActive(false);
                timelines[currentTimeline].Stop();
                currentTimeline += 1;
                timelines[currentTimeline].Play();
                startTime = Time.time;
            }

        }

    }

    //used in level 4
    private void fightIntro()
    {
        dialogText.text = "";
        speakerText.text = "";
        fightIntroTimeline.Play();
        GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>().finishedCutscene = true;
        GameObject player = GameObject.Find("player1");
        player.GetComponent<PlayerHealth>().unlockControl();
        gameObject.GetComponent<DialogController>().enabled = false;
    }

    //used in level 4
    private void luigiOut()
    {
        fightIntroTimeline.Play(); //used the same variable for luigi
        GameObject player = GameObject.Find("player1");
        player.GetComponent<PlayerHealth>().unlockControl();
    }

    //used in level 4 and 5 after fight
    public IEnumerator outro(int level, GameObject boss)
    {
        currentTimeline = 0;
        currentSentence = 0;

        blackFade.Play("whiteFadeIn");

        if (level == 4)
        {
            yield return new WaitForSeconds(1f);
            // JB.GetComponent<Animator>().runtimeAnimatorController = null;
            JB.GetComponent<SpriteRenderer>().sprite = JBTalkingAnimation[4];
            JB.GetComponent<SpriteRenderer>().sortingOrder = 1;
            //indices 4 and 5 have sad JB
            JBTalkingAnimation[0] = JBTalkingAnimation[4];
            JBTalkingAnimation[1] = JBTalkingAnimation[5];

            DialogActivator.SetActive(false);
            startTime = Time.time;
            timelines[currentTimeline].Play();
            enabled = true; //the update function of this class is enabled

            GameObject player = GameObject.Find("player1");
            player.GetComponent<PlayerHealth>().lockControl(!player.GetComponent<Move2D>().facingRight);
            player.transform.eulerAngles = new Vector3(0, 0, 0);
            player.GetComponent<SpriteRenderer>().color = Color.white;
            player.GetComponent<Animator>().SetBool("isGrounded", true);
            player.GetComponent<Animator>().SetBool("isShooting", false);
            player.GetComponent<Animator>().speed = 1;
            player.transform.position = new Vector2(2063.84f, 326.53f);
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            yield return new WaitForSeconds(2f);
            blackFade.Play("whiteFadeOut");
        }
        else if (level == 5) //post giorgio
        {
            yield return new WaitForSeconds(0.5f);
            GameObject player = GameObject.Find("player1");
            player.GetComponent<PlayerHealth>().lockControl(false);

            DialogActivator.SetActive(false);
            startTime = Time.time;
            timelines[currentTimeline].Play();
            enabled = true;
            yield return new WaitForSeconds(2);

            boss.transform.position = new Vector2(965, -0.96f);
            boss.transform.eulerAngles = new Vector3(0, 180, 0);
            boss.GetComponent<Animator>().Play("giorgio_wounded");

            
  
            if (!player.GetComponent<Move2D>().facingRight)
            {
                player.GetComponent<Move2D>().flip();
            }
            //player.GetComponent<PlayerHealth>().lockControl(!player.GetComponent<Move2D>().facingRight);
            player.transform.eulerAngles = new Vector3(0, 0, 0);
            //player.GetComponent<SpriteRenderer>().color = Color.white;
            player.GetComponent<Animator>().SetBool("isGrounded", true);
            player.GetComponent<Animator>().SetBool("isShooting", false);
            player.GetComponent<Animator>().speed = 1;
            player.transform.position = new Vector2(947, 0);
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            yield return new WaitForSeconds(2);
            blackFade.Play("whiteFadeOut");
        }
    }

    public IEnumerator typeSentence() {
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
        else if (speakers[currentSentence] == "???")
        {
            textSpeed = 0.02f;
            speakerText.color = Color.yellow;
        }
        else if (speakers[currentSentence] == "??")
        {
            if (DialogActivator.GetComponent<DialogActivator>().isLevel5)
            {
                textSpeed = 0.02f;
            }
            else
            {
                textSpeed = 0.1f;
            }
            speakerText.color = Color.blue;
        }
        else if (speakers[currentSentence] == "Luigi")
        {
            textSpeed = 0.02f;
            speakerText.color = Color.green;
        }
        else
        {
            textSpeed = 0.02f;
            speakerText.color = Color.blue;
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
            if (openMouth && JBisSpeaking && c != '!')
            {
                JB.GetComponent<SpriteRenderer>().sprite = JBTalkingAnimation[index + 1];
            }
            else if (!openMouth && JBisSpeaking) {
                JB.GetComponent<SpriteRenderer>().sprite = JBTalkingAnimation[index];
            }
            if (counter % 3 == 0) {
                if (speakers[currentSentence] == "??")
                {
                    voice2.Play();
                }
                else
                {
                    voice.Play();
                }
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
                yield return new WaitForSeconds(textSpeed);
            }


        }

        if (JB != null)
        {
            JB.GetComponent<SpriteRenderer>().sprite = JBTalkingAnimation[index];
        }
        JBisSpeaking = false;
        doneWithSentence = true;
        currentSentence += 1;
    }
}
