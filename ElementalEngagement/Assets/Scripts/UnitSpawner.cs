using UnityEngine;

public class UnitSpawner : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject entity;
    public Vector3 waypoint;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SpawnEntity()
    {
        MoveablePiece MP = Instantiate(entity, transform.position + new Vector3(10f, 0f, 0f), new Quaternion()).GetComponent<MoveablePiece>();
    }

    public void SetWaypoint(Vector3 V)
    {
        waypoint = V;
    }
}
