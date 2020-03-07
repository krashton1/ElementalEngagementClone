using UnityEngine;

public class UnitSpawner : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject entity;
    public Vector3 waypoint;

	public PlayerBehaviour mPlayer;

    void Start()
    {
        mPlayer = GameObject.Find("PlayerController").GetComponent<PlayerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnEntity()
    {
		if(mPlayer.getPopulation() < mPlayer.getPopulationCap())
		{
			if(mPlayer.getGold() >= 5)
			{
				mPlayer.setGold(mPlayer.getGold() - 5);
				mPlayer.setPopulation(mPlayer.getPopulation() + 1);
				MoveablePiece MP = Instantiate(entity, transform.position + waypoint + new Vector3(-Random.value, 0, -Random.value), new Quaternion()).GetComponent<MoveablePiece>();
            }
		}
    }

    public void SetWaypoint(Vector3 V)
    {
        waypoint = V;
    }
}
