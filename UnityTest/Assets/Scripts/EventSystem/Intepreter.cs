using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;  
using System;

#if UNITY_EDITOR

using Sirenix.Utilities.Editor;

#endif

public class Intepreter : SerializedMonoBehaviour {

    [Title(title: "Conditions")]
    public List<bool> conditions;
    
    [Title(title:"Event Contents")]
    [ListDrawerSettings(OnBeginListElementGUI ="BeginDrawEventList",OnEndListElementGUI = "EndDrawEventList")]
    public List<EventBase> events = new List<EventBase>();


    //Editor modifications
#if UNITY_EDITOR
    private void BeginDrawEventList(int index)
    {
        SirenixEditorGUI.BeginBox(events[index].GetLabel());
    }
    private void EndDrawEventList(int index)
    {
        SirenixEditorGUI.EndBox();
    }
#endif
    //--------------------


}

