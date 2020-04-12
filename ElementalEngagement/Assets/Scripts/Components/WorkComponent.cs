using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Work Component
// A unit with the work component can "work" on structures and carry ore
// The specific behaviour is defined in the structure's "work" function
// Generally, this allows for construction, repair, and mining 

// Worker AI
// When a completes a task, it will look within a radius for more jobs to do

public class WorkComponent : MonoBehaviour
{

    // General Stats
    public float workingRange = 0.5f;
    public Structure target;
    public float workSpeed = 25;

    // Mining
    public int resourceCap = 10;
    public int resourceCount = 0;
    Structure currentOreDeposit = null;

	private Animator anim;

    // AI
    public enum States {Waiting, Working, Sensing, Carrying, Moving};
    public States state = States.Waiting;
    public enum Jobs {Miner, Builder};
    public Jobs job = Jobs.Miner;
    public float senseRange = 15.0f;

    public Unit unit;
    public PlayerBehaviour resourceController;

    void Start()
    {
		anim = GetComponentInChildren<Animator>();
        resourceController = GameObject.Find("PlayerController").GetComponent<PlayerBehaviour>();
    }

    void Update()
    {
        // Normally, the target should never be null
        if (!target){
            state = States.Waiting;
        }
        if (state == States.Moving || state == States.Working || state == States.Carrying){
            // If we are close to the target
            if(Vector2.Distance(transform.position, target.transform.position) < target.dimensions.magnitude + workingRange + 0.5f){
                // Dropoff Ore
                if (state == States.Carrying){
                    resourceController.addGold(resourceCount);
                    resourceCount = 0;
                    if (currentOreDeposit){
                        unit.targetEntity(currentOreDeposit.gameObject);
                        state = States.Moving;
                    }
                }
                // Do work
                else{
                    state = States.Working;
                    anim.SetTrigger("Attack1Trigger");
                    Structure.WorkReturnCode r = target.Work(workSpeed * Time.deltaTime);
                    if( r == Structure.WorkReturnCode.Finished){
                        OnWorkFinish();
                    }
                    else if (r == Structure.WorkReturnCode.GiveOre) {
                        OnGetResource();
                    }
                    else if (r == Structure.WorkReturnCode.Exhausted){
                        OnGetResource();
                        OnDepositExhausted();
                    }
                }               
            }
            else{
                state = States.Moving;
                anim.ResetTrigger("Attack1Trigger");
            }
        }
    }

    public float SetTarget(Structure structure)
    {
        target = structure;
        // If target is ore deposit or dropoff point, become a miner
        if (structure.gameObject.GetComponent<ResourceComponent>()){
            if (structure.gameObject.GetComponent<ResourceComponent>().isDropoffPoint()) state = States.Carrying;
            else {
                state = States.Moving;
                currentOreDeposit = structure;
            }
            job = Jobs.Miner;
        }
        // If target is a construction site, become a builder
        else{
            job = Jobs.Builder;
            state = States.Moving;
            currentOreDeposit = null;
        }
        return structure.dimensions.magnitude + workingRange;
    }

    public void unsetTarget(){
        target = null;
        currentOreDeposit = null;
        state = States.Waiting;
    }

    void OnGetResource(){
        resourceCount += 1;
        if (resourceCount >= resourceCap){
            OnWorkFinish();
        }
    }

    // When a building is finished or our inventory is full, look for a new target
    void OnWorkFinish(){
        anim.ResetTrigger("Attack1Trigger");
        target = null;
        state = States.Sensing;


        // Find the nearest structure
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, senseRange, LayerMask.GetMask("Choosable"));
        
        Structure nearest = null;
        float distanceToNearest = 1000;

        for (int i = 0; i < hitColliders.Length; i++)
        {
            Structure s = hitColliders[i].gameObject.GetComponent<Structure>();
            if (!s) continue;

            float dist = Vector3.Distance(transform.position, s.transform.position);
            if (dist >= distanceToNearest) continue;

            // Builders look for unfinished buildings
            if (job == Jobs.Builder){
                if (s.finished == false){
                    nearest = s;
                    distanceToNearest = dist;
                }
            }

            // Miners look for dropoff points
            else if (job == Jobs.Miner){
                if (s.gameObject.GetComponent<ResourceComponent>()){
                    if (s.gameObject.GetComponent<ResourceComponent>().isDropoffPoint()){
                        nearest = s;
                        distanceToNearest = dist;
                    }
                }
            }
        }

        if (nearest){
            if (job == Jobs.Builder) state = States.Moving;
            else state = States.Carrying;
            unit.targetEntity(nearest.gameObject);
        }
        else {
            state = States.Waiting;
        }

    }

    // When an ore deposit is exhausted, look for another nearby
    void OnDepositExhausted(){
        anim.ResetTrigger("Attack1Trigger");
        target = null;
        state = States.Sensing;

		float dist = 10000;

		// Find a nearby ore deposit
		Collider[] hitColliders = Physics.OverlapSphere(currentOreDeposit.transform.position, senseRange, LayerMask.GetMask("Choosable"));
        Structure newOreDeposit = null;
        for (int i = 0; i < hitColliders.Length; i++)
        {
            ResourceComponent s = hitColliders[i].gameObject.GetComponent<ResourceComponent>();
            if (!s) continue;
            if (s.isDropoffPoint()) continue;
			if(Vector3.Distance(transform.position, s.transform.position) < dist)
			{
				if(hitColliders[i].gameObject.GetComponent<Structure>() != currentOreDeposit)
				{

					newOreDeposit = hitColliders[i].gameObject.GetComponent<Structure>();
					dist = Vector3.Distance(transform.position, s.transform.position);
				}
			}


        }

        if (newOreDeposit){
            state = States.Moving;
			unit.targetEntity(newOreDeposit.gameObject);
		}
        else {
            state = States.Waiting;
        }

    }
}
