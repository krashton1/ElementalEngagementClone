using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Housing Component
// A very small component
// Structures with this component increase population cap when built

public class HousingComponent : MonoBehaviour
{

    public int capacity;
    PlayerBehaviour p;

    // Start is called before the first frame update
    void Start()
    {
        p = GameObject.Find("PlayerController").GetComponent<PlayerBehaviour>();
    }

    public void OnConstruction(){
        p.addPopulationCap(capacity);
    }

    private void OnDestroy() {
        p.addPopulationCap(-capacity);
    }
}
