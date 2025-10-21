using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandlerScript : MonoBehaviour
{
    private Scene mainScene;
    private bool mainSceneOn = true;
    void Start()
    {
        mainScene = SceneManager.GetSceneByName("MainGame");
        Debug.Assert(mainScene != null);
        print(transform.root.gameObject.name);
    }
    void Update()
    { 
    
    }
    public void LoadMinigame(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        mainSceneOn = !mainSceneOn;
        operation.completed += OperationCompleted;
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(sceneName));
    }
    public void QuitScene(string sceneName) 
    {
        mainSceneOn = !mainSceneOn;
        AsyncOperation operation =  SceneManager.UnloadSceneAsync(sceneName);
        SceneManager.MoveGameObjectToScene(gameObject,mainScene);
        operation.completed += OperationCompleted;
    }
    private void OperationCompleted(AsyncOperation obj)
    {
        SetActiveForScene(mainScene, mainSceneOn);
        obj.completed -= OperationCompleted;
    }
    void SetActiveForScene(Scene scene, bool active)
    {
        foreach (GameObject gameObject in scene.GetRootGameObjects()) 
        {
            if(gameObject.tag == GameManagerScript.Instance.tagSO.rootTag) 
            {
                gameObject.SetActive(active);
                break;
            }
        }
    }
}
