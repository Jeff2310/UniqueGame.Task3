using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotspotNode : SceneNode {
    
    protected override void Init()
    {
        base.Init();
        type = NodeType.HotSpot;
    }
    public override void OnArrival()
    {
        
    }

    public override void OnInspect()
    {
        
    }
}
