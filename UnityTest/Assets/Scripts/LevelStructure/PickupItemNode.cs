using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItemNode : SceneNode {
	protected override void Init()
	{
		base.Init();
		type = NodeType.PickupItem;
	}
	public override void OnArrival()
	{
        
	}

	public override void OnInspect()
	{
        
	}
}
