using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
	public Slider mHealthBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		mHealthBar.enabled = true;

		mHealthBar.transform.position = transform.position + new Vector3(0.0f, 50.0f, 0.0f);
		mHealthBar.value = 0.20f;
    }
}
