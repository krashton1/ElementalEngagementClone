using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceComponent : MonoBehaviour
{
    public int resourceCount;
    public float timeToGather = 5;
    float gatherTimer = 0;


    public int gather(float work){
        gatherTimer += work;
        if (gatherTimer >= timeToGather){
            gatherTimer = 0;
            resourceCount--;
            return 1;
        }
        return 0;
    }
}
