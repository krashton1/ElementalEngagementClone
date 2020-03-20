using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityContainer : MonoBehaviour
{
    public List<string> abilityNames;
    List<Ability> abilities = new List<Ability>();

    
    // Start is called before the first frame update
    void Start()
    {
        foreach (string name in abilityNames){
            abilities.Add(AbilityManager.abilities[name]);
        }
    }

    // Update is called once per frame
    public List<Ability> GetAbilities(){
        return abilities;
    }
}
