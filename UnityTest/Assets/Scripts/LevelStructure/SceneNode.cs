using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneNode : MonoBehaviour
{
	public Transform ArriveTransform;
	public Transform NodePosition;
	[HideInInspector] 
	public Vector3 ArriveLocation;
	[HideInInspector] 
	public Vector3 Position;

	public enum NodeType
	{
		Character,
		PickupItem,
		HotSpot
	};

	protected NodeType type;
	public NodeType Type
	{
		get { return type; }
	}

	void Start ()
	{
		Init();
	}
	
	void Update () {
		
	}

	protected virtual void Init()
	{
		ArriveLocation = ArriveTransform.position;
	}
	public abstract void OnArrival();
	public abstract void OnInspect();
}
