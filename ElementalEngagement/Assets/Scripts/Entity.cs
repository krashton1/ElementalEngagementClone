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

		switch (et)
		{
			case ElementComponent.ElementType.Fire:
				go = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadMainAssetAtPath("Assets/ErbGameArt/Procedural fire/Prefabs/Magic fire pro red.prefab")) as GameObject;
				break;
			case ElementComponent.ElementType.Water:
				go = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadMainAssetAtPath("Assets/ErbGameArt/Procedural fire/Prefabs/Magic fire pro blue.prefab")) as GameObject;
				break;
			case ElementComponent.ElementType.Grass:
				go = PrefabUtility.InstantiatePrefab(AssetDatabase.LoadMainAssetAtPath("Assets/ErbGameArt/Procedural fire/Prefabs/Magic fire pro green.prefab")) as GameObject;
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

    public void Damage(float damage_amount, ElementComponent.ElementType elem_type = ElementComponent.ElementType.None)
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
