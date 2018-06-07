using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.OdinInspector;
#endif

public class EventManager : Singleton<EventManager> {

    [ReadOnly]
    public List<Interpreter> Interpreter = new List<Interpreter>();

    public Interpreter CurrentInterpreter;

    public void TryContinueDialog()
    {
//        foreach (var i in Interpreter)
//        {
//            if (i.currentEvent is EventDialog)
//            {
//                i.DialogContinue();
//            }
//        }
        if (CurrentInterpreter == null) return;
        if (CurrentInterpreter.currentEvent is EventDialog)
        {
            CurrentInterpreter.DialogContinue();
        }
    }
}
