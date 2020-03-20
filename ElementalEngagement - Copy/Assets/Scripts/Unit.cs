using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : Entity {
    public int healthCap = 100;
    public int healthCurrent;
    public Slider hpBarUi;


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
}
