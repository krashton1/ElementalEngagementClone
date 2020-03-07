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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		gold_text.text = "Gold: " + mPlayer.getGold() + "/" + mPlayer.getGoldCap();
		mana_text.text = "Mana: " + mPlayer.getMana() + "/" + mPlayer.getManaCap();
		population_text.text = "Pop: " + mPlayer.getPopulation() + "/" + mPlayer.getPopulationCap();
    }
}
