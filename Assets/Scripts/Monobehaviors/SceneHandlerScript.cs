using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandlerScript : MonoBehaviour
{
    [SerializeField] private Scene mainScene;
    private bool mainSceneOn = true;
    string nextScene;
    void Start()
    {
        mainScene = SceneManager.GetSceneByName("MainGame");
        Debug.Assert(mainScene != null);
    }
    void Update()
    { 
    
    }
    //I want to approach it now with normal loading



    public void LoadMinigame(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }




    //This was the approach with LoadSceneMode.additive
    //public void LoadMinigame(string sceneName)
    //{
    //    AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    //    mainSceneOn = !mainSceneOn;
    //    operation.completed += OperationCompleted;
    //    SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(sceneName));
    //    nextScene = sceneName;
    //}
    /*    public async void LoadMinigame(string sceneName)
    {
        try
        {
            mainSceneOn = !mainSceneOn;
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            while (!operation.isDone && !destroyCancellationToken.IsCancellationRequested)
            {
                await Task.Yield();
            }
            if (destroyCancellationToken.IsCancellationRequested) return;
            Scene targetScene = SceneManager.GetSceneByName(sceneName);
            if (!targetScene.IsValid()) return;
            SceneManager.SetActiveScene(targetScene);
            SceneManager.MoveGameObjectToScene(gameObject, targetScene);
            OperationCompleted(operation);
        }
        catch (Exception excep)
        {
            Debug.LogError($"LoadMinigame Error: {excep.Message}");
        }
    }
*/  

    //public void QuitScene(string sceneName) 
    //{
    //    Debug.Log(SceneManager.GetActiveScene().name);
    //   if(SceneManager.GetActiveScene().name != sceneName) 
    //    {
    //        Debug.Log("You are not in the MinigameScene");
    //        return;
    //    }
    //    mainSceneOn = !mainSceneOn;
    //    nextScene = mainScene.name;
    //    AsyncOperation operation =  SceneManager.UnloadSceneAsync(sceneName);
    //    SceneManager.MoveGameObjectToScene(gameObject,mainScene);
    //    operation.completed += OperationCompleted;
    //}
    //private void OperationCompleted(AsyncOperation obj)
    //{
    //    SetActiveForScene(mainScene, mainSceneOn);
    //    Debug.Log("This is the next scene " + nextScene);
    //    SceneManager.SetActiveScene(SceneManager.GetSceneByName(nextScene));
    //    obj.completed -= OperationCompleted;
    //}
    //void SetActiveForScene(Scene scene, bool active)
    //{
    //    foreach (GameObject gameObject in scene.GetRootGameObjects()) 
    //    {
    //        if(gameObject.tag == GameManagerScript.Instance.tagSO.rootTag) 
    //        {
    //            gameObject.SetActive(active);

    //            break;
    //        }
    //    }
    //}
}
