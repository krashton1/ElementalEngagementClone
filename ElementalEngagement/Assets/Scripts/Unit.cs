using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour {
    public int healthCap = 100;
    public int healthCurrent;
    public Slider hpBarUi;

    public Material selected_mat;
    private Material selected_base_mat;

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

    public void Damage(int damage_amount)
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
        Renderer R = GetComponent<Renderer>();
        if (b)
        {
            selected_base_mat = R.material;
            R.material = selected_mat;
        }
        else
        {
            R.material = selected_base_mat;
            selected_base_mat = null;
        }
        selected = b;
    }
}
