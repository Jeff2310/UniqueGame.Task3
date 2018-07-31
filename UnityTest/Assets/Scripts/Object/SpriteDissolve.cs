using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class SpriteDissolve : SerializedMonoBehaviour {

    [Range(0,1.5f)]
    public float factor = 0f;
    [Range(0,10f)]
    public float strength = 5f;
    public Color edgeColor = Color.white;
    [Range(0,0.1f)]
    public float edgeWidth = 0.01f;

    public Texture2D NoiseTex;

    [HideInEditorMode]
    public Material mat;
    [HideInEditorMode]
    public SpriteRenderer spr;

    private void Awake()
    {

        spr = GetComponent<SpriteRenderer>();
        mat = Resources.Load("Materials/Sprite_Dissolve_Mat") as Material;
        if (mat == null)
        {
            Debug.Log("Load mat failed");
        }
        spr.material = new Material(mat);
    }

    private void Update()
    {
        spr.material.SetFloat("_Factor", factor);
        spr.material.SetFloat("_EdgeWidth", edgeWidth);
        spr.material.SetColor("_EdgeColor", edgeColor);
        spr.material.SetFloat("_Size", strength);
    }
}
