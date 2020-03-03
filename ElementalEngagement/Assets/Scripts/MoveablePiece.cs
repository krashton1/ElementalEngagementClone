using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveablePiece : MonoBehaviour {
    // Start is called before the first frame update
    public float speed = 1.0f;
    public float attack_range;
    private NavMeshAgent nav;
    public GameObject target;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
            TargetEnemy();
    }

    public void SetFuturePosition(Vector3 V)
    {
        nav.destination = V;
    }

    public void SetTarget(GameObject GO)
    {
        target = GO;
    }

    private void TargetEnemy()
    {
        float dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist > attack_range)
        {
            nav.destination = target.transform.position;
        }
        else
        {
            nav.destination = transform.position;
        }

    }
}
