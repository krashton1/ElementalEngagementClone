using UnityEngine;
using UnityEngine.AI;

public class MoveablePiece : Unit {
    // Start is called before the first frame update
    public float speed = 1.0f;
    public float attack_range;
    public float stop_range;
    private NavMeshAgent nav;
    public GameObject target;
    public GameObject projectile;
    public int attack_frame_counter = 30;
    public int current_attack_frame;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        current_attack_frame = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            TargetEnemy();
            AttackEnemy();
        }
    }

    public void SetFuturePosition(Vector3 V)
    {
        nav.destination = V;
        target = null;
    }

    public void SetTarget(GameObject GO)
    {
        target = GO;
    }

    private void TargetEnemy()
    {
        float dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist > stop_range)
        {
            nav.destination = target.transform.position;
        }
        else
        {
            nav.destination = transform.position;
        }
    }

    private void AttackEnemy()
    {
        if (current_attack_frame++ > attack_frame_counter)
        {
            Vector3 dist = target.transform.position - transform.position;
            if (dist.magnitude < attack_range)
            {
                Projectile MP = Instantiate(projectile, transform.position + (dist.normalized * 5 * GetComponent<CapsuleCollider>().radius), new Quaternion()).GetComponent<Projectile>();
                MP.SetDirection(dist);
                MP.SetDamage(25);
                MP.SetGO(gameObject);
            }
            current_attack_frame = 0;
        }
    }
}
