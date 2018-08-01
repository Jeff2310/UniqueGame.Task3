using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

public class EventScript : EventBase
{
    [MultiLineProperty]
    public string namespaces;
    [MultiLineProperty(6)]
    public string code;


}