
using System;
using System.Collections;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

public class EventBase
{

    [InfoBox("This is an EMPTY BASE EVENT,you shouldn't have created this,try to set it to other child Events.", InfoMessageType.Info)]
    
    public virtual string GetLabel()
    {
        return GetType().Name;
    }
}


public class EventDialog : EventBase
{
    public enum DialogType { Conversation, Thought, Monologue }
    public enum DialogPos { Bottom, Middle, Top }


    [TabGroup("Content")]
    [OdinSerialize]
    public string characterName = "";
    [TabGroup("Content")]
    [MultiLineProperty(lines: 4)]
    public string message = "";


    [TabGroup("Settings")]
    [EnumToggleButtons]
    public DialogType dialogType;
    [TabGroup("Settings")]
    [EnumToggleButtons]
    public DialogPos dialogPosition;
    [TabGroup("Settings")]
    public bool StartOfDialog;
    [TabGroup("Settings")]
    public bool EndOfDialog;

    public override string GetLabel()
    {
        if (!(this is EventDialog))
        {
            return "Null";
        }

        string lable = "Dialog - ";
        switch (dialogType)
        {
            case DialogType.Conversation:
                lable += characterName;
                lable += " Say: ";
                break;
            case DialogType.Thought:
                lable += characterName;
                lable += " Think: ";
                break;
            case DialogType.Monologue:
                lable += "Monologue: ";
                break;
            default:
                break;
        }

        if (message.Length > 20)
        {
            lable += message.Substring(0, 20);
            lable += "......";
        }
        else
        {
            lable += message;
        }
        return lable;
    }

}

public class EventWait : EventBase
{
    public enum WaitUnit { Seconds,Frames}
    [EnumToggleButtons]
    public WaitUnit waitUnit;
    [PropertyRange(0,999)]
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

public class EventCondition
{
    
}
