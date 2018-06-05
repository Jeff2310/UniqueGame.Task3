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
    [ListDrawerSettings(OnBeginListElementGUI ="BeginDrawEventList",OnEndListElementGUI = "EndDrawEventList")]
    public List<EventBase> events = new List<EventBase>();

    public EventBase currentEvent { get; private set; }

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

    public void Trigger()
    {
        StopCoroutine(ExcuteEvents());
        EventManager.Instance.currentInepreters.Add(this);
        StartCoroutine(ExcuteEvents());
        
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
            if (e is EventWait)
            {
                WaitEvent();
            }


            //Won't continue until this one is finished
            while (e.Processing)
            {
                yield return null;
            }

            currentEvent = null;
        }

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

}

