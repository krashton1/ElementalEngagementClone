using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveablePiece : MonoBehaviour {
    // Start is called before the first frame update
    public float speed = 1.0f;
    private NavMeshAgent nav;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePiece();
    }

    public void SetFuturePosition(Vector3 V)
    {
        nav.destination = V;
    }

    private void MovePiece()
    {
    }
}
