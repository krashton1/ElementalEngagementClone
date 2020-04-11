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

    public override void invoke(GameObject source, bool repeated=false){
        controller.listenForSelection(OnSelection);
        Structure s = prefab.GetComponent<Structure>();
        controller.beginTileSelection((int)s.dimensions.x, (int)s.dimensions.y);

    }

    public void OnSelection(){

        if (resourceManager.getGold() >= cost.gold && resourceManager.getMana() >=cost.mana){
            resourceManager.addGold(-cost.gold);
            resourceManager.addMana(-cost.mana);
        

        GameObject newStructure = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        Structure s = newStructure.GetComponent<Structure>();
        s.Create();
        Vector2 pos = controller.getSelectedPosition();
        s.setGridPosition(pos);
        newStructure.transform.position = controller.placeStructureAt(s, (int)pos.x, (int)pos.y);
        }
    }
}
