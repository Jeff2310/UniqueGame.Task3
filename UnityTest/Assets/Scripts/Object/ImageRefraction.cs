using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
[RequireComponent(typeof(SpriteRenderer))]
public class ImageRefraction : SerializedMonoBehaviour {

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
    public Image image;

    private void Awake()
    {

        image = GetComponent<Image>();
        mat = Resources.Load("Materials/Sprite_Refraction_Mat") as Material;
        if (mat == null)
        {
            Debug.Log("Load mat failed");
        }
        image.material = new Material(mat);
    }

    private void Update()
    {
        if (distortionMap == null)
        {
            distortionMap = mat.GetTexture("_DistortionMap");
        }


        image.material.SetFloat("_Refraction", refraction);
        image.material.SetFloat("_DistortionPower", power);
        image.material.SetFloat("_AnimXSpeed", xSpeed);
        image.material.SetFloat("_AnimYSpeed", ySpeed);

        image.material.SetTexture("_DistortionMap", distortionMap);
    }
}
