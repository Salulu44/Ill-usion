using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    [HideInInspector] public float[] position;
    [HideInInspector] public List<int> collectedCollectable;
    [HideInInspector] public int collectableAmount;
    public PlayerData(PlayerMovementScript player)
    {
        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
        collectedCollectable = new List<int>();
    }
    public PlayerData()
    {
        position = new float[3];
        collectedCollectable = new List<int>();
    }
    public Vector3 GetPosition()
    {
        return new Vector3(position[0], position[1], position[2]);
    }
    public void SetPosition(Transform player)
    {
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
    public void SetPosition(Vector3 position)
    {
        this.position[0] = position.x;
        this.position[1] = position.y;
        this.position[2] = position.z;
    }

    //public void AddCollectable(CollectableScript collectableScript)
    //{
    //    //if (collectedCollectable == null) Debug.Log("Ist null");
    //    Debug.Log("Amount" + collectableAmount + " " + this);
    //    if (collectedCollectable.Contains(collectableScript.collectableID)) return;
    //    collectedCollectable.Add(collectableScript.collectableID);
    //    Debug.Log("Amount" + collectableAmount + " " + this);
    //    collectableAmount++;
    //}
}
