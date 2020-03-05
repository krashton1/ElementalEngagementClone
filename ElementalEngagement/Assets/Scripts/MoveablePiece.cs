using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class MoveablePiece : Unit {
    // Start is called before the first frame update
    public float speed = 1.0f;
    public float attack_range;
    private NavMeshAgent nav;
    public GameObject target;




	private float mElapsedTime = 0.0f;


	void Start()
    {
        nav = GetComponent<NavMeshAgent>();

		healthCurrent = 0;
	}

    // Update is called once per frame
    void Update()
    {
        if (target != null)
            TargetEnemy();

		// Time since last update, this is only here so that health can be modified over time
		mElapsedTime += Time.deltaTime;
		if (mElapsedTime >= 0.1f)
		{
			mElapsedTime = mElapsedTime % 0.1f;

			if (healthCurrent < healthCap)
			{
				healthCurrent++;
			}
		}

		// If a health bar has been attached, enable it if we have selected this piece, make it follow the piece, and update its value to the piece's current health.
		if (hpBarUi != null)
		{
			if(selected)
			{
				hpBarUi.gameObject.SetActive(true);
				hpBarUi.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 8, 0));
				hpBarUi.value = ((float)(healthCurrent) / (float)(healthCap));
				//Debug.Log(hpBarUi.value);
			}
			else
			{
				hpBarUi.gameObject.SetActive(false);
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
