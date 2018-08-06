using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePrepare : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject dialogCanvas = GameObject.Find("DialogCanvas");
        if (dialogCanvas == null)
        {
            Debug.LogWarning("Can NOT load the dialog canvas");
            return;
        }


        //The UI Canvas
        DontDestroyOnLoad(dialogCanvas);
        GameManager.Instance.ToTitle();
	}
	
}
