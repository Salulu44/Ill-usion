using Pathfinding;
using UnityEngine;
using UnityEngine.Scripting;

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
            AstarPath.active.AddWorkItem(new Pathfinding.AstarWorkItem(ctx =>
            {
                // Get the moving obstacle's collider bounds in world space
                Bounds bounds = movingObstacle.GetComponent<Collider2D>().bounds;

                // Create a GraphUpdateObject with those bounds
                var guo = new GraphUpdateObject(bounds);
                guo.updatePhysics = true; // Recalculate physics/collisions

                // Update the graphs within those bounds safely
                AstarPath.active.UpdateGraphs(guo);

                // Ensure connectivity info and flood fill are updated after graph updates
                ctx.EnsureValidFloodFill();

            }));
        }

    }
}
