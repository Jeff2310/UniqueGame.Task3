using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Sirenix.OdinInspector;

public class Choice
{
	public enum ChoiceType
	{
		Single, Continous, Expanding
	}

	public ChoiceType Type = ChoiceType.Single;
	public string Text;
	public Choice ParentChoice;
	public int Index;

	public Choice()
	{
		Type = ChoiceType.Single;
		Text = ""; 
	}
	public Choice(string text)
	{
		Text = text;
	}

	public void Select(BranchConversation conversation)
	{
		Debug.Log(Index);
		OnSelect(conversation);
	}

	protected virtual void OnSelect(BranchConversation conversation)
	{
		
	}
}

public class ContinousChoice : Choice
{
	public Choice NextChoice;
	public ContinousChoice() : base()
	{
		Type = ChoiceType.Continous;
	}

	protected override void OnSelect(BranchConversation conversation)
	{
	}
}

public class ExpandingChoice : Choice
{
	public Choice[] SubChoices;
	public ExpandingChoice() : base()
	{
		Type = ChoiceType.Expanding;
	}
	
	protected override void OnSelect(BranchConversation conversation)
	{

	}
}

public class BranchConversation : SerializedMonoBehaviour
{
	public CanvasRenderer ConversationBox;
	private RectTransform _ConversationBoxRect;
	private TextMeshProUGUI[] ChoiceTexts;

	public List<Choice> ChoiceDB = new List<Choice>();
	public List<Choice> CurrentChoices = new List<Choice>();

	private bool _changed = true;
	
	// Use this for initialization
	void Start ()
	{
		if (ConversationBox != null)
		{
			_ConversationBoxRect = ConversationBox.gameObject.GetComponent<RectTransform>();
		}
		ChoiceTexts = new TextMeshProUGUI[5];
		var root = new ExpandingChoice();
		root.SubChoices = new Choice[ChoiceDB.Count];
		ChoiceDB.Insert(0, root);
		for (int i = 1; i <= 4; i++)
		{
			(ChoiceDB[0] as ExpandingChoice).SubChoices[i-1] = ChoiceDB[i];
			ChoiceDB[i].ParentChoice = ChoiceDB[0];
			CurrentChoices.Add(ChoiceDB[i]);
		}
		for (int i = 5; i <= 7; i++)
		{
			(ChoiceDB[4] as ExpandingChoice).SubChoices[i-5] = ChoiceDB[i];
			ChoiceDB[i].ParentChoice = ChoiceDB[4];
		}

		for (int i = 0; i < 5; i++)
		{
			ChoiceTexts[i] = GameObject.Find("ChoiceText" + i.ToString()).GetComponent<TextMeshProUGUI>();
			ChoiceTexts[i].gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
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
			UpdateChoice(i, choice.SubChoices[i]);
		}
		_changed = true;
	}

	public void CollapseChoice(int index)
	{
		Choice choice = CurrentChoices[index];
		CurrentChoices.Clear();
		if (!(choice.ParentChoice.ParentChoice is ExpandingChoice))
		{
			Debug.Log("Wrong Model: No GrandParent!");
			throw new Exception();
		}
		// 返回到父节点，并恢复之前各项选择
		// 也就是获取父节点的姊妹
		ExpandingChoice grandParentChoice = (ExpandingChoice)choice.ParentChoice.ParentChoice;
		for (int i = 0; i < grandParentChoice.SubChoices.Length; i++)
		{
			UpdateChoice(i, grandParentChoice.SubChoices[i]);
		}
		FinishChoice(index);
		_changed = true;
	}

	public void ContinueChoice(int index)
	{
		ContinousChoice choice = (ContinousChoice)CurrentChoices[index];
		choice.NextChoice.Index = choice.Index;
		if (choice.ParentChoice is ExpandingChoice)
		{
			// 在选择树上删去这个节点
			choice.NextChoice.ParentChoice = choice.ParentChoice;
			(choice.ParentChoice as ExpandingChoice).SubChoices[choice.Index] = choice.NextChoice;
		}
		UpdateChoice(choice.Index, choice.NextChoice);
		_changed = true;
	}

	public void FinishChoice(int index)
	{
		CurrentChoices.RemoveAt(index);
		for (int i =  index; i < CurrentChoices.Count; i++)
		{
			CurrentChoices[i].Index--;
			if (CurrentChoices[i].Type == Choice.ChoiceType.Continous)
			{
				Choice next = CurrentChoices[i];
				do
				{
					next = (next as ContinousChoice).NextChoice;
					next.Index = CurrentChoices[i].Index;
				}while (next.Type == Choice.ChoiceType.Continous);
			}
		}
		_changed = true;
	}

	public void SelectChoice(int index)
	{
		CurrentChoices[index].Select(this);
		if (CurrentChoices[index] is ContinousChoice)
		{
			ContinueChoice(index);
		}
		else if (CurrentChoices[index] is ExpandingChoice)
		{
			ExpandChoice(index);
		}
		else
		{
			FinishChoice(index);
		}
	}
}
