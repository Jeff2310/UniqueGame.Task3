using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class BranchConversation : SerializedMonoBehaviour
{
	public CanvasRenderer ConversationBox;
	private RectTransform _ConversationBoxRect;
	private TextMeshProUGUI[] ChoiceTexts;

	public List<Choice> ChoiceDB = new List<Choice>();
	public List<Choice> CurrentChoices = new List<Choice>();

	public bool Enabled;
	public bool Finish;
	private bool _changed = true;
	private Interpreter _currentInterpreter;
	
	// Use this for initialization
	void Start ()
	{
		if (ConversationBox != null)
		{
            //_ConversationBoxRect = UIManager.Instance.dialogBox.gameObject.GetComponent<RectTransform>();
            _ConversationBoxRect = ConversationBox.gameObject.GetComponent<RectTransform>();
        }
		if(ChoiceTexts==null)
			ChoiceTexts= new TextMeshProUGUI[5];
		for (int i = 0; i < 5; i++)
		{
			ChoiceTexts[i] = GameObject.Find("ChoiceText" + i.ToString()).GetComponent<TextMeshProUGUI>();
			ChoiceTexts[i].gameObject.SetActive(false);
		}
		var root = new ExpandingChoice();
		root.SubChoices = new Choice[5];
		ChoiceDB.Insert(0, root);
		for (int i = 1; i < ChoiceDB.Count; i++)
		{
			ChoiceDB[i].IndexInParent = ChoiceDB[i].IndexInCurrent;
		}
		for (int i = 1; i <= 5; i++)
		{
			(ChoiceDB[0] as ExpandingChoice).SubChoices[i-1] = ChoiceDB[i];
			ChoiceDB[i].ParentChoice = ChoiceDB[0];
		}
		(ChoiceDB[5] as ExpandingChoice).SubChoices = new Choice[4];
		for (int i = 6; i <= 9; i++)
		{
			(ChoiceDB[5] as ExpandingChoice).SubChoices[i-6] = ChoiceDB[i];
			ChoiceDB[i].ParentChoice = ChoiceDB[5];
		}
		(ChoiceDB[4] as ContinousChoice).NextChoice = ChoiceDB[10];
		ChoiceDB[10].ParentChoice = ChoiceDB[4];
		(ChoiceDB[10] as ContinousChoice).NextChoice = ChoiceDB[11];
		ChoiceDB[11].ParentChoice = ChoiceDB[10];
		for (int i = 1; i <= 5; i++)
		{
			//if (ChoiceDB[i].Finished) continue;
			CurrentChoices.Add(ChoiceDB[i]);
		}

		Finish = false;
		Disable();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!Enabled) return;
		float lineHeight = 20.0f;
		if (!_changed) return;
		Vector2 newSize = _ConversationBoxRect.rect.size;
		newSize.y = CurrentChoices.Count * lineHeight + 40.0f;
		_ConversationBoxRect.sizeDelta = newSize;
		
		for (int i = 0; i < CurrentChoices.Count; i++)
		{
			ChoiceTexts[i].text = CurrentChoices[i].Text;
			ChoiceTexts[i].gameObject.SetActive(true);
		}

		for (int i = CurrentChoices.Count; i < 5; i++)
		{
			ChoiceTexts[i].gameObject.SetActive(false);
		}
		_changed = false;
	}

	private void UpdateChoice(int index, Choice choice)
	{
		CurrentChoices[index] = choice;
	}

	public void ExpandChoice(int index)
	{
		ExpandingChoice choice = (ExpandingChoice)CurrentChoices[index];
		CurrentChoices.Clear();
		for (int i = 0; i < choice.SubChoices.Length; i++)
		{
			CurrentChoices.Add(choice.SubChoices[i]);
		}
		_changed = true;
	}

	public void CollapseChoice(int index)
	{
		Choice choice = CurrentChoices[index];
		CurrentChoices.Clear();
		_changed = true;
		if (!(choice.ParentChoice.ParentChoice is ExpandingChoice))
		{
			// 到根节点了
			Disable();
			Finish = true;
			return;
			// throw new Exception();
		}
		// 返回到父节点，并恢复之前各项选择
		// 也就是获取父节点的姊妹
		ExpandingChoice grandParentChoice = (ExpandingChoice)choice.ParentChoice.ParentChoice;
		for (int i = 0; i < grandParentChoice.SubChoices.Length; i++)
		{
			if (grandParentChoice.SubChoices[i].Finished) continue;
			CurrentChoices.Add(grandParentChoice.SubChoices[i]);
			//UpdateChoice(i, grandParentChoice.SubChoices[i]);
		}
		FinishChoice(choice.ParentChoice.IndexInCurrent);
		
	}

	public void ContinueChoice(int index)
	{
		ContinousChoice choice = (ContinousChoice)CurrentChoices[index];
		choice.NextChoice.IndexInCurrent = choice.IndexInCurrent;
		if (choice.ParentChoice is ExpandingChoice)
		{
			// 在选择树上删去这个节点
			choice.NextChoice.ParentChoice = choice.ParentChoice;
			(choice.ParentChoice as ExpandingChoice).SubChoices[choice.IndexInParent] = choice.NextChoice;
		}
		UpdateChoice(choice.IndexInCurrent, choice.NextChoice);
		_changed = true;
	}

	public void FinishChoice(int index)
	{
		CurrentChoices[index].Finished = true;
		CurrentChoices.RemoveAt(index);
		for (int i =  index; i < CurrentChoices.Count; i++)
		{
			CurrentChoices[i].IndexInCurrent--;
			if (CurrentChoices[i].Type == Choice.ChoiceType.Continous)
			{
				Choice next = CurrentChoices[i];
				do
				{
					next = (next as ContinousChoice).NextChoice;
					next.IndexInCurrent = CurrentChoices[i].IndexInCurrent;
				}while (next.Type == Choice.ChoiceType.Continous);
			}
		}
		_changed = true;
	}

	public void SelectChoice(int index)
	{
		CurrentChoices[index].Select(this);
		_currentInterpreter = CurrentChoices[index].Detail;
		var temp = EventManager.Instance.CurrentInterpreter;
		if (_currentInterpreter != null)
		{
			EventManager.Instance.CurrentInterpreter = _currentInterpreter;
		}

		switch (CurrentChoices[index].Type)
		{
			case Choice.ChoiceType.Continous:
				ContinueChoice(index);
				break;
			case Choice.ChoiceType.Expanding:
				ExpandChoice(index);
				break;
			case Choice.ChoiceType.Collapse:
				CollapseChoice(index);
				break;
			case Choice.ChoiceType.Single:
				FinishChoice(index);
				break;
			default:
				break;
		}
		Disable();
		StartCoroutine(DetectChioceDetailEnd());
	}

	public void Enable()
	{
		Enabled = true;
	}

	public void Disable()
	{
		for (int i = 0; i < 5; i++)
		{
			ChoiceTexts[i].gameObject.SetActive(false);
		}
		Enabled = false;
	}

	private IEnumerator DetectChioceDetailEnd()
	{
		while (_currentInterpreter != null && _currentInterpreter.Running)
		{
			yield return null;
		}
		Enable();
		UIManager.Instance.dialogMessage.gameObject.SetActive(false);
		UIManager.Instance.continueButton.gameObject.SetActive(false);
		UIManager.Instance.nameBox.gameObject.SetActive(false);
		_currentInterpreter = null;
	}
}