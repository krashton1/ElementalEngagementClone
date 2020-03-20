using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    public enum TypeOfTarget {Entity, GroundPosition};


    public Material selected_mat;
    private Material selected_base_mat;

    // If this piece is currently selected
    protected bool selected = false;

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

    public bool getSelected()
    {
        return selected;
    }

    public void setSelected(bool b)
    {
        Renderer R = GetComponent<Renderer>();
        if (!R) R = GetComponentInChildren<Renderer>();
        if (b)
        {
            selected_base_mat = R.material;
            R.material = selected_mat;
        }
        else
        {
            R.material = selected_base_mat;
            selected_base_mat = null;
        }
        selected = b;
    }
}
