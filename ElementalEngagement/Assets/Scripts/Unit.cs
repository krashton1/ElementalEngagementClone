using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour {
    public int healthCap = 100;
	public int healthCurrent;
	public Slider hpBarUi;

	// If this piece is currently selected
	protected bool selected = false;


	// Start is called before the first frame update
	void Start()
	{
		// This isnt being called, unexpected behaviour
	}

    // Update is called once per frame
    void Update()
    {
		// This isnt being called, unexpected behaviour
	}

	public void takeDamage(int damage_amount)
    {
        healthCap -= damage_amount;
        if (healthCap < 0)
        {
            Destroy(gameObject);
        }

    }

	public bool getSelected()
	{
		return selected;
	}

	public void setSelected(bool b)
	{
		selected = b;
	}
}
