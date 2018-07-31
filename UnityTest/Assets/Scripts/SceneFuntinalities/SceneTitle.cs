using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Light2D;
using UnityEngine.UI;
public class SceneTitle : MonoBehaviour {
	public void ClickStart()
    {
        StartCoroutine(StartClickStart());
    }
    private IEnumerator StartClickStart()
    {
        var newspaper = GameObject.Find("Newspaper").GetComponent<ImageDissolve>();

        newspaper.gameObject.GetComponent<Button>().interactable = false;

        LightSprite ambL = GameObject.Find("Light ambient").GetComponent<LightSprite>();
        LightSprite topL = GameObject.Find("Light top").GetComponent<LightSprite>();
        //do 180 frames
        for (int i = 0; i <= 180; i++)
        {
            ambL.Color.a = (180f/255f) / 180 * (180 - i);
            topL.Color.a = 1f / 180f * (180-i);
            yield return null;
        }

        
        //dissolve 120 frames
        newspaper.Dissolve(240);

        while (newspaper.dissolving)
        {
            yield return null;
        }

        LightSprite blueL = GameObject.Find("Light blue").GetComponent<LightSprite>();
        LightSprite scrollL = GameObject.Find("Light scroll").GetComponent<LightSprite>();
        //do 180 frames
        for (int i = 0; i <= 180; i++)
        {
            blueL.Color.a = (200f/255f) / 180 * (180 - i);
            scrollL.Color.a = 1f / 180 * (180 - i);
            yield return null;
        }

        //wait 120 frames
        for (int i = 0; i <= 120; i++)
        {
            yield return null;
        }



        GameManager.Instance.ToGameScene("Prelogue Scene");
    }
}
