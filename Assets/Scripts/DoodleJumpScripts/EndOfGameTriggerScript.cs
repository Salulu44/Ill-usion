using UnityEngine;

public class EndOfGameTriggerScript : MonoBehaviour
{
    [SerializeField] VideoScript videoScript;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == GameManagerScript.Instance.tagSO.playerTag)
        {
            VideoManagerScript.Instance.PlayVideo(videoScript);
            //Maybe freeze the Player, so that he cant do anything funny
            videoScript.OnVideoStop += EndMinigame;
        }
    }
    void EndMinigame()
    {
        SceneHandlerScript.LoadScene("MainGame");
    }
}
