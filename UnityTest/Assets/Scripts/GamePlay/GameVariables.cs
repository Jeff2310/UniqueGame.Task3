using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameVariables : Singleton<GameVariables>
{
	public const int capacity = 100;
	private float[] variables = new float[capacity];

	public void SetVar(int index,float value)
	{
		if (index>=capacity||index<0) {
			Debug.LogWarning("The index of the GameVariable is OUT OF RANGE!!");
			return;
		}
		variables [index] = value;
	}

	public float GetVar(int index)
	{
		if (index>=capacity||index<0) {
			Debug.LogWarning("The index of the GameVariable is OUT OF RANGE!!");
			return 0f;
		}
		return variables [index];
	}
}

