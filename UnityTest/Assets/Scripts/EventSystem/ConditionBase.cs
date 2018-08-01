using UnityEngine;

public class ConditionBase {
    public string name;

    public virtual bool Check()
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
