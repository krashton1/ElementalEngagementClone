using UnityEngine;

// Unit Spawner
// The unit spawner will correctly position newly spawned units
// If a waypoint is set, newly spawned units will move to the waypoint


public class UnitSpawner : MonoBehaviour {
    public Vector3 offset;
    public Vector3 waypoint;
    bool waypointSet = false;

    public void SetWaypoint(Vector3 V)
    {
        waypoint = V;
    }

    public void onUnitSpawn(GameObject unit)
    {
        unit.transform.Translate(offset);
        if (waypointSet)
        {
            Unit move = unit.GetComponent<Unit>();
            if (move) move.SetFuturePosition(waypoint);
        }
    }
}
