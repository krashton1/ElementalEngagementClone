using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{

	private int mGold = 15;
	private int mMana = 10;
	private int mPopulation = 0;

	private int mGoldCap = 10000;
	private int mManaCap = 100;
	private int mPopulationCap = 5;


	private float mElapsedTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		mElapsedTime += Time.deltaTime;
		if(mElapsedTime>=5.0f)
		{
			mElapsedTime = mElapsedTime % 5.0f;

			if (mMana < mManaCap)
			{
				mMana++;
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

	public void setGold(int i )
	{
		mGold = i;
	}

	public void setPopulation(int i)
	{
		mPopulation = i;
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

	public void addPopulationCap(int i){
		mPopulationCap += i;
	}

	public void addGold(int i){
		if (mGold + i >= 0 && mGold + i <= mGoldCap){
			mGold += i;
		}
	}

	public void addMana(int i){
		if (mMana + i >= 0 && mMana + i <= mManaCap){
			mMana += i;
		}
	}

	public void addPop(int i){
		mPopulation += i;
	}
}
