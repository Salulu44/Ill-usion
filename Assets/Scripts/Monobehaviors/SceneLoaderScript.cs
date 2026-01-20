using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderScript : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] SceneHandlerScript sceneHandler;
    [SerializeField] GameObject dialogue;
    void Start()
    {
        GetComponent<DialogueScript>().OnEndDialogue += StartMinigame;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && transform.GetChild(0).gameObject.activeSelf && !dialogue.activeSelf) 
        {
          //dialogue.SetActive(true);
          GetComponent<DialogueScript>().StartDialogue();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == GameManagerScript.Instance.tagSO.playerTag) 
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == GameManagerScript.Instance.tagSO.playerTag)
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    void StartMinigame() 
    {
        sceneHandler.LoadMinigame(sceneName);
        GetComponent<Collider2D>().enabled = false;
    }
    public void EndMiniGame() 
    {
        sceneHandler.QuitScene(sceneName);
        GetComponent<Collider2D>().enabled = true;
    }
}
