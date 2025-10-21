using UnityEngine;

public class SceneLoaderScript : MonoBehaviour
{
    [SerializeField] string sceneName;
    [SerializeField] SceneHandlerScript sceneHandler;
    [SerializeField] GameObject dialogue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && transform.GetChild(0).gameObject.activeSelf && !dialogue.activeSelf) 
        {
            print("Hiii");
            dialogue.SetActive(true);
          StartCoroutine(GetComponent<DialogueScript>().RunDialogue());
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
    public void StartMinigame(string sceneName) 
    {
        sceneHandler.LoadMinigame(sceneName);
    }
}
