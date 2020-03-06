using UnityEngine;

public class Unit : MonoBehaviour {
    public int health = 100;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Damage(int damage_amount)
    {
        health -= damage_amount;
        // Debug.Log(name + ": Took " + damage_amount + " damage ||| " + health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }

    }
}
