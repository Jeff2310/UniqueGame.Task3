using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

public class EventJumpToLabel : EventBase
{
    public string toLabelName;

    public override string GetLabel()
    {
        return "Jump to the label:" + toLabelName;
    }

}