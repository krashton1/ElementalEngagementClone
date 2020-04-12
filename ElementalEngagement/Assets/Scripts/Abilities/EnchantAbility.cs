using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantAbility : Ability
{
    PlayerBehaviour resourceManager;
    ElementComponent.ElementType element;

    public EnchantAbility(PlayerBehaviour resources_, ElementComponent.ElementType element_, string name_, int goldCost=0, int manaCost=0){
        cost = new Cost(goldCost, manaCost);
        fullName = name_;
        resourceManager = resources_;
        element = element_;
    }

    public override void invoke(GameObject source, bool repeated=false){
       if(resourceManager.getMana() >= cost.mana)
		{
            resourceManager.addMana(-cost.mana);
			source.GetComponent<Entity>().setElementType(element);
		}
    }
}
