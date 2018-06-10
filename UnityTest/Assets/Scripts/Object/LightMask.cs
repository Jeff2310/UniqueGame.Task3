using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Sirenix.OdinInspector;
public class LightMask : SerializedMonoBehaviour {
    public class Lantern
    {
        public Texture lightTex;
    }

    public List<Lantern> lanterns = new List<Lantern>();

    private void Awake()
    {
        
    }
}
