using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnerAbility : Ability
{
    GameObject prefab;
    PlayerBehaviour resourceManager;

    public UnitSpawnerAbility(GameObject prefab_, PlayerBehaviour resources_, string name_, int goldCost=0, int manaCost=0){
        prefab = prefab_;
        cost = new Cost(goldCost, manaCost);
        fullName = name_;
        resourceManager = resources_;
    }

    public override void invoke(GameObject source){
       if(resourceManager.getPopulation() < resourceManager.getPopulationCap()
       && resourceManager.getMana() >= cost.mana && resourceManager.getGold() >= cost.gold)
		{
				resourceManager.addGold(-cost.gold);
                resourceManager.addMana(-cost.mana);
				resourceManager.setPopulation(resourceManager.getPopulation() + 1);
				GameObject newUnit = GameObject.Instantiate(prefab, source.transform.position, new Quaternion());
                if (source.GetComponent<UnitSpawner>()){
                    source.GetComponent<UnitSpawner>().onUnitSpawn(newUnit);
                }
		}
    }
}
