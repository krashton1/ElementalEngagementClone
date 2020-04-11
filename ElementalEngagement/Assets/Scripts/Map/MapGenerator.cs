﻿using System.Collections;
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
    public float noiseCutoff = 0.25f;
    public float startingAreaRadius = 15;

    public int Ore_Amount = 25;

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


        // Generate Trees
        for (int i = 0; i < grid.width; i ++){
            for (int j = 0; j < grid.height; j++)
            {
                if (Vector2.Distance(new Vector2(i, j), new Vector2(x, y)) < startingAreaRadius) continue;
                if (noiseMap[i,j] < noiseCutoff){
                    GameObject tree = GameObject.Instantiate(Prefab_Tree,Vector3.zero, Quaternion.Euler(0,Random.Range(0, 359),0));
                    tree.transform.position = grid.placeStructureAt(tree.GetComponent<Structure>(), i, j);
                    tree.name = "Tree" + i + j;
                    float s = 1.25f - noiseMap[i,j] * 2;
                    tree.transform.localScale = new Vector3(s,s,s);
                    tree.transform.SetParent(transform);

                }
            }
        }

        // Generate Ore
        List<Vector2> orePositions = PoissonDiscSampling.GeneratePoints(25, new Vector2(grid.width, grid.height));

        for (int i = 0; i < orePositions.Count; i++){
            if (i >= Ore_Amount) break;
            if (Vector2.Distance(orePositions[i], new Vector2(x, y)) < startingAreaRadius) continue;
            if (grid.getTileAt(orePositions[i].x, orePositions[i].y).isEmpty()){
                GenerateOrePatch(grid, (int)orePositions[i].x, (int)orePositions[i].y);
            }
        }

        // Generete 3 ore patches in starting area
        for (int i = 0; i < 3;){

            //randomly select points around the town center
            int a = x + (int) ((Random.Range(0,2)*2-1) * Random.Range(6, startingAreaRadius));
            int b = y + (int) ((Random.Range(0,2)*2-1) * Random.Range(6, startingAreaRadius));

            if (grid.getTileAt(a, b).isEmpty()){
                GenerateOrePatch(grid, a, b);
                i++;
            }
        }


        //Build nav mesh
        surface.BuildNavMesh();

    }


    // Generate an ore patch of varying size
    void GenerateOrePatch(MapGrid grid, int x_, int y_){

        int patchSize = Random.Range(3, 6);
        int j = 0; // sloppy protection against infinite loops
        for (int i = 0; i < patchSize;)
        {
            int x, y;
            x = x_ + Random.Range(-1,1);
            y = y_ + Random.Range(-1,1);
            x = Mathf.Clamp(x, 0, 150);
            y = Mathf.Clamp(y, 0, 150);
            if (grid.getTileAt(x, y).isEmpty()){
                    GameObject ore = GameObject.Instantiate(Prefab_Gold,Vector3.zero, Quaternion.Euler(0,Random.Range(0, 359),0));
                    ore.transform.position = grid.placeStructureAt(ore.GetComponent<Structure>(), x, y);
                    ore.name = "OreDeposit" + x + y + "_" + i;
                    ore.transform.SetParent(transform);
                    i++;
                }
            j++;
            if (j >= 10) break;
        }

        

        

    }

}
