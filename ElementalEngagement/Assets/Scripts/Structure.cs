using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : Entity
{
    public Vector2 dimensions;
    public string type;
    Vector2 gridPosition;

    public bool finished = false;

    public void setGridPosition(Vector2 pos){
        gridPosition = pos;
    }

    public void Create(){      

        if (Random.value < 0.5f) {
            gameObject.transform.Rotate(new Vector3(0, -90, 0));
        }
        name = type + GetInstanceID();

        healthCurrent = 10;

    }

    public virtual int Work(float w){
        healthCurrent += w;
        if (healthCurrent >= healthCap){
            healthCurrent = healthCap;
            if (!finished) OnConstructionFinish();
        }
        return 0;
    }

    void OnConstructionFinish(){
        finished = true;
        foreach (Transform child in gameObject.transform) {
            if (child.CompareTag("Default Model")) { child.gameObject.SetActive(true);} 
            if (child.CompareTag("Construction Model")) { child.gameObject.SetActive(false);} 
        }

        AbilityContainer a = gameObject.GetComponent<AbilityContainer>();
        if (a){
            a.enabled = true;
        }

    }




}
