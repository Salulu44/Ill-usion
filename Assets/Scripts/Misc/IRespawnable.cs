using UnityEngine;

public interface IRespawnable 
{
    public Vector3 RespawnPoint { get; set; }
    public void SetRespawnPoint(Vector3 respawnPoint);
    public void Respawn();
}
