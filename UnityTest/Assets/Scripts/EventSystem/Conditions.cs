using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Condition {
    public string name;

	public bool Check()
    {
        switch (name)
        {
            case "Check switch 1":
                return GameSwitches.Instance.GetSwitch(1);

            





            default:
                Debug.LogWarning("There is no condition named '" + name + "'!");
                return false;
        }
    }
    
}
