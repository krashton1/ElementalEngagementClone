using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    static public Dictionary<string, Ability> abilities = new Dictionary<string, Ability>();
    public MapController controller;
    public PlayerBehaviour resourceManager;

    void Start()
    {
         initialiseAbilities();
    }

    void initialiseAbilities(){
        // TO DO: Read this data from a textfile
        abilities.Add("construction_Tower", new ConstructionAbility((GameObject) Resources.Load("Prefabs/Structures/Structure_Tower", typeof(GameObject)), controller, resourceManager, "Build Tower", 5));
        abilities.Add("construction_House", new ConstructionAbility((GameObject) Resources.Load("Prefabs/Structures/Structure_House", typeof(GameObject)), controller, resourceManager, "Build House", 2));
        abilities.Add("construction_Pylon", new ConstructionAbility((GameObject) Resources.Load("Prefabs/Structures/Structure_Pylon", typeof(GameObject)), controller, resourceManager, "Build Pylon", 2, 2));
        abilities.Add("construction_Barracks", new ConstructionAbility((GameObject) Resources.Load("Prefabs/Structures/Structure_Barracks", typeof(GameObject)), controller, resourceManager, "Build Barracks", 3));
        abilities.Add("construction_MiningCamp", new ConstructionAbility((GameObject) Resources.Load("Prefabs/Structures/Structure_MiningCamp", typeof(GameObject)), controller, resourceManager, "Build Mining Camp", 3));
        abilities.Add("unit_Worker", new UnitSpawnerAbility((GameObject) Resources.Load("Prefabs/AllyWorker", typeof(GameObject)), resourceManager, "Train Worker", 3));
        abilities.Add("unit_Soldier", new UnitSpawnerAbility((GameObject) Resources.Load("Prefabs/AllySoldier", typeof(GameObject)), resourceManager, "Train Soldier", 3));

    }
}
