using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

public class EventBase
{
    [HideInEditorMode]
    [ReadOnly]
    public bool Processing = false;

    [InfoBox("This is an EMPTY BASE EVENT,you shouldn't have created this,try to set it to other child Events.", InfoMessageType.Info)]

    public virtual string GetLabel()
    {
        return GetType().Name;
    }
}