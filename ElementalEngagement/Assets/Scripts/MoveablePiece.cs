using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MoveablePiece : Unit {
    // Start is called before the first frame update
    public float speed = 1.0f;
    public float attack_range;
    private NavMeshAgent nav;
    public GameObject target;
    private

	
	private int mHealth = 30;
	private int mHealthCap = 100;


	private float mElapsedTime = 0.0f;

	void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
            TargetEnemy();

		mElapsedTime += Time.deltaTime;
		if (mElapsedTime >= 1.0f)
		{
			mElapsedTime = mElapsedTime % 1.0f;

			if (mHealth < mHealthCap)
			{
				mHealth++;
			}
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
