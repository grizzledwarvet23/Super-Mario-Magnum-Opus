using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public int sceneNumber;
    public Dialogue dialogue;

    private Queue<string> sentences;   //FIFO




    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        StartDialogue(dialogue);
    }

    public void StartDialogue(Dialogue dialogue)
    {

        Debug.Log("Starting conversation with " + dialogue.speaker);

        nameText.text = dialogue.speaker;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence(dialogue.voice);
    }

    public void DisplayNextSentence(AudioSource voice)
    {
        if (sentences.Count == 0) // reached end of queue
        {
            EndDialogue();
            if (sceneNumber != null)
            {
                SceneManager.LoadScene(sceneNumber);
            }
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence, voice));
        //dialogueText.text = sentence;
    }

    IEnumerator TypeSentence (string sentence, AudioSource voice)
    {
        dialogueText.text = "";
        int counter = 0;
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            if (counter % 3 == 0)
            {
                voice.Play();
            }
            yield return new WaitForSeconds(0.02f); //speed of scrolling text
            counter++;
        }
    }

    void EndDialogue()
    {
        Debug.Log("End of conservation.");
    }


}
