using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GloomEffect : SerializedMonoBehaviour {

    public Light2D.LightSprite lightSpr;

    public float baseValue = 1f;
    public float a = 0.5f;
    public float phrase = 0f;
    public float speed = 1f;

    private Vector3 oriScale;
    

    private void Start()
    {
        oriScale = transform.localScale;
    }

    // Update is called once per frame
    void Update () {
        float k = (Mathf.Sin(Time.time*speed+phrase)+1f)*a/2+baseValue;
        transform.localScale = oriScale*k;

        if (lightSpr != null)
        {

        }
	}
}
