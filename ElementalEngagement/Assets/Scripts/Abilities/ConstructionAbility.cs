using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionAbility : Ability
{
    GameObject prefab;
    MapController controller;
    PlayerBehaviour resourceManager;

    public ConstructionAbility(GameObject prefab_, MapController controller_, PlayerBehaviour resources_, string name_, int goldCost=0, int manaCost=0){
        prefab = prefab_;
        controller = controller_;
        cost = new Cost(goldCost, manaCost);
        fullName = name_;
        resourceManager = resources_;
    }

    public override void invoke(){
        controller.listenForSelection(OnSelection);
        Structure s = prefab.GetComponent<Structure>();
        controller.beginTileSelection((int)s.dimensions.x, (int)s.dimensions.y);

    }

    public void OnSelection(){

        if (resourceManager.getGold() >= cost.gold && resourceManager.getMana() >=cost.mana){
            resourceManager.addGold(-cost.gold);
            resourceManager.addMana(-cost.mana);
        

        GameObject newStructure = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        if (Random.value < 0.5f) {
            newStructure.transform.Rotate(new Vector3(0, 90, 0));
        }
        Structure s = newStructure.GetComponent<Structure>();
        newStructure.name = s.type + newStructure.GetInstanceID();
        Vector2 pos = controller.getSelectedPosition();
        s.setGridPosition(pos);
        newStructure.transform.position = controller.placeStructureAt(s, (int)pos.x, (int)pos.y);
        }
    }
}
