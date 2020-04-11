using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Mana Pylon
// Mana pylons generate mana over time. They generate less mana if they are close together

public class ManaPylon : MonoBehaviour
{

    public int baseManaRate = 5; // seconds per mana
    public int manaRate = 5;
    PlayerBehaviour p;
    public AnimationCurve distanceCurve;
    float counter = 0;
    bool finished = false;

    // Start is called before the first frame update
    void Start()
    {
        p = GameObject.Find("PlayerController").GetComponent<PlayerBehaviour>();
    }

    public void OnConstruction(){
        finished = true;
        updateManaRate();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 12, LayerMask.GetMask("Choosable"));
        for (int i = 0; i < hitColliders.Length; i++)
        {
            ManaPylon s = hitColliders[i].gameObject.GetComponent<ManaPylon>();
            if (!s) continue;
            s.updateManaRate();
            
        }

    }

    private void Update() {
        if (finished){
        counter += Time.deltaTime;
        if (counter > manaRate){
            p.addMana(1);
            counter = 0;
        }
        }   
    }

    // Find nearest pylon to determine mana generation rate
    public void updateManaRate(){
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 24, LayerMask.GetMask("Choosable"));
        ManaPylon nearest = null;
        float distanceToNearest = 24;

        for (int i = 0; i < hitColliders.Length; i++)
        {
            ManaPylon s = hitColliders[i].gameObject.GetComponent<ManaPylon>();
            if (!s) continue;

            float dist = Vector3.Distance(transform.position, s.transform.position);
            if (dist >= distanceToNearest || s.GetComponent<Entity>().MarkedForDeletion || s == this) continue;
            nearest = s;
            distanceToNearest = dist;
        }
        print(distanceCurve.Evaluate(distanceToNearest));
        manaRate = Mathf.FloorToInt(baseManaRate * distanceCurve.Evaluate(distanceToNearest));

    }

    private void OnDestroy() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 12, LayerMask.GetMask("Choosable"));
        for (int i = 0; i < hitColliders.Length; i++)
        {
            ManaPylon s = hitColliders[i].gameObject.GetComponent<ManaPylon>();
            if (!s) continue;
            s.updateManaRate();
        }
    }
}
