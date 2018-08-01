using UnityEngine;
using UnityEditor;

public class ConditionState : ConditionBase
{
    public string StateName;
    public int Value;

    public override bool Check()
    {
        return StateManager.Instance.GetState(StateName) == Value;
    }
}