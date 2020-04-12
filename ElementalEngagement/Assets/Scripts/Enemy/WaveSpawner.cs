using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {
    // Start is called before the first frame update
    [Header("Spawning Entity")]
    public GameObject EnemyGO;
    public GameObject HomeBase;

    [Header("Spawning Params")]
    public float Spawn_radius = 1.0f;
    public float Spawn_interval = 35.0f;
    public float Min_Spawn_interval = 30.0f;
    public int Spawn_count = 2;

    private float currTimer = 0;

    public MapGrid grid;

    void Start()
    {
        HomeBase = GameObject.FindGameObjectWithTag("Home Base");
    }

    // Update is called once per frame
    void Update()
    {
        if (HomeBase == null)
        {
            HomeBase = GameObject.FindGameObjectWithTag("Home Base");
        }

        currTimer += Time.deltaTime;
        if (currTimer > Spawn_interval && HomeBase != null)
        {
            currTimer = 0;
            SpawnWave();
        }
    }


    public void SpawnWave()
    {
        Vector3 start = HomeBase.transform.position;
        float start_y = 0.5f;
        bool valid = false;
        Vector3 spawn_point = Vector3.zero;
        while(!valid){
            spawn_point = new Vector3(150, 0, 150) + Spawn_radius * new Vector3(Mathf.Cos(Time.realtimeSinceStartup), 1, Mathf.Sin(Time.realtimeSinceStartup));
            spawn_point.y = start_y;
            valid = grid.getTileAt(spawn_point.x, spawn_point.y).isEmpty();
        }

        SpawnEnemies(spawn_point);

        Spawn_interval = Mathf.Max(Min_Spawn_interval, Spawn_interval * 0.90f);
        Spawn_count += 1;
    }

    private void SpawnEnemies(Vector3 spawn_point)
    {
        float r = Spawn_count;
        List<Vector2> points = PoissonDiscSampling.GeneratePoints(0.25f, new Vector2(r, r));
        for (int i = 0; i < Spawn_count; i++)
        {
            Vector3 spawn = spawn_point + new Vector3 (points[i].x, 0, points[i].y);
            GameObject newEnemy = Instantiate(EnemyGO, spawn, Quaternion.identity);
            BasicEnemy BE = newEnemy.GetComponent<BasicEnemy>();
            BE.SetTarget(HomeBase.gameObject);
        }
    }
}
