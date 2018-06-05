using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour {
	private SpriteRenderer _sprite;
	public enum CharacterState
	{
		Idle, Moving, Running
	}
	public CharacterState State;

	public bool FlipX;

	// Use this for initialization
	void Start ()
	{
		State = CharacterState.Idle;
		FlipX =  false;
		_sprite = GetComponentInChildren<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		_sprite.flipX = FlipX;
	}

	public void CharacterIdle()
	{
		State = CharacterState.Idle;
	}

	public void CharacterMove()
	{
		State = CharacterState.Moving;
	}
}
