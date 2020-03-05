using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MoveablePiece : MonoBehaviour {
    // Start is called before the first frame update
    public float speed = 1.0f;
    private NavMeshAgent nav;

	
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
        MovePiece();

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
    }

    private void MovePiece()
    {
    }
}
