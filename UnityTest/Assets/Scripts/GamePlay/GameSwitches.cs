using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSwitches : Singleton<GameSwitches> {

	public const int capacity = 100;
	private bool[] switches = new bool[capacity];

    public void SetSwitch(int index,bool statement){
		if (index>=capacity||index<0) {
			Debug.LogWarning("The index of the GameSwitch is OUT OF RANGE!!");
			return;
		}

		switches [index] = statement;
	}

	public bool GetSwitch(int index){
		
		if (index>=capacity||index<0) {
			Debug.LogWarning("The index of the GameSwitch is OUT OF RANGE!!");
			return false;
		}

		return switches [index];
	}

}
