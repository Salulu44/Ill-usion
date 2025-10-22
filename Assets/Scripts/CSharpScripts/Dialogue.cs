using UnityEngine;
[System.Serializable]
public class Dialogue
{

    public DialogueLine[] dialogueLines;
    [TextArea(3, 10)]
    //public string[] transitionLineText;
    //public AudioClip[] transitionLineAudioClip;
    //public float[] transitionAudioVolume;
    public SFX[] SFXs;
    [System.Serializable]
    public class SFX
    {
        public float pitch;
    }

}
[System.Serializable]
public class DialogueLine
{
    public string dialogueID;
    //public bool hasDecision;
    public string nextDialogueID;
    public string speaker;
    [TextArea(3, 10)]
    public string textContent;
    public Sprite sprite;
    public AudioClip audioClip;
    [SerializeField] private float audioVolume;
    public float AudioVolume { get { return audioVolume; } set { if (value <= 0) audioVolume = 0; else audioVolume = value; } }
    public bool hasSpecialEffect;
    public DialogueChoice[] choices = new DialogueChoice[3];

}
[System.Serializable]
public struct DialogueChoice
{
    public string choiceText;
    public int severity;
    public string nextDialogueID;
}

////
////  Dialogue.asset – speichert einen kompletten Dialog (z. B. ein Gespräch)
////
//[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Asset")]
//public class Dialogue : ScriptableObject
//{
//    [Header("Dialogue Info")]
//    public string dialogueName;
//    [Tooltip("Liste aller Dialogzeilen in der richtigen Reihenfolge")]
//    public DialogueLine[] lines;
//}

////
////  Eine einzelne Zeile innerhalb eines Dialogs
////
//[System.Serializable]
//public class DialogueLine
//{
//    [Header("Basic Info")]
//    public string speakerName;

//    [TextArea(3, 10)]
//    public string text;

//    [Header("Visuals & Audio")]
//    public Sprite portrait;

//    public AudioClip voiceClip;
//    [Range(0f, 1f)] public float voiceVolume = 1f;

//    public SFXData sfx;

//    [Header("Flow Control")]
//    [Tooltip("Wenn true, gibt es Auswahlmöglichkeiten (z. B. Ja/Nein).")]
//    public bool hasDecision;
//    [Tooltip("Liste der möglichen Antworten, falls hasDecision true ist.")]
//    public DialogueDecision[] decisions;

//    [Tooltip("Index der nächsten Zeile im Dialogue.lines Array (-1 = Ende).")]
//    public int nextLineIndex = -1;

//    [Header("Special Effects")]
//    public bool triggerSpecialEffect;
//    [Tooltip("Ein Event oder Effektname, der von deinem System verarbeitet werden kann.")]
//    public string specialEffectName;
//}

////
////  Eine mögliche Entscheidung des Spielers während des Dialogs
////
//[System.Serializable]
//public class DialogueDecision
//{
//    public string choiceText;
//    [Tooltip("Index im Dialogue.lines Array, zu dem diese Entscheidung führt.")]
//    public int nextLineIndex;
//}

////
////  Optionale Soundeffekte, die zusätzlich zu Voice Clips abgespielt werden
////
//[System.Serializable]
//public class SFXData
//{
//    public bool playSFX;
//    public AudioClip clip;
//    [Range(-3f, 3f)] public float pitch = 1f;
//    [Range(0f, 1f)] public float volume = 1f;
//}
