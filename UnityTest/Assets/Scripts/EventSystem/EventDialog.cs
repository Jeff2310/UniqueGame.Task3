using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

public class EventDialog : EventBase
{
    public enum DialogType { Conversation, Thought, Monologue }
    public enum DialogPos { Bottom, Middle, Top }


    [TabGroup("Content")]
    [HideIf("dialogType", DialogType.Monologue)]
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