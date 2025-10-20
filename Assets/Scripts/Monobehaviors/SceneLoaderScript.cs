using UnityEngine;

public class SceneLoaderScript : MonoBehaviour
{
    [SerializeField] string sceneName;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(GameManagerScript.Instance.tagSO.playerTag == "Player") 
        {

        }
    }
}
