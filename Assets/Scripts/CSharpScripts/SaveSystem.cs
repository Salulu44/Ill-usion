using System.IO;
using UnityEngine;

public class SaveSystem
{
    private const string playerDataJsonName = "playerData.json";

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SyncIDBFS_Internal();
#endif

    private static string GetSavePath(string fileName)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return "/idbfs/NotPrimitive/" + fileName;
#else
        return Path.Combine(Application.persistentDataPath, fileName);
#endif
    }

    private static void SyncIDBFS()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SyncIDBFS_Internal();
#endif
    }

    public static void SavePosition(Transform player)
    {
        PlayerData playerData = LoadSystem() ?? new PlayerData();
        playerData.SetPosition(player);
        string json = JsonUtility.ToJson(playerData);
        string path = GetSavePath(playerDataJsonName);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, json);
        SyncIDBFS();
    }

    public static void Save(PlayerData playerData)
    {
        string json = JsonUtility.ToJson(playerData);
        string path = GetSavePath(playerDataJsonName);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, json);
        SyncIDBFS();
    }

    //public static void SaveCollectables(CollectableScript collectableScript)
    //{
    //    PlayerData playerData = LoadSystem() ?? new PlayerDataScript();
    //    playerData.AddCollectable(collectableScript);
    //    string json = JsonUtility.ToJson(playerData);
    //    string path = GetSavePath(playerDataJsonName);
    //    Directory.CreateDirectory(Path.GetDirectoryName(path));
    //    File.WriteAllText(path, json);
    //    SyncIDBFS();
    //}

    public static PlayerData LoadSystem()
    {
        string path = GetSavePath(playerDataJsonName);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            if (!string.IsNullOrEmpty(json))
            {
                return JsonUtility.FromJson<PlayerData>(json);
            }
        }
        return null;
    }

    public static void ClearUpFile(string relativePath)
    {
        string path = GetSavePath(relativePath);
        if (File.Exists(path))
        {
            File.WriteAllText(path, string.Empty);
            SyncIDBFS();
        }
    }
}
