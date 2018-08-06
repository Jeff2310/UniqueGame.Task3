using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif
[RequireComponent(typeof(Intepreter))]
public class Character : SerializedMonoBehaviour
{
   
    public class EventPage
    {
        [OnInspectorGUI("DrawIntepreterName")]
        public Intepreter intepreter;
        public enum TriggerType { PressConfirm, Collide, Auto }
        [EnumToggleButtons]
        public TriggerType triggerType;
        public List<Condition> conditions = new List<Condition>();

#if UNITY_EDITOR
        private void DrawIntepreterName()
        {
            string iName;
            if (intepreter == null)
            {
                iName = "The intepreter haven't setup!";
            }
            else
            {
                iName = intepreter.intepreterName;
            }
            SirenixEditorGUI.Title(iName, "", TextAlignment.Center, true);
        }
#endif
    }
    

    //Index of active intepreter
    public int pageIndex = 0;

    [ListDrawerSettings(ShowIndexLabels = true)]
    public List<EventPage> pages = new List<EventPage>();


    public EventPage page
    {
        get
        {
            return pages[pageIndex];
        }
    }

    private void Update()
    {
        //Auto Trigger
        if (page == null)
        {
            Debug.LogWarning("Can not find the page of " + pageIndex + "!");
            return;
        }
        if (page.triggerType == EventPage.TriggerType.Auto)
        {
            ExectuePage();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Collide
        var o = collision.gameObject;
        if (o.tag == "Player")
        {
            if (page == null)
            {
                Debug.LogWarning("Can not find the page of " + pageIndex + "!");
                return;
            }
            if (Input.GetButtonDown("Submit"))
            {
                if (page.triggerType == EventPage.TriggerType.Auto)
                {
                    ExectuePage();
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Press Confirm
        if (Input.GetButtonDown("Submit"))
        {
            var o = collision.gameObject;
            if (o.tag == "Player")
            {
                if (page == null)
                {
                    Debug.LogWarning("Can not find the page of " + pageIndex + "!");
                    return;
                }

                if (page.triggerType == EventPage.TriggerType.Auto)
                {
                    ExectuePage();
                }
            }
        }
    }

    private void ExectuePage()
    {
        if (page == null)
        {
            Debug.LogWarning("Can not find the page of " + pageIndex + "!");
            return;
        }

        foreach (var c in page.conditions)
        {
            if (!c.Check())
            {
                return;
            }
        }

        //Trigger the intepreter
        page.intepreter.Trigger();
    }
}
