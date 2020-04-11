using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{

    public enum TypeOfTarget {Entity, GroundPosition};


    // If this piece is currently selected
    protected bool selected = false;


    public float healthCap = 100;
    public float healthCurrent;
    public Slider hpBarUi;
    public string entityName;
    public bool MarkedForDeletion = false;

    public void handleReceiveTarget(RaycastHit hit, TypeOfTarget type){
        if (type == TypeOfTarget.Entity){
            targetEntity(hit.transform.gameObject);
        }
        else if (type == TypeOfTarget.GroundPosition){
            targetPosition(hit.point);
        }
    }

    virtual public void targetEntity(GameObject target){

    } 

    virtual public void targetPosition(Vector3 point){

    }
    

    private void LateUpdate() {
        if (MarkedForDeletion){
            Destroy(gameObject);
        }
    }

    public bool getSelected()
    {
        return selected;
    }

    public void setSelected(bool b)
    {
        cakeslice.Outline outline = GetComponent<cakeslice.Outline>();
        if(!outline) outline = GetComponentInChildren<cakeslice.Outline>();
        outline.eraseRenderer = !b;
        selected = b;
    }

        public void Damage(int damage_amount)
    {
        healthCurrent -= damage_amount;
        if (healthCurrent < 0)
        {
            Destroy(gameObject);
        }

    }

    public void Delete(){
        MarkedForDeletion = true;
    }
}
