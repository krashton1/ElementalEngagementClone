using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Structure : Entity
{

    public enum WorkReturnCode {None, Finished, GiveOre, Exhausted};

    public Vector2 dimensions;
    public string type;
    Vector2 gridPosition;

    public bool finished = false;
    public ResourceComponent resource = null;

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

    public virtual WorkReturnCode Work(float w){
        if (resource){
            return resource.gather(w);
        }
        healthCurrent += w;
        if (healthCurrent >= healthCap){
            healthCurrent = healthCap;
            if (!finished) OnConstructionFinish();
            return WorkReturnCode.Finished;
        }
        return WorkReturnCode.None;
    }

    void OnConstructionFinish(){
        finished = true;
        foreach (Transform child in gameObject.transform) {
            if (child.CompareTag("Default Model")) { child.gameObject.SetActive(true);} 
            if (child.CompareTag("Construction Model")) { child.gameObject.SetActive(false);} 
        }

        // Activate components
        // To Do: Abstract components are children of a master class, and make this a loop
        Wall w = gameObject.GetComponent<Wall>();
        if (w){
            w.setRotationAroundTile((int)gridPosition.x, (int)gridPosition.y);
        }

        AbilityContainer a = gameObject.GetComponent<AbilityContainer>();
        if (a){
            a.enabled = true;
        }

        HousingComponent h = gameObject.GetComponent<HousingComponent>();
        if (h) {
            h.OnConstruction();
        }

        ManaPylon m = gameObject.GetComponent<ManaPylon>();
        if (m) {
            m.OnConstruction();
        }

    }

    private void OnDestroy() {
        if (GameObject.Find("GroundPlane")){
            GameObject.Find("GroundPlane").GetComponent<MapGrid>().clearTile((int)gridPosition.x, (int)gridPosition.y);
        }
    }




}
