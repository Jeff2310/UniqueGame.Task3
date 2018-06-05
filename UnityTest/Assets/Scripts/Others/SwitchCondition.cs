using UnityEngine;
using System.Collections;

[System.Serializable]
public class SwitchCondition
{
    public enum CompareType { IfTrue,IfFalse }

    public int switchIndex;
    public CompareType compareType;

    public bool Condition()
    {
        bool s = GameSwitches.Instance.GetSwitch(switchIndex);

        switch (compareType)
        {
            case CompareType.IfTrue:
                if (s==true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case CompareType.IfFalse:
                if (s==false)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                return false;
        }
    }
}