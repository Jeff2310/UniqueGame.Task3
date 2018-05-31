using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNode : SceneNode {
	
	protected override void Init()
	{
		base.Init();
		type = NodeType.Character;
	}

	public override void OnArrival()
	{
        
	}

	public override void OnInspect()
	{
        
	}
}
