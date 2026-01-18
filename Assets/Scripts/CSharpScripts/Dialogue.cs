using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
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
    public DialogueLine severeLine;

}
[System.Serializable]
public class UpperCaseString 
{
    [SerializeField] private string value;
    public static implicit operator string(UpperCaseString upper) => upper?.value?.ToUpper() ?? "";

    public static implicit operator UpperCaseString(string s) => new UpperCaseString { value = s?.ToUpper() };
    public override string ToString() => value?.ToUpper() ?? "";
}
[System.Serializable]
public class DialogueLine
{
    public UpperCaseString dialogueID;
    //public bool hasDecision;
    public UpperCaseString nextDialogueID;
    public string speaker;
    [TextArea(3, 10)]
    public string textContent;
    public Sprite sprite;
    public AudioClip audioClip;
    [SerializeField] private float audioVolume;
    public float AudioVolume { get { return audioVolume; } set { if (value <= 0) audioVolume = 0; else audioVolume = value; } }
    public bool hasSpecialEffect;
    public DialogueChoice[] choices = new DialogueChoice[3];
    //public void ToUpperIDs()
    //{
    //    if (!string.IsNullOrEmpty(dialogueID))
    //        dialogueID = dialogueID.ToUpper();
    //    if (!string.IsNullOrEmpty(nextDialogueID))
    //        nextDialogueID = nextDialogueID.ToUpper();
    //}
}
[System.Serializable]
public struct DialogueChoice
{
    public string choiceText;
    public int severity;
    public UpperCaseString nextDialogueID;
}
