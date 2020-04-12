using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;



public class BasicEnemy : Entity {
    // Start is called before the first frame update
    public float speed = 1.0f;
    private NavMeshAgent nav;
    public float stopRange = 1.0f;
    public GameObject targetObject;
    public GameObject attackingObject;
    public int attack_damage = 10;
    public float senseRadius = 1.0f;
    public float attackRadius = 1.0f;

    float mElapsedTime = 0.0f;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        healthCurrent = healthCap;


	}

    // Update is called once per frame
    void Update()
    {

        // Time since last update, this is only here so that health can be modified over time
        mElapsedTime += Time.deltaTime;
        if (mElapsedTime >= 0.5f)
        {
            mElapsedTime = mElapsedTime % 0.5f;

            if (healthCurrent < healthCap)
            {
                healthCurrent++;
            }



			if (attackingObject)
			{
				MoveToTarget(attackingObject);
				AttackTarget();
			}
			else if (targetObject)
			{
				MoveToTarget(targetObject);
				SenseTarget();
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
    override public void targetEntity(GameObject target)
    {
        AttackComponent a = gameObject.GetComponent<AttackComponent>();
        if (a)
        {
            stopRange = a.SetTarget(target);
            targetObject = target;

        }

        WorkComponent w = gameObject.GetComponent<WorkComponent>();
        if (w && target.GetComponent<Structure>())
        {
            stopRange = w.SetTarget(target.GetComponent<Structure>());
            targetObject = target;

        }

    }

    override public void targetPosition(Vector3 point)
    {
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

    private void MoveToTarget(GameObject target)
    {
        float dist = Vector3.Distance(transform.position, target.transform.position);
        if (dist > stopRange)
        {
            nav.destination = target.transform.position;
        }
        else
        {
            nav.destination = transform.position;
        }
    }

    private void SenseTarget()
    {
        GameObject[] allEntities = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject GO in allEntities)
        {
            if (Vector3.Distance(transform.position, GO.transform.position) < senseRadius)
            {
                attackingObject = GO;
                return;
            }
        }
    }

    private void AttackTarget()
    {
        if (Vector3.Distance(transform.position, attackingObject.transform.position) < senseRadius)
        {
            attackingObject.GetComponent<Entity>().Damage(attack_damage, element_type);
        }
    }
}



