using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;

using UnityEngine;

#if UNITY_EDITOR

using Sirenix.Utilities.Editor;

#endif

public class Intepreter : SerializedMonoBehaviour
{

    [Title(title: "The Name of the Intepreter", subtitle: "Name the intepreter so it can be recognized easily.")]
    public string intepreterName = "New Intepreter";

    [Title(title: "Event Contents")]
    [OnInspectorGUI("CheckEventElements")]
    [ListDrawerSettings(OnBeginListElementGUI = "BeginDrawEventList", OnEndListElementGUI = "EndDrawEventList", ShowIndexLabels = true)]
    public List<EventBase> events = new List<EventBase>();

    public EventBase currentEvent { get; private set; }


    private int index = 0;
    private Dictionary<string, int> labels = new Dictionary<string, int>();

    //Editor modifications
#if UNITY_EDITOR
    private void CheckEventElements()
    {
        for (int i = 0; i < events.Count;)
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
        var e = events[index];
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
        else if (e is EventAudio)
        {
            GUIHelper.PushColor(new Color(0.7f, 0.7f, 0.2f));
        }
        else
        {
            GUIHelper.PushColor(Color.gray);
        }
        SirenixEditorGUI.BeginBox(events[index].GetLabel());
    }
    private void EndDrawEventList(int index)
    {
        if (events[index] == null)
        {
            return;
        }
        GUIHelper.PopColor();
        SirenixEditorGUI.EndBox();
    }
#endif
    //--------------------

    public void Trigger()
    {

        StopCoroutine(ExcuteEvents());
        if (!EventManager.Instance.currentInepreters.Contains(this))
            EventManager.Instance.currentInepreters.Add(this);
        StartCoroutine(ExcuteEvents());

    }
    private IEnumerator ExcuteEvents()
    {
        for (index = 0; index < events.Count; index++)
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
            if (e is EventInventory)
            {
                InventoryEvent();
            }
            if (e is EventSwitch)
            {
                SwitchEvent();
            }
            if (e is EventVariable)
            {
                VarEvent();
            }
            if (e is EventAudio)
            {
                AudioEvent();
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
        DialogUIRef.Instance.nameText.text = eDialog.characterName;
        DialogUIRef.Instance.dialogMessage.text = "";

        for (int i = 0; i < eDialog.message.Length; i++)
        {
            char letter = eDialog.message[i];

            DialogUIRef.Instance.dialogMessage.text += letter;

            //Fix the transmeaning pattern
            if (letter == '<')
            {
                int j = 1;
                while (eDialog.message[i + j] != '>')
                {
                    DialogUIRef.Instance.dialogMessage.text += eDialog.message[i + j];
                    j++;
                }
                DialogUIRef.Instance.dialogMessage.text += eDialog.message[i + j];
                i = i + j;
            }



            switch (letter)
            {
                case '\n':
                case '.':
                    yield return new WaitForSeconds(0.1f);
                    break;
                case ',':
                case ' ':
                    yield return new WaitForSeconds(0.05f);
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
            DialogUIRef.Instance.dialogBox.gameObject.GetComponent<Animator>().SetBool("Dialogging", true);
        }
    }
    private void DialogEndAnim()
    {
        if ((currentEvent as EventDialog).EndOfDialog)
        {
            DialogUIRef.Instance.dialogBox.gameObject.GetComponent<Animator>().SetBool("Dialogging", false);
        }
    }
    private void DialogHideContinue()
    {
        DialogUIRef.Instance.continueButton.gameObject.SetActive(false);
    }
    private void DialogShowContinue()
    {
        DialogUIRef.Instance.continueButton.gameObject.SetActive(true);
    }
    public void DialogSkip()
    {
        StopCoroutine(DialogTyping());
        DialogUIRef.Instance.dialogMessage.text = (currentEvent as EventDialog).message;
        DialogShowContinue();
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




    //Inventory Event Implemention
    private void InventoryEvent()
    {

        var e = currentEvent as EventInventory;
        if (e == null)
        {
            return;
        }
        switch (e.itemChangeType)
        {
            case EventInventory.ItemChangeType.Gain:

                break;
            case EventInventory.ItemChangeType.Lose:

                break;
            default:
                break;
        }
    }




    //Switch Event Implemention
    private void SwitchEvent()
    {
        EventSwitch eSwitch = currentEvent as EventSwitch;
        eSwitch.Processing = true;
        switch (eSwitch.operation)
        {
            case EventSwitch.SwitchOperation.SetToFalse:
                GameSwitches.Instance.SetSwitch(eSwitch.switchIndex, false);
                break;
            case EventSwitch.SwitchOperation.SetToTrue:
                GameSwitches.Instance.SetSwitch(eSwitch.switchIndex, true);
                break;
            default:
                break;
        }
        eSwitch.Processing = false;
    }


    //Var Event Implemention
    private void VarEvent()
    {
        EventVariable eVar = currentEvent as EventVariable;
        eVar.Processing = true;
        float v;

        switch (eVar.operation)
        {
            case EventVariable.VarOperation.EqualsTo:
                GameVariables.Instance.SetVar(eVar.varIndex, eVar.number);
                break;
            case EventVariable.VarOperation.Plus:
                v = GameVariables.Instance.GetVar(eVar.varIndex);
                v += eVar.number;
                GameVariables.Instance.SetVar(eVar.varIndex, v);
                break;
            case EventVariable.VarOperation.Minus:
                v = GameVariables.Instance.GetVar(eVar.varIndex);
                v -= eVar.number;
                GameVariables.Instance.SetVar(eVar.varIndex, v);
                break;
            case EventVariable.VarOperation.Multiply:
                v = GameVariables.Instance.GetVar(eVar.varIndex);
                v *= eVar.number;
                GameVariables.Instance.SetVar(eVar.varIndex, v);
                break;
            default:
                break;
        }
        eVar.Processing = false;
    }



    //Audio Event Implemention
    private void AudioEvent()
    {
        currentEvent.Processing = true;


        EventAudio eAudio = currentEvent as EventAudio;
        var audioType = eAudio.audioType;
        var audioName = eAudio.audioName;
        var bgmOperationType = eAudio.bgmOperationType;
        var duration = eAudio.duration;

        switch (audioType)
        {
            case EventAudio.AudioType.BGM:
                switch (bgmOperationType)
                {
                    case EventAudio.BGMOperationType.Play:
                        AudioManager.Instance.PlayBGM(audioName);
                        break;
                    case EventAudio.BGMOperationType.Stop:
                        AudioManager.Instance.StopBGM(audioName);
                        break;
                    case EventAudio.BGMOperationType.FadeIn:
                        AudioManager.Instance.FadeInBGM(audioName, duration);
                        break;
                    case EventAudio.BGMOperationType.FadeOut:
                        AudioManager.Instance.FadeOutBGM(audioName, duration);
                        break;
                    default:
                        Debug.LogWarning("AudioManager ERROR!!");
                        break;
                }
                break;
            case EventAudio.AudioType.SE:
                AudioManager.Instance.PlaySE(audioName);
                break;
            default:
                Debug.LogWarning("AudioManager ERROR!!");
                break;
        }

        currentEvent.Processing = false;
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



    //Custom Script Event Implemention
    private void CustomEvent()
    {
        EventCustom eCustom = currentEvent as EventCustom;
        AsyncCallback callback = new AsyncCallback(CustomAsyncResult);
        //parameters
        object o = new object();
        eCustom.func.BeginInvoke(callback, o);
    }
    private void CustomAsyncResult(IAsyncResult ar)
    {
        Debug.Log("Delegate Activated");
    }

    #endregion
}

