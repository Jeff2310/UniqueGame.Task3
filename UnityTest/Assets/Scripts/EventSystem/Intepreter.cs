using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

using UnityEngine;

#if UNITY_EDITOR

using Sirenix.Utilities.Editor;

#endif

public class Intepreter : SerializedMonoBehaviour {

    [Title(title: "Conditions")]
    public List<bool> conditions = new List<bool>();
    
    [Title(title:"Event Contents")]
    [OnInspectorGUI("CheckEventElements")]
    [ListDrawerSettings(OnBeginListElementGUI ="BeginDrawEventList",OnEndListElementGUI = "EndDrawEventList")]
    public List<EventBase> events = new List<EventBase>();

    public EventBase currentEvent { get; private set; }


    private int index = 0;
    private Dictionary<string, int> labels = new Dictionary<string, int>();

    //Editor modifications
#if UNITY_EDITOR
    private void CheckEventElements()
    {
        for(int i=0;i<events.Count;)
        {
            if (events[i] == null)
            {
                events.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }
    private void BeginDrawEventList(int index)
    {
        if (events[index] == null)
        {
            return;
        }
        SirenixEditorGUI.HorizontalLineSeparator(2);
        SirenixEditorGUI.BeginBox(events[index].GetLabel());
    }
    private void EndDrawEventList(int index)
    {
        if (events[index] == null)
        {
            return;
        }
        SirenixEditorGUI.EndBox();
        SirenixEditorGUI.HorizontalLineSeparator(2);
    }
#endif
    //--------------------

    public void Trigger()
    {

        StopCoroutine(ExcuteEvents());
        EventManager.Instance.currentInepreters.Add(this);
        StartCoroutine(ExcuteEvents());
        
    }
    private IEnumerator ExcuteEvents()
    {
        for(index = 0;index < events.Count;index++)
        {
            var e = events[index];
            if (e == null)
            {
                continue;
            }

            currentEvent = e;

            if (e is EventDialog)
            {
                DialogEvent();
            }
            if (e is EventWait)
            {
                WaitEvent();
            }
            if (e is EventLabel)
            {
                LabelEvent();
            }
            if (e is EventJumpToLabel)
            {
                JumpToLabelEvent();
            }


            //Won't continue until this one is finished
            while (e.Processing)
            {
                yield return null;
            }

            currentEvent = null;
        }

        index = 0;
        EventManager.Instance.currentInepreters.Remove(this);
    }









    #region All the functionalities to implement
    //Dialog Event Implemention
    private void DialogEvent()
    {
        EventDialog eDialog = currentEvent as EventDialog;
        eDialog.Processing = true;
        StopCoroutine("DialogTyping");
        StartCoroutine(DialogTyping());
    }
    private IEnumerator DialogTyping()
    {
        EventDialog eDialog = currentEvent as EventDialog;
        DialogHideContinue();
        DialogStartAnim();
        UIManager.Instance.nameText.text = eDialog.characterName;
        UIManager.Instance.dialogMessage.text = "";

        foreach (var letter in eDialog.message)
        {
            UIManager.Instance.dialogMessage.text += letter;
            switch (letter)
            {
                case '\n':
                case '.':
                    yield return new WaitForSeconds(0.1f);
                    break;
                case ',':
                case ' ':
                    yield return new WaitForSeconds(0.08f);
                    break;
                case '?':
                case '!':
                    yield return new WaitForSeconds(0.15f);
                    break;
            }
            yield return null;
        }

        DialogShowContinue();
    }
    private void DialogStartAnim()
    {
        if ((currentEvent as EventDialog).StartOfDialog)
        {
            UIManager.Instance.dialogBox.gameObject.GetComponent<Animator>().SetBool("Dialogging",true);
        }
    }
    private void DialogEndAnim()
    {
        if ((currentEvent as EventDialog).EndOfDialog)
        {
            UIManager.Instance.dialogBox.gameObject.GetComponent<Animator>().SetBool("Dialogging", false);
        }
    }
    private void DialogHideContinue()
    {
        UIManager.Instance.continueButton.gameObject.SetActive(false);
    }
    private void DialogShowContinue()
    {
        UIManager.Instance.continueButton.gameObject.SetActive(true);
    }
    public void DialogSkip()
    {
        
    }
    public void DialogContinue()
    {
        DialogEndAnim();
        currentEvent.Processing = false;
    }



    //Wait Event Implemention
    private void WaitEvent()
    {
        EventWait eWait = currentEvent as EventWait;
        eWait.Processing = true;
        StopCoroutine("WaitWaiting");
        StartCoroutine(WaitWaiting());
    }
    private IEnumerator WaitWaiting()
    {
        EventWait eWait = currentEvent as EventWait;
        switch (eWait.waitUnit)
        {
            case EventWait.WaitUnit.Seconds:
                yield return new WaitForSeconds(eWait.amount);
                break;
            case EventWait.WaitUnit.Frames:
                for (int i = 0; i < eWait.amount; i++)
                {
                    yield return null;
                }
                break;
        }


        eWait.Processing = false;
    }



    //Label Event Implemention
    private void LabelEvent()
    {
        EventLabel eLabel = currentEvent as EventLabel;
        eLabel.Processing = true;
        foreach (var name in labels.Keys)
        {
            if (name == eLabel.labelName)
            {
                //Debug.LogWarning("The label: " + name + " has already existed!!!");
                eLabel.Processing = false;
                return;
            }
        }

        //Record the next event index
        labels.Add(eLabel.labelName, index);

        eLabel.Processing = false;
    }


    //Jump To Label Event Implemention
    private void JumpToLabelEvent()
    {
        EventJumpToLabel eJump = currentEvent as EventJumpToLabel;
        eJump.Processing = true;

        foreach (var p in labels)
        {
            if (p.Key == eJump.toLabelName)
            {
                index = p.Value;
                eJump.Processing = false;
                return;
            }
        }
        Debug.LogWarning("The label: " + eJump.toLabelName + " was not found!!");
        eJump.Processing = false;
    }
    #endregion

}

