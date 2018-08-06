using System;
using UnityEngine;
using System.Collections.Generic;
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


public class EventDialog : EventBase
{
    public enum DialogType { Conversation, Thought, Monologue }
    public enum DialogPos { Bottom, Middle, Top }


    [TabGroup("Content")]
    [HideIf("dialogType",DialogType.Monologue)]
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
        lable += '\n';
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

public class EventCondition:EventBase
{
    [Title(title:"Conditions")]
    [HideReferenceObjectPicker]
    public List<Condition> conditions = new List<Condition>();


    [Title(title:"Events to Excute if Conditions are True")]
    [ListDrawerSettings(OnBeginListElementGUI ="BeginDrawIfList",OnEndListElementGUI = "EndDrawEventList",ShowIndexLabels = true)]
    public List<EventBase> ifContent = new List<EventBase>();


    [Title(title:"Events to Excute if Conditions are False")]
    [ListDrawerSettings(OnBeginListElementGUI ="BeginDrawElseList",OnEndListElementGUI = "EndDrawEventList", ShowIndexLabels = true)]
    public List<EventBase> elseContent = new List<EventBase>();
    
    [Title(title:"",subtitle:"Whether this will loop when the conditions are still True")]
    public bool loop = false;



    //Editor modifications
#if UNITY_EDITOR
    private void BeginDrawIfList(int index)
    {
        var e = ifContent[index];
        if (e == null)
        {
            return;
        }
        else if (e is EventDialog)
        {
            GUIHelper.PushColor(new Color(0.2f, 0.75f, 0.9f));
        }
        else if (e is EventWait)
        {
            GUIHelper.PushColor(new Color(0.7f, 0.8f, 0.5f));
        }
        else if (e is EventInventory)
        {
            GUIHelper.PushColor(new Color(0.8f, 0.6f, 0.4f));
        }
        else if (e is EventLabel || e is EventJumpToLabel)
        {
            GUIHelper.PushColor(new Color(0.4f, 0.5f, 0.7f));
        }
        else if (e is EventSwitch || e is EventVariable)
        {
            GUIHelper.PushColor(new Color(0.8f, 0.4f, 0.4f));
        }
        else
        {
            GUIHelper.PushColor(Color.gray);
        }
        SirenixEditorGUI.BeginBox(ifContent[index].GetLabel());
    }
    private void BeginDrawElseList(int index)
    {
        var e = elseContent[index];
        if (e == null)
        {
            return;
        }
        else if (e is EventDialog)
        {
            GUIHelper.PushColor(new Color(0.2f, 0.75f, 0.9f));
        }
        else if (e is EventWait)
        {
            GUIHelper.PushColor(new Color(0.7f, 0.8f, 0.5f));
        }
        else if (e is EventInventory)
        {
            GUIHelper.PushColor(new Color(0.8f, 0.6f, 0.4f));
        }
        else if (e is EventLabel || e is EventJumpToLabel)
        {
            GUIHelper.PushColor(new Color(0.4f, 0.5f, 0.7f));
        }
        else if (e is EventCondition)
        {
            GUIHelper.PushColor(new Color(0.8f, 0.4f, 0.4f));
        }
        else
        {
            GUIHelper.PushColor(Color.gray);
        }
        SirenixEditorGUI.BeginBox(elseContent[index].GetLabel());
    }
    private void EndDrawEventList(int index)
    {
        GUIHelper.PopColor();
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

public class EventChoice : EventBase
{
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine)]
    public Dictionary<string, List<EventBase>> choices = new Dictionary<string, List<EventBase>>();
    public enum ChoiceType { Normal,Ultra}
    [EnumToggleButtons]
    public ChoiceType choiceType;
    
#if UNITY_EDITOR
    
# endif

}

public class EventInventory : EventBase
{
    public ItemBase item;
    public enum ItemChangeType { Gain,Lose}
    public ItemChangeType itemChangeType;
    public int amount = 1;

    public override string GetLabel()
    {
        string text = "Inventory - ";
        text += itemChangeType.ToString();
        text += " ";
        text += amount;
        text += " ";
        if (item == null)
        {
            text += "None";
        }
        else
        {
            text += item.itemName;
        }
        
        return text;
        
    }
}

public class EventSwitch : EventBase {

    public int switchIndex = 0;
    public enum SwitchOperation { SetToFalse, SetToTrue }
    [EnumToggleButtons]
    public SwitchOperation operation;

}

public class EventVariable : EventBase
{
    public int varIndex = 0;
    public enum VarOperation { EqualsTo,Plus,Minus,Multiply}
    [EnumToggleButtons]
    public VarOperation operation;
    public float number;
}

public class EventLabel:EventBase
{
    public string labelName;

    public override string GetLabel()
    {
        return "Label: " + labelName;
    }
}

public class EventJumpToLabel:EventBase
{
    public string toLabelName;

    public override string GetLabel()
    {
        return "Jump to the label:" + toLabelName;
    }

}


public class EventCustom:EventBase
{
    
    public delegate void CustomFunc();
    public CustomFunc func;

}
