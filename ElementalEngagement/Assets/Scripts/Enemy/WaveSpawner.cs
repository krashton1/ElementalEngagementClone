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
    public int Spawn_count = 1;

    private float currTimer = 0;

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
        float start_y = start.y;
        Vector3 spawn_point = Spawn_radius * new Vector3(Mathf.Cos(Time.realtimeSinceStartup), 1, Mathf.Sin(Time.realtimeSinceStartup)) + start;
        spawn_point.y = start_y;

        SpawnEnemies(spawn_point);

        Spawn_interval = Mathf.Max(30.0f, Spawn_interval * 0.99f);
		Spawn_interval = 10;
        Spawn_count += 1;
    }

    private void SpawnEnemies(Vector3 spawn_point)
    {

		System.Random rnd = new System.Random();

		for (int i = 0; i < 1; i++)
        {
            GameObject newEnemy = Instantiate(EnemyGO, spawn_point, Quaternion.identity);
            BasicEnemy BE = newEnemy.GetComponent<BasicEnemy>();
			Entity E = newEnemy.GetComponent<Entity>();

			int r = rnd.Next(3, 4);
			switch (3)
			{
				case 0:
					E.element_type = ElementComponent.ElementType.None;
					break;
				case 1:
					E.element_type = ElementComponent.ElementType.Fire;
					break;
				case 2:
					E.element_type = ElementComponent.ElementType.Water;
					break;
				case 3:
					E.element_type = ElementComponent.ElementType.Grass;
					break;
				default:
					E.element_type = ElementComponent.ElementType.None;
					break;
			}

			BE.SetTarget(HomeBase.gameObject);
        }
    }
}
