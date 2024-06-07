using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;

    public TMP_Text title;
    public TMP_Text dialogueText;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            DisplayNextSentence();
        }
    }
    //----------Starts the set of dialogue----------
    public void StartDialogue(Dialogue dialogue)
    {
        title.text = dialogue.name;
        sentences.Clear();
        foreach (string sentence in dialogue.sentences) 
        { 
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    //----------Goes thru the set of dialogue----------
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            dialogueText.text = "Try it out yourself!";
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueText.text = sentence;
    }
}
