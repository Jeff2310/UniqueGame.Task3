using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

public class EventChoice : EventBase
{
    /*
    [DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.OneLine)]
    public Dictionary<string, List<EventBase>> choices = new Dictionary<string, List<EventBase>>();
    public enum ChoiceType { Normal, Ultra }
    [EnumToggleButtons]
    public ChoiceType choiceType;
    #if UNITY_EDITOR
    #endif
    */
    public BranchConversation ConversationScript;
}