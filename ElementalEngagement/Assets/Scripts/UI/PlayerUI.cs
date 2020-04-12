using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

	public PlayerBehaviour mPlayer;

	public Text gold_text;
	public Text mana_text;
	public Text population_text;
  public GameObject AbilitiesPanel;
  public GameObject EntityPanel;
  public Text wave_text;
  public WaveSpawner waveControl;
  public Text gameOver_text;
  bool gameOver =false;
  float timer = 0;

  public GameObject abilityButtonPrefab;

  Entity selectedEntity = null;
  public Text healthText;
  public Slider healthSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      if (!gameOver) timer += Time.deltaTime;
      if (selectedEntity){
        healthText.text = selectedEntity.healthCurrent.ToString() + " / " + selectedEntity.healthCap.ToString();
        healthSlider.value = (float)selectedEntity.healthCurrent / selectedEntity.healthCap;
        GameObject resourceText = EntityPanel.transform.Find("ResourceText").gameObject;
        if (resourceText.activeSelf){
        if (selectedEntity.GetComponent<WorkComponent>()){
            resourceText.GetComponent<Text>().text = "Carrying " + selectedEntity.GetComponent<WorkComponent>().resourceCount.ToString() + " ore.";
        }
        else {
          if (selectedEntity.GetComponent<ManaPylon>()){
            resourceText.GetComponent<Text>().text = "Generating 1 mana / " + selectedEntity.GetComponent<ManaPylon>().manaRate.ToString() + " s.";
          }
        }
        }
      }
		  gold_text.text = "Gold: " + mPlayer.getGold() + "/" + mPlayer.getGoldCap();
		  mana_text.text = "Mana: " + mPlayer.getMana() + "/" + mPlayer.getManaCap();
		  population_text.text = "Pop: " + mPlayer.getPopulation() + "/" + mPlayer.getPopulationCap();
      wave_text.text = "Next wave in " + Mathf.RoundToInt(waveControl.Spawn_interval - waveControl.currTimer);
    }

    void setEntityPanelEnabled(bool enabled){
      EntityPanel.SetActive(enabled);
    }

    void setAbilitiesPanelEnabled(bool enabled){
      AbilitiesPanel.SetActive(enabled);
      foreach (Transform child in AbilitiesPanel.transform){
        Destroy(child.gameObject);
      }
    }

    public void setSelectedEntity(Entity entity){
      setEntityPanelEnabled(true);
      selectedEntity = entity;
      EntityPanel.transform.Find("NameText").GetComponent<Text>().text = entity.entityName;
      GameObject resourceText = EntityPanel.transform.Find("ResourceText").gameObject;
      WorkComponent w = entity.GetComponent<WorkComponent>();
      ManaPylon m = entity.GetComponent<ManaPylon>();

      if (w){
        resourceText.SetActive(true);
        resourceText.GetComponent<Text>().text = "Carrying " + w.resourceCount.ToString() + " ore.";
      }
      else if (m) {
        resourceText.SetActive(true);
        resourceText.GetComponent<Text>().text = "Generating 1 mana / " + m.manaRate.ToString() + " s.";
      }
      else{
        resourceText.SetActive(false);
      }
      // Display the ability panel
 
      if (entity.gameObject.GetComponent<AbilityContainer>()){
        if (entity.gameObject.GetComponent<AbilityContainer>().enabled){
          setAbilitiesPanelEnabled(true);
          foreach (Ability a in entity.gameObject.GetComponent<AbilityContainer>().GetAbilities()){
            GameObject b = GameObject.Instantiate(abilityButtonPrefab, Vector3.zero, Quaternion.identity, AbilitiesPanel.transform);

              b.GetComponentInChildren<Text>().text = a.getCost().gold.ToString() + "G " + a.getCost().mana.ToString() + "M :" + a.getName();
              b.GetComponent<Button>().onClick.AddListener(delegate{a.invoke(selectedEntity.gameObject);});
          }
        }
      }
      healthSlider.gameObject.SetActive(true);
    }

    public void selectMany(int count){
      setEntityPanelEnabled(true);
      EntityPanel.transform.Find("NameText").GetComponent<Text>().text = count.ToString() + " units selected.";
      healthSlider.gameObject.SetActive(false);
    }

    public void deselect(){
      selectedEntity = null;
      setAbilitiesPanelEnabled(false);
      setEntityPanelEnabled(false);
    }

    public void endGame(){
      gameOver_text.enabled = true;
      gameOver = true;
      gameOver_text.text = "Game Over! You survived " + (int)timer + "seconds!";
    }
}


