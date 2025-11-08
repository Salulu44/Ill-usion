using Pathfinding;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private int[] zahlen = new int[sbyte.MaxValue];
    private sbyte index = sbyte.MaxValue -1;
    [field:SerializeField] public int property { get; private set; }
    [SerializeField] private DoodlePlatformScript script;
    private GridGraph graph;
    [SerializeField] GameObject movingObstacle;
    void Start()
    {
        graph = AstarPath.active.data.gridGraph;

    }

    // Update is called once per frame
    void Update()
    {
        graph.center = new Vector3(graph.center.x, Camera.main.transform.position.y, graph.center.z);
        if (Input.GetKeyDown(KeyCode.K)) 
        {
            index = (sbyte) ((index + 1) % zahlen.Length);
            Debug.Log(zahlen[index]);
            Instantiate(script, transform.position, Quaternion.identity);
            transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.P)) 
        {
            //graph.center = new Vector3(graph.center.x, Camera.main.transform.position.y, graph.center.z);
            //AstarPath.active.ScanAsync();
            AstarPath.active.AddWorkItem(() =>
            {
                Bounds bounds = movingObstacle.GetComponent<Collider2D>().bounds;
                var updateObj = new GraphUpdateObject(bounds);
                updateObj.updatePhysics = true; // Falls du Kollisionen neu berechnen möchtest
                AstarPath.active.UpdateGraphs(updateObj);
            });
            //Bounds worldBounds = movingObstacle.GetComponent<Collider2D>().bounds; // Welt-Bounds vom Collider

            //// Umrechnung Welt->Grid-Koordinaten
            //var graphTransform = graph.transform;
            //Vector3 min = graphTransform.InverseTransform(worldBounds.min);
            //Vector3 max = graphTransform.InverseTransform(worldBounds.max);

            //// Begrenzen auf Gridgröße
            //min.x = Mathf.Max(min.x, 0);
            //min.z = Mathf.Max(min.z, 0);
            //max.x = Mathf.Min(max.x, graph.width);
            //max.z = Mathf.Min(max.z, graph.depth);

            //var updateBounds = new Bounds();
            //updateBounds.SetMinMax(min, max);

            //// Teilupdate im Grid ausführen
            //AstarPath.active.UpdateGraphs(updateBounds);
            //AstarPath.active.ScanAsync();
        }
    }
}
