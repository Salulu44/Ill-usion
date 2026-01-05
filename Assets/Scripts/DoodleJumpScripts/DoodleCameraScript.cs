using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoodleCameraScript : MonoBehaviour
{
    private Transform player;
   [SerializeField] private bool shouldPlayerInViweport = true;
    [SerializeField] float duration;
    [SerializeField] private float magnitude;
    public event Action OnScreenExit;
    bool isShaking;
    private void OnEnable()
    {
        GameObject[] allPlayerInstances = GameObject.FindGameObjectsWithTag(GameManagerScript.Instance.tagSO.playerTag);
        foreach(GameObject playerInstance in allPlayerInstances) 
        {
            if(playerInstance.TryGetComponent(out DoodlePlayerScript doodlePlayerScript)) 
            {
                player = playerInstance.transform;
                break;
            }
        }
    }
    public IEnumerator CameraShake(float duration, float magnitude) 
    {
        isShaking = true;
        Vector3 originalPos = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration) 
        {
            transform.position = new Vector3(originalPos.x + UnityEngine.Random.Range(-1, 1) * magnitude, originalPos.y + UnityEngine.Random.Range(-1, -1) * magnitude, -10);
            Debug.Log("original pos " + originalPos);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isShaking = false;
    }
    // Update is called once per frame
    private void Update()
    {

        if (shouldPlayerInViweport) 
        {
            StayInViewPort(player);
        }
        if (Input.GetKeyDown(KeyCode.Return)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ShouldPlayerStayInViewPort(bool stay) 
    {
        shouldPlayerInViweport = stay;
    }
    public void StayInViewPort(Transform gameObject) 
    {
        Vector3 gameObjectScreenPosition = Camera.main.WorldToScreenPoint(gameObject.position);
        if (DoodleSpawnerScript.instance.spawnVertically)
        {
            if (gameObjectScreenPosition.x >= Screen.width)
            {
                Vector3 gameObjectDestinedPosition = Camera.main.ScreenToWorldPoint(new Vector3(1, 0, 0));
                gameObject.position = new Vector3(gameObjectDestinedPosition.x, gameObject.position.y, 0);
                if(gameObject.tag == GameManagerScript.Instance.tagSO.playerTag) 
                {
                    gameObject.GetComponent<Rigidbody2D>().linearVelocityX *= .25f;
                    OnScreenExit?.Invoke();
                }
            }
            else if (gameObjectScreenPosition.x < 0)
            {
                Vector3 playerDestinedPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 10, 0, 0));
                gameObject.position = new Vector3(playerDestinedPosition.x, gameObject.position.y, 0);
                if (gameObject.tag == GameManagerScript.Instance.tagSO.playerTag)
                {
                    gameObject.GetComponent<Rigidbody2D>().linearVelocityX *= .5f;
                    OnScreenExit?.Invoke();
                }
            }
        }
        else 
        {
            if (gameObjectScreenPosition.y >= Screen.height)
            {
                Vector3 gameObjectDestinedPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, 20, 0));
                gameObject.position = new Vector3(gameObject.position.x, gameObjectDestinedPosition.y, 0);
            }
            else if (gameObjectScreenPosition.y < 0)
            {
                Vector3 playerDestinedPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height - 10, 0));
                gameObject.position = new Vector3(gameObject.position.x, playerDestinedPosition.y, 0);
            }
        }
    }
    private void LateUpdate()
    {
        CameraMovement();
    }
    public void CameraMovement() 
    {
        if (shouldPlayerInViweport)
        {
            if (player.transform.position.y >= transform.position.y && DoodleSpawnerScript.instance.spawnVertically)
            {
                transform.position = new Vector3(transform.position.x, player.transform.position.y, -10);
            }
            else if (player.transform.position.x >= transform.position.x && !DoodleSpawnerScript.instance.spawnVertically)
            {
                transform.position = new Vector3(player.transform.position.x, transform.position.y, -10);
            }
        }
        else
        {

            transform.position = new Vector3(player.position.x, player.position.y, -10);
        }
        // I want camera shake but it doesnt do what i want it to do
        //if (DoodleScoreScript.hasWon && !isShaking)
        //{


        //}
        //if (!isShaking)
        //{
        //    StartCoroutine(CameraShake(duration, magnitude));
        //}

    }
}
