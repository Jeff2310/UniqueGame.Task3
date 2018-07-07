using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTitle : MonoBehaviour {
	public void ClickStart()
    {
        //Start Funcs
        GameManager.Instance.ToGameScene("Prelogue Scene");
    }
}
