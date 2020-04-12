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
        abilities.Add("construction_Tower", new ConstructionAbility((GameObject) Resources.Load("Prefabs/Structures/Structure_Tower", typeof(GameObject)), controller, resourceManager, "Build Tower", 25));
        abilities.Add("construction_House", new ConstructionAbility((GameObject) Resources.Load("Prefabs/Structures/Structure_House", typeof(GameObject)), controller, resourceManager, "Build House", 15));
        abilities.Add("construction_Pylon", new ConstructionAbility((GameObject) Resources.Load("Prefabs/Structures/Structure_Pylon", typeof(GameObject)), controller, resourceManager, "Build Pylon", 5, 3));
        abilities.Add("construction_Barracks", new ConstructionAbility((GameObject) Resources.Load("Prefabs/Structures/Structure_Barracks", typeof(GameObject)), controller, resourceManager, "Build Barracks", 20));
        abilities.Add("construction_MiningCamp", new ConstructionAbility((GameObject) Resources.Load("Prefabs/Structures/Structure_MiningCamp", typeof(GameObject)), controller, resourceManager, "Build Mining Camp", 10));
        abilities.Add("construction_Wall", new ConstructionAbility((GameObject) Resources.Load("Prefabs/Structures/Structure_Wall", typeof(GameObject)), controller, resourceManager, "Build Wall", 8));       
        abilities.Add("unit_Worker", new UnitSpawnerAbility((GameObject) Resources.Load("Prefabs/AllyWorker", typeof(GameObject)), resourceManager, "Train Worker", 5));
        abilities.Add("unit_Soldier", new UnitSpawnerAbility((GameObject) Resources.Load("Prefabs/AllySoldier", typeof(GameObject)), resourceManager, "Train Soldier", 8));
        abilities.Add("unit_Ranger", new UnitSpawnerAbility((GameObject) Resources.Load("Prefabs/AllyRanger", typeof(GameObject)), resourceManager, "Train Ranger", 9));
        abilities.Add("debug_SuperWorker", new UnitSpawnerAbility((GameObject) Resources.Load("Prefabs/SuperWorker", typeof(GameObject)), resourceManager, "(Debug) FastWorker", 0));
        abilities.Add("enchant_nature", new EnchantAbility( resourceManager, ElementComponent.ElementType.Grass, "Enchant (Grass)", 0, 10));
        abilities.Add("enchant_fire", new EnchantAbility( resourceManager, ElementComponent.ElementType.Fire, "Enchant (Fire)", 0, 10));
        abilities.Add("enchant_water", new EnchantAbility( resourceManager, ElementComponent.ElementType.Water, "Enchant (Water)", 0, 10));

    }
}
