using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    //----------Starts Dialogue when entering the trigger----------
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponentInParent<TutorialPlayer>().checkpoint = gameObject.transform;
        FindFirstObjectByType<DialogueManager>().StartDialogue(dialogue);
    }
}
