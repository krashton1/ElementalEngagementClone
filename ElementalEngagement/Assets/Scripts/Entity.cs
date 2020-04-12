using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;
using UnityEngine.UI;
using UnityEditor;

public class Entity : MonoBehaviour
{

    public enum TypeOfTarget {Entity, GroundPosition};


    // If this piece is currently selected
    protected bool selected = false;


    public float healthCap = 100;
    public float healthCurrent;
    public Slider hpBarUi;
    public string entityName;
	public ElementComponent.ElementType element_type;
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
    
	public void setElementType(ElementComponent.ElementType et)
	{
		element_type = et;



		GameObject go;
        foreach (Transform child in gameObject.transform) {
            if (child.CompareTag("ElementalFX")) 
            {Destroy(child.gameObject);
                break;
            }
        }
		switch (et)
		{
			case ElementComponent.ElementType.Fire:
				go = GameObject.Instantiate((GameObject) Resources.Load("ErbGameArt/Procedural fire/Prefabs/Magic fire pro red"));
				break;
			case ElementComponent.ElementType.Water:
				go = GameObject.Instantiate((GameObject) Resources.Load("ErbGameArt/Procedural fire/Prefabs/Magic fire pro blue"));
				break;
			case ElementComponent.ElementType.Grass:
				go = GameObject.Instantiate((GameObject) Resources.Load("ErbGameArt/Procedural fire/Prefabs/Magic fire pro green"));
				break;
			default:
				return;
		}
		
		go.transform.SetParent(this.transform);
		go.transform.position = this.transform.position;
		go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
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

    public void Damage(int damage_amount, ElementComponent.ElementType elem_type = ElementComponent.ElementType.None)
    {
		if (ElementComponent.getStrength(elem_type) == element_type)
			damage_amount *= 2;
		else if (ElementComponent.getWeakness(elem_type) == element_type)
			damage_amount /= 2;

		healthCurrent -= damage_amount;
        if (healthCurrent <= 0)
        {
            Destroy(gameObject);
        }

    }

    public void Delete(){
        MarkedForDeletion = true;
    }
}
