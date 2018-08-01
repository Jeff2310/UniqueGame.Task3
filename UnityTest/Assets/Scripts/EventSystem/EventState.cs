using UnityEngine;
using UnityEditor;

public class EventState : EventBase
{
    public string StateName;
    public int Value;

    public override string GetLabel()
    {
        return string.Format("Set State [{0}] to {1}", StateName, Value);
    }
}