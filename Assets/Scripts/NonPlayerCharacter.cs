using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerCharacter : MonoBehaviour
{
    // Variable utilizada para poder dar un di�logo distinto a cada elemento
    public string dialogueText;

    // Variable utilizada para poder dar un tiempo distinto a cada di�logo
    public float displayTime;

    // Variable utilizada para saber si se ha hablado con los NPCs necesarios
    public bool talkedTo;
}
