using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class MapGenerator : MonoBehaviour
{

    public GameObject Prefab_TownCenter;
    public GameObject Prefab_Tree;
    public GameObject Prefab_Gold;
    public NavMeshSurface surface;
    public float noiseScale = 0.3f;

    public float startingAreaRadius = 15;

    float[,] noiseMap;

    // Start is called before the first frame update
    public void Start()
    {
        MapGrid grid = this.GetComponent<MapGrid>();
        grid.Init();

        noiseMap = Noise.GenerateNoiseMap(grid.width, grid.height, noiseScale);

        int x = (int)Mathf.Round(Random.Range(3* grid.width/6, 4 * grid.width/6));
        int y = (int)Mathf.Round(Random.Range(3 * grid.height/6, 4 * grid.height/6));

        GameObject TownCenter = GameObject.Instantiate(Prefab_TownCenter,Vector3.zero, Quaternion.identity);
        TownCenter.transform.position = grid.placeStructureAt(TownCenter.GetComponent<Structure>(), x, y);

        // move this to a game controller class
        Camera.main.GetComponent<CameraController>().position = TownCenter.transform.position - new Vector3(15, 0, 15);



        for (int i = 0; i < grid.width; i ++){
            for (int j = 0; j < grid.height; j++)
            {
                if (Vector2.Distance(new Vector2(i, j), new Vector2(x, y)) < startingAreaRadius) continue;
                if (noiseMap[i,j] < 0.25f){
                    GameObject tree = GameObject.Instantiate(Prefab_Tree,Vector3.zero, Quaternion.Euler(0,Random.Range(0, 359),0));
                    tree.transform.position = grid.placeStructureAt(tree.GetComponent<Structure>(), i, j);
                    tree.name = "Tree" + i + j;
                    float s = 1.25f - noiseMap[i,j] * 3;
                    tree.transform.localScale = new Vector3(s,s,s);
                    tree.transform.SetParent(transform);

                }
            }
        }


        //Build nav mesh
        surface.BuildNavMesh();

    }


}
