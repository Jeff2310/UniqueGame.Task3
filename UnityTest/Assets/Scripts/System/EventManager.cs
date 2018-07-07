using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

public class EventManager : Singleton<EventManager> {

    [ReadOnly]
    public List<Intepreter> currentInepreters = new List<Intepreter>();

    public void TryContinueDialog()
    {
        foreach (var i in currentInepreters)
        {
            if (i.currentEvent is EventDialog)
            {
                i.DialogContinue();
            }
            else
            {
                Debug.Log("There is no Dialog running");
            }
        }
    }
}
