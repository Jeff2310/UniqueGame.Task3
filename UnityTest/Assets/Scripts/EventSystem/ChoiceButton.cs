using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceButton : MonoBehaviour
{
	public int index;
	private BranchConversation _conversation;

	private void Start()
	{
		_conversation = GameObject.Find("TestConversation").GetComponent<BranchConversation>();
	}

	public void Trigger()
	{
		_conversation.SelectChoice(index);
	} 
}
