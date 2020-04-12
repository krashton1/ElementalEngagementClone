using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceComponent : MonoBehaviour
{
    public int resourceCount = 200;
    public float timeToGather = 100; // base worker speed is 25
    float gatherTimer = 0;

    public bool gatherPoint = false;
    public bool dropoffPoint = false;


    public Structure.WorkReturnCode gather(float work){
        gatherTimer += work;
        if (gatherTimer >= timeToGather){
            gatherTimer = 0;
            resourceCount--;
			
			if (resourceCount <= 0)
			{
				Entity E = gameObject.GetComponent<Entity>();
				E.Damage(E.healthCap);
				return Structure.WorkReturnCode.Exhausted;
			} 
            else return Structure.WorkReturnCode.GiveOre;
        }
        return Structure.WorkReturnCode.None;
    }

    public bool isDropoffPoint(){
        return dropoffPoint;
    }
 
    public bool isExhausted(){
        return resourceCount == 0;
    }
    
}
