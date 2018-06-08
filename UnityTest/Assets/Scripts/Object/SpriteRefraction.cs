using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRefraction : SerializedMonoBehaviour {

    [Range(-1f,1f)]
    public float refraction = 0.5f;
    [Range(-1f, 1f)]
    public float power = 0.2f;

    [Range(-1f, 1f)]
    public float xSpeed = 0.05f;
    [Range(-1f, 1f)]
    public float ySpeed = 0.05f;

    [InfoBox("If none,this will be set to the default distortion map.")]
    public Texture distortionMap;

    [HideInEditorMode]
    public Material mat;
    [HideInEditorMode]
    public SpriteRenderer spr;

    private void Awake()
    {

        spr = GetComponent<SpriteRenderer>();
        mat = Resources.Load("Materials/Sprite_Refraction_Mat") as Material;
        if (mat == null)
        {
            Debug.Log("Load mat failed");
        }
        spr.material = new Material(mat);
    }

    private void Update()
    {
        if (distortionMap == null)
        {
            distortionMap = mat.GetTexture("_DistortionMap");
        }


        spr.material.SetFloat("_Refraction", refraction);
        spr.material.SetFloat("_DistortionPower", power);
        spr.material.SetFloat("_AnimXSpeed", xSpeed);
        spr.material.SetFloat("_AnimYSpeed", ySpeed);

        spr.material.SetTexture("_DistortionMap", distortionMap);
    }
}
