using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability
{
    //do nothing for now
    public abstract void invoke();

    protected Cost cost;
    protected string fullName;

    public string getName(){
        return fullName;
    }

    public Cost getCost(){
        return cost;
    }

}

public struct Cost{
    public int gold;
    public int mana;

    public Cost(int goldCost=0, int manaCost=0){
        gold = goldCost;
        mana= manaCost;
    }
        
    
}
