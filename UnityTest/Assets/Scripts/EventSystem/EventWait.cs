using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

public class EventWait : EventBase
{
    public enum WaitUnit { Seconds, Frames }
    [EnumToggleButtons]
    public WaitUnit waitUnit;
    [PropertyRange(0, 999)]
    public float amount;

    public override string GetLabel()
    {
        string label = "Wait - Wait for ";
        label += amount;
        label += " ";
        label += waitUnit.ToString();
        return label;
    }
}