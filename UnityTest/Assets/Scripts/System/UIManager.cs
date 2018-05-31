using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager : Singleton<UIManager> {

    public CanvasRenderer dialogBox;
    public TextMeshProUGUI dialogMessage;
    public TextMeshProUGUI nameText;
    public Button continueButton;
}
