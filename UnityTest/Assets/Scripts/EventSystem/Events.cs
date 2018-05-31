using System.Collections.Generic;
using Sirenix.Serialization;
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
    public List<bool> conditions = new List<bool>();


    [Title(title:"Events to Excute if Conditions are True")]
    [ListDrawerSettings(OnBeginListElementGUI ="BeginDrawIfList",OnEndListElementGUI = "EndDrawEventList")]
    public List<EventBase> ifContent = new List<EventBase>();


    [Title(title:"Events to Excute if Conditions are False")]
    [ListDrawerSettings(OnBeginListElementGUI ="BeginDrawElseList",OnEndListElementGUI = "EndDrawEventList")]
    public List<EventBase> elseContent = new List<EventBase>();
    
    [Title(title:"",subtitle:"Whether this will loop when the conditions are still True")]
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

public class EventScript:EventBase
{
    [MultiLineProperty]
    public string namespaces;
    [MultiLineProperty(6)]
    public string code;


}
