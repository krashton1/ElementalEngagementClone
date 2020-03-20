using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityManager : MonoBehaviour
{
    Dictionary<string, Ability> abilities = new Dictionary<string, Ability>();
    public MapController controller;
    public PlayerBehaviour resourceManager;

    public GameObject buttonPrefab;

    // Start is called before the first frame update
    void Start()
    {
         initialiseAbilities();
         createButtons();
    }

    void initialiseAbilities(){
        abilities.Add("construction_Tower", new ConstructionAbility((GameObject) Resources.Load("Prefabs/Structures/Structure_Tower", typeof(GameObject)), controller, resourceManager, "Build Tower", 5));
        abilities.Add("construction_House", new ConstructionAbility((GameObject) Resources.Load("Prefabs/Structures/Structure_House", typeof(GameObject)), controller, resourceManager, "Build House", 2));
        abilities.Add("construction_Pylon", new ConstructionAbility((GameObject) Resources.Load("Prefabs/Structures/Structure_Pylon", typeof(GameObject)), controller, resourceManager, "Build Pylon", 2, 2));
        abilities.Add("construction_Baracks", new ConstructionAbility((GameObject) Resources.Load("Prefabs/Structures/Structure_Barracks", typeof(GameObject)), controller, resourceManager, "Build Barracks", 3));

    }

    void createButtons(){
        GameObject cv = GameObject.Find("UI");
        int i = 0;
        foreach(KeyValuePair<string, Ability> ability in abilities){
            GameObject buttonObj = GameObject.Instantiate(buttonPrefab, Vector3.zero, Quaternion.identity);
            buttonObj.GetComponentInChildren<Text>().text = ability.Value.getName();
            buttonObj.transform.SetParent(cv.transform, false);
            RectTransform rect = buttonObj.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(i * 160 + i * 15, 0); 
            Button button = buttonObj.GetComponent<Button>();
            button.onClick.AddListener(delegate{invokeAbility(ability.Value);});
            i++;
        }
    }

    void invokeAbility(Ability ability){
        ability.invoke();
    }

}
