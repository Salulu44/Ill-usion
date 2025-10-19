using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapScript : MonoBehaviour
{
    private Tilemap tilemap; // Referenz zur Tilemap setzen
    public TileBase tile;
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        Vector3Int pos = new Vector3Int(1, -1, 0); // Zellkoordinaten
        TileBase myTile = tilemap.GetTile(pos);   // Tile an Position abrufen
        tilemap.SetTile(new Vector3Int(0, 0, 0), tile);
        tilemap.SetTile(new Vector3Int(-2, 0, 0), tile);
        if (myTile != null)
        {
            Debug.Log("Tile gefunden: " + myTile.name);
        }
        Vector3Int cellPosition = new Vector3Int(0, 0, 0); // Zell-Koordinate
        Vector3 worldPosition = tilemap.CellToWorld(cellPosition);
        Debug.Log("Weltposition des 0,0,0-Tiles: " + worldPosition);
    }
}
