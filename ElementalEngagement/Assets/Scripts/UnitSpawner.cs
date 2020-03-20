using UnityEngine;

public class UnitSpawner : MonoBehaviour {
    
    public Vector3 offset;
    public Vector3 waypoint;
    bool waypointSet = false;

    public void SetWaypoint(Vector3 V)
    {
        waypoint = V;
    }

    public void onUnitSpawn(GameObject unit){
        unit.transform.Translate(offset);
        if (waypointSet){
            MoveablePiece move = unit.GetComponent<MoveablePiece>();
            if (move) move.SetFuturePosition(waypoint);
        }
    }
}
