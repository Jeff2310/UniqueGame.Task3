using UnityEngine;
using UnityEditor;

public class EventLabel : EventBase
{
    public string labelName;

    public override string GetLabel()
    {
        return "Label: " + labelName;
    }
}