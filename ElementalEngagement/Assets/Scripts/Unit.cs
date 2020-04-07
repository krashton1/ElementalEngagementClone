using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class Unit : Entity {
    // Start is called before the first frame update
    public float speed = 1.0f;
    private NavMeshAgent nav;
    public float stopRange = 1.0f;
    public GameObject targetObject;

    float mElapsedTime = 0.0f;

	private Animator anim;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        healthCurrent = healthCap;
		anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (targetObject){
            MoveToTarget();
        }

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
            if (selected)
            {
                hpBarUi.gameObject.SetActive(true);
                hpBarUi.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 2, 0));
                hpBarUi.value = ((float)(healthCurrent) / (float)(healthCap));
                //Debug.Log(hpBarUi.value);
            }
            else
            {
                hpBarUi.gameObject.SetActive(false);
            }

        }
    }

    // These target handlers are inherited from entity
    override public void targetEntity(GameObject target){
        Debug.Log("Test");
        AttackComponent a = gameObject.GetComponent<AttackComponent>();
        if (a) {
            stopRange = a.SetTarget(target);
            targetObject = target;

        }
    
        WorkComponent w = gameObject.GetComponent<WorkComponent>();
        if (w && target.GetComponent<Structure>()) {
            stopRange = w.SetTarget(target.GetComponent<Structure>());
            targetObject = target;

        }

    }

    override public void targetPosition(Vector3 point){
        SetFuturePosition(point);
    }


    public void SetFuturePosition(Vector3 V)
    {
        nav.destination = V;
		targetObject = null;
		
    }

    public void SetTarget(GameObject GO)
    {
        targetObject = GO;
    }

    private void MoveToTarget()
    {
        float dist = Vector3.Distance(transform.position, targetObject.transform.position);
        if (dist > stopRange)
        {
            nav.destination = targetObject.transform.position;
        }
        else
        {
            nav.destination = transform.position;
        }
    }

}



