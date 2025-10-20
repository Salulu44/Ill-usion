using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToggle : MonoBehaviour
{
    public string sceneName;
    private bool secondSceneLoaded;
    private Scene mainScene;

    void Start()
    {
        mainScene = SceneManager.GetActiveScene();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!secondSceneLoaded)
            {
                // Szene B laden (additiv)
                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                secondSceneLoaded = true;

                // Szene A "pausieren" -> alle GameObjects deaktivieren
                SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(sceneName));
                SetActiveForScene(mainScene, false);
            }
            else
            {
                // Szene B wieder entladen
                SceneManager.MoveGameObjectToScene(gameObject, mainScene);
                SceneManager.UnloadSceneAsync(sceneName);
                secondSceneLoaded = false;

                // Szene A wieder aktivieren
                SetActiveForScene(mainScene, true);
               
            }
        }
    }
    //public static void LoadMinigame(string sceneName) 
    //{
       
    //    // Szene B laden (additiv)
    //    SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
    //    // Szene A "pausieren" -> alle GameObjects deaktivieren
    //    SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(sceneName));
    //    SetActiveForScene(mainScene, false);
        
    //    // Szene B wieder entladen
    //    SceneManager.MoveGameObjectToScene(gameObject, mainScene);
    //    SceneManager.UnloadSceneAsync(sceneName);
    //    secondSceneLoaded = false;

    //    // Szene A wieder aktivieren
    //    SetActiveForScene(mainScene, true);

    //}
    void SetActiveForScene(Scene scene, bool active)
    {
        foreach (GameObject go in scene.GetRootGameObjects())
        {
            go.SetActive(active);
        }
    }
}
