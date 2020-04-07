using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Work Component
// A unit with the work component can "work" on structures and carry ore
// The specific behaviour is defined in the structure "work" function
// Generally, this allows for building and reparing buildings, and mining ore

public class WorkComponent : MonoBehaviour
{

    public float workingRange = 0.5f;
    public Structure target;
    public float workSpeed = 25;

    public int resourceCap = 10;
    int resourceCount = 0;

	private Animator anim;

    void Start()
    {
		anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (target){
            if(Vector2.Distance(transform.position, target.transform.position) < target.dimensions.magnitude + workingRange + 0.5f){
                resourceCount += target.Work(workSpeed * Time.deltaTime);
            }
        }
    }

    public float SetTarget(Structure structure)
    {
        target = structure;
        return structure.dimensions.magnitude + workingRange;
    }
}
