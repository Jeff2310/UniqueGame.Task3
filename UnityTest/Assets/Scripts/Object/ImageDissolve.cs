using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
public class ImageDissolve : SerializedMonoBehaviour {

    [Range(0,1.2f)]
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
    public Image image;

    [HideInEditorMode]
    [ReadOnly]
    public bool dissolving = false;

    private void Awake()
    {

        image = GetComponent<Image>();
        mat = Resources.Load("Materials/Sprite_Dissolve_Mat") as Material;
        if (mat == null)
        {
            Debug.Log("Load mat failed");
        }
        image.material = new Material(mat);
    }

    private void Update()
    {
        image.material.SetFloat("_Factor", factor);
        image.material.SetFloat("_EdgeWidth", edgeWidth);
        image.material.SetColor("_EdgeColor", edgeColor);
        image.material.SetFloat("_Size", strength);
    }


    //Straightly Dissolve function
    public void Dissolve(int duration)
    {
        dissolving = true;
        StartCoroutine(StartDissolve(duration));
    }
    private IEnumerator StartDissolve(int duration)
    {
        for (int i = 0; i <= duration; i++)
        {
            factor = 0.55f / duration * i + 0.25f;
            yield return null;
        }
        dissolving = false;
    }
}
