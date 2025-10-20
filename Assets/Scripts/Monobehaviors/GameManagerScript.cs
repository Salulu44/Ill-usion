using System.Reflection;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance { get; private set; }
    public TagSO tagSO;
    [HideInInspector] public event Action OnGameStart;
    public event Action<float> OnTimerOff;
    [HideInInspector] public event Func<int> func;
    public GameObject player;
    #region Prefabs
    public GameObject coinPrefab;
    private bool isHUDOn;
    #endregion
    private int smartPathAmount;

    void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //SceneManager.sceneLoaded += OnSceneLoaded;
        }

    }

    private void Start()
    {

        OnGameStart?.Invoke();
        DontDestroyOnLoad(gameObject);
    }
    //private void CollectableCheck()
    //{
    //    PlayerDataScript playerData = SaveSystemScriptNew.LoadSystem();
    //    GameObject[] collectables = GameObject.FindGameObjectsWithTag(TagSO.CollectableTag);

    //    foreach (GameObject collectable in collectables)
    //    {
    //        print(collectable);
    //        if (playerData.collectedCollectable.Contains(collectable.GetComponent<CollectableScript>().GetID()))
    //        {
    //            print("You are already collected");
    //            collectable.SetActive(false);
    //        }
    //    }
    //}
    //void SetPlayerPosition()
    //{
    //    GameObject player = GameObject.FindWithTag(TagSO.PlayerTag);
    //}
    //void Update()
    //{

    //    //if (Input.GetKeyDown(KeyCode.Escape) && player != null)
    //    //{
    //    //    isHUDOn = !isHUDOn;
    //    //    player.GetComponent<PlayerMovementScript>().enabled = !isHUDOn;
    //    //    player.GetComponent<GrappleScript>().enabled = !isHUDOn;
    //    //    CollectableManagerScript.Instance.SetHUD(isHUDOn);
    //    //}
    //}
    //public void IncreaseSmartPaths(GameObject easyPathObject)
    //{
    //    if (easyPathObject.tag == TagSO.EasyPathTag) smartPathAmount++;
    //}
    //public int GetEasyPathAmount() => smartPathAmount;
    ////private void OnDestroy()
    ////{
    ////    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}
    ////private void OnApplicationQuit()
    ////{
    ////    player.GetComponent<PlayerRespawnScript>().playerSO.SetToDefault();
    ////}
    ////void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    ////{
    ////    CollectableCheck();
    ////    player = GameObject.FindWithTag("Player");
    ////    if (scene.name == "CaveScene")
    ////    {
    ////        AudioClip audioClip = AudioManagerScript.Instance.GetMusicAudio().clip;
    ////        if (audioClip != AudioManagerScript.Instance.caveTheme)
    ////        {

    ////            AudioManagerScript.Instance.PlayCaveMusic();
    ////        }

    ////    }
    ////    if (player != null)
    ////    {

    ////        player.GetComponent<PlayerRespawnScript>().SetSpawnPosition();
    ////    }
    ////    else Debug.LogWarning("Player nicht gefunden nach Szenenwechsel!");

    ////}

    //public float GetSineWave(float sinAmplitude)
    //{
    //    return Mathf.Sin(Time.time) * sinAmplitude;
    //}
    //public float GetPingPong(float pingPongValue, float speed)
    //{
    //    return Mathf.PingPong(Time.time * speed, pingPongValue);

    //}

    //public TComponent CopyComponent<TComponent>(GameObject destination, TComponent originalComponent) where TComponent : Component
    //{
    //    Type type = originalComponent.GetType();

    //    Component copyComponent = destination.AddComponent(type);

    //    FieldInfo[] fieldInfos = type.GetFields();

    //    foreach (FieldInfo fieldInfo in fieldInfos)
    //    {
    //        fieldInfo.SetValue(copyComponent, originalComponent);
    //    }
    //    return copyComponent as TComponent;
    //}
}
