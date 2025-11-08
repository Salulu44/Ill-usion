using UnityEngine;
using UnityEngine.SceneManagement;

public class DoodleCameraScript : MonoBehaviour
{
    private Transform player;
    private void OnEnable()
    {
        player = GameObject.FindWithTag(GameManagerScript.Instance.tagSO.playerTag).transform;
    }
    // Update is called once per frame
    private void Update()
    {
        //Vector3 playerScreenPos = Camera.main.WorldToScreenPoint(player.position);
        //if (playerScreenPos.x >= Screen.width)
        //{
        //    print("Right");
        //    Vector3 playerDestinedPosition = Camera.main.ScreenToWorldPoint(new Vector3(1, 0, 0));
        //    player.position = new Vector3(playerDestinedPosition.x, transform.position.y, 0);
        //}
        //else if (playerScreenPos.x < 0)
        //{
        //    print("Left");
        //    Vector3 playerDestinedPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 10, 0, 0));
        //    player.position = new Vector3(playerDestinedPosition.x, transform.position.y, 0);
        //}
        StayInViewPort(player);
        if (Input.GetKeyDown(KeyCode.Return)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void StayInViewPort(Transform gameObject) 
    {
        Vector3 gameObjectScreenPosition = Camera.main.WorldToScreenPoint(gameObject.position);
        if (gameObjectScreenPosition.x >= Screen.width)
        {
            Vector3 gameObjectDestinedPosition = Camera.main.ScreenToWorldPoint(new Vector3(1, 0, 0));
            gameObject.position = new Vector3(gameObjectDestinedPosition.x, gameObject.position.y, 0);
        }
        else if (gameObjectScreenPosition.x < 0)
        {
            Vector3 playerDestinedPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 10, 0, 0));
            gameObject.position = new Vector3(playerDestinedPosition.x, gameObject.position.y, 0);
        }
    }
    private void LateUpdate()
    {
        if (player.transform.position.y >= transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y, -10);
        }
    }
}
