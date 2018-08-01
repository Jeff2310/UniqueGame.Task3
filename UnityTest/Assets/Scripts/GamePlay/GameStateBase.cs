using UnityEngine;

using StateValue = System.Int32;

public abstract class GameStateBase : MonoBehaviour
{
    public string StateName;
    // 状态值为非负整数，-1代表未定义这种状态
    public int Value;
}