using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

public class EventCondition : EventBase
{
    [Title(title: "Conditions")]
    [HideReferenceObjectPicker]
    public List<ConditionBase> conditions = new List<ConditionBase>();


    [Title(title: "Events to Excute if Conditions are True")]
    [ListDrawerSettings(OnBeginListElementGUI = "BeginDrawIfList", OnEndListElementGUI = "EndDrawEventList")]
    public List<EventBase> ifContent = new List<EventBase>();


    [Title(title: "Events to Excute if Conditions are False")]
    [ListDrawerSettings(OnBeginListElementGUI = "BeginDrawElseList", OnEndListElementGUI = "EndDrawEventList")]
    public List<EventBase> elseContent = new List<EventBase>();

    [Title(title: "", subtitle: "Whether this will loop when the conditions are still True")]
    public bool loop = false;


    //Editor modifications
#if UNITY_EDITOR
    private void BeginDrawIfList(int index)
    {
        SirenixEditorGUI.BeginBox(ifContent[index].GetLabel());
    }
    private void BeginDrawElseList(int index)
    {
        SirenixEditorGUI.BeginBox(elseContent[index].GetLabel());
    }
    private void EndDrawEventList(int index)
    {
        SirenixEditorGUI.EndBox();
    }
#endif
    //--------------------

    public override string GetLabel()
    {
        string label = "Condition - If " + conditions.Count + " Conditions are True.  - ";
        switch (loop)
        {
            case true:
                label += "Do ";
                break;
            case false:
                label += "Not ";
                break;
        }
        label += "loop.";
        return label;
    }
}

