using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

public class Choice
{
    public enum ChoiceType
    {
        Single, Continous, Expanding, Collapse
    }

    public ChoiceType Type = ChoiceType.Single;
    public string Text;
    public Choice ParentChoice;
    public int IndexInCurrent;
    public int IndexInParent;
    public bool Finished = false;
    public Interpreter Detail;
    public UnityEvent[] OnSelect;

    public Choice()
    {
        Type = ChoiceType.Single;
        Text = "Option";
    }

    public void Select(BranchConversation conversation)
    {
        Debug.Log(IndexInCurrent);
        if (Detail != null)
        {
            Detail.Trigger();
        }
        OnSelectBase(conversation);
        if (OnSelect != null)
        {
            foreach (var e in OnSelect)
            {
                e.Invoke();
            }
        }
    }

    protected virtual void OnSelectBase(BranchConversation conversation)
    {

    }
}