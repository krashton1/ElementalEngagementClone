using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementComponent : MonoBehaviour
{

	public enum ElementType { None, Fire, Water, Grass }
	public ElementType element_type;

	private Dictionary<ElementType, Tuple<ElementType, ElementType>> element_chart; // key is elem, first elem of tuple is strong rival elem(s), second elem is weak rival elem(s)

	// Start is called before the first frame update
	void Start()
	{
		element_chart.Add(ElementType.Fire, new Tuple<ElementType, ElementType> (ElementType.Water, ElementType.Grass));
		element_chart.Add(ElementType.Water, new Tuple<ElementType, ElementType>(ElementType.Grass, ElementType.Fire));
		element_chart.Add(ElementType.Grass, new Tuple<ElementType, ElementType>(ElementType.Fire, ElementType.Water));
	}


	static public ElementType getStrength(ElementType et)
	{
		switch(et)
		{
			case ElementType.Fire:
				return ElementType.Grass;
			case ElementType.Water:
				return ElementType.Fire;
			case ElementType.Grass:
				return ElementType.Water;
			default:
				return ElementType.None;
		}
	}

	static public ElementType getWeakness(ElementType et)
	{
		switch (et)
		{
			case ElementType.Fire:
				return ElementType.Water;
			case ElementType.Water:
				return ElementType.Grass;
			case ElementType.Grass:
				return ElementType.Fire;
			default:
				return ElementType.None;
		}
	}
}
