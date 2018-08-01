using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

using UnityEngine;

#if UNITY_EDITOR

using Sirenix.Utilities.Editor;

#endif

public class Interpreter : SerializedMonoBehaviour {

    [Title(title: "Conditions")]
    public List<bool> conditions = new List<bool>();
    
    [Title(title:"Event Contents")]
    [ListDrawerSettings(OnBeginListElementGUI ="BeginDrawEventList",OnEndListElementGUI = "EndDrawEventList")]
    public List<EventBase> events = new List<EventBase>();

    public EventBase currentEvent { get; private set; }
    public bool Running;

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

    public void Awake()
    {
        EventManager.Instance.Interpreter.Add(this);
        Running = false;
    }

    public void Trigger()
    {
        if (Running)
        {
            ForceStop();
        }
        EventManager.Instance.CurrentInterpreter = this;
        StartCoroutine(ExcuteEvents());
        Running = true;
    }

    public void ForceStop()
    {
        StopAllCoroutines();
        EventManager.Instance.CurrentInterpreter = null;
    }
    
    private IEnumerator ExcuteEvents()
    {
        foreach (var e in events)
        {
            if (e == null)
            {
                continue;
            }

            currentEvent = e;

            if (e is EventDialog)
            {
                DialogEvent();
            }
            else if (e is EventWait)
            {
                WaitEvent();
            }
            else if (e is EventChoice)
            {
                SelectEvent();
            }


            //Won't continue until this one is finished
            while (e.Processing)
            {
                yield return null;
            }

            currentEvent = null;
        }

        Running = false;
        EventManager.Instance.CurrentInterpreter = null;
    }

    #region All the functionalities to implement
    //Dialog Event Implemention
    private void DialogEvent()
    {
        EventDialog eDialog = currentEvent as EventDialog;
        eDialog.Processing = true;
        DialogShowMessage();
        DialogShowName();
        StopCoroutine("DialogTyping");
        StartCoroutine(DialogTyping());
    }
    private IEnumerator DialogTyping()
    {
        EventDialog eDialog = currentEvent as EventDialog;
        DialogHideContinueButton();
        DialogStartAnim();
        UIManager.Instance.nameText.text = eDialog.characterName;
        UIManager.Instance.dialogMessage.text = "";

        foreach (var letter in eDialog.message)
        {
            UIManager.Instance.dialogMessage.text += letter;
            yield return null;
        }

        DialogShowContinueButton();
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
    private void DialogHideContinueButton()
    {
        UIManager.Instance.continueButton.gameObject.SetActive(false);
    }
    private void DialogShowContinueButton()
    {
        UIManager.Instance.continueButton.gameObject.SetActive(true);
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

    #endregion
    
    // Select Event Implemention

    private void DialogHideMessage()
    {
        UIManager.Instance.dialogMessage.gameObject.SetActive(false);
    }
    private void DialogShowMessage()
    {
        UIManager.Instance.dialogMessage.gameObject.SetActive(true);
    }
    private void DialogHideName()
    {
        UIManager.Instance.nameBox.gameObject.SetActive(false);
    }
    private void DialogShowName()
    {
        UIManager.Instance.nameBox.gameObject.SetActive(true);
    }
    
    private void SelectEvent()
    {
        EventChoice eSelect = currentEvent as EventChoice;
        eSelect.Processing = true;
        DialogHideMessage();
        DialogHideContinueButton();
        DialogHideName();
        eSelect.ConversationScript.Enable();
        StartCoroutine(DetectSelectEnding());
    }

    private IEnumerator DetectSelectEnding()
    {
        EventChoice eSelect = currentEvent as EventChoice;
        while (!eSelect.ConversationScript.Finish)
        {
            yield return null;
        }
        eSelect.Processing = false;
        eSelect.ConversationScript.Disable();
        EventManager.Instance.CurrentInterpreter = this;
        DialogShowMessage();
    }
}

