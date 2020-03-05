﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

	private int mGold = 5;
	private int mMana = 10;
	private int mPopulation = 15;

	private int mGoldCap = 10;
	private int mManaCap = 20;
	private int mPopulationCap = 30;


	private float mElapsedTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		mElapsedTime += Time.deltaTime;
		if(mElapsedTime>=1.0f)
		{
			mElapsedTime = mElapsedTime % 1.0f;

			if(mGold<mGoldCap)
			{
				mGold++;
			}

			if (mMana < mManaCap)
			{
				mMana++;
			}

			if (mPopulation < mPopulationCap)
			{
				mPopulation++;
			}
		}

	}

	public int getGold()
	{
		return mGold;
	}

	public int getMana()
	{
		return mMana;
	}

	public int getPopulation()
	{
		return mPopulation;
	}

	public int getGoldCap()
	{
		return mGoldCap;
	}

	public int getManaCap()
	{
		return mManaCap;
	}

	public int getPopulationCap()
	{
		return mPopulationCap;
	}
}