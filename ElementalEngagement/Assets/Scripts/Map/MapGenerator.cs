using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MapGenerator : MonoBehaviour
{

    public GameObject Prefab_TownCenter;
    public NavMeshSurface surface;

    // Start is called before the first frame update
    public void Start()
    {
        MapGrid grid = this.GetComponent<MapGrid>();
        grid.Init();
        int x = (int)Mathf.Round(Random.Range(3* grid.width/6, 4 * grid.width/6));
        int y = (int)Mathf.Round(Random.Range(3 * grid.height/6, 4 * grid.height/6));

        GameObject TownCenter = GameObject.Instantiate(Prefab_TownCenter,Vector3.zero, Quaternion.identity);
        TownCenter.transform.position = grid.placeStructureAt(TownCenter.GetComponent<Structure>(), x, y);

        // move this to a game controller class
        Camera.main.GetComponent<CameraController>().position = TownCenter.transform.position - new Vector3(15, 0, 15);
        //Build nav mesh
        surface.BuildNavMesh();

    }


}
