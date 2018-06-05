using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : Singleton<StateManager>
{
    private Dictionary<string, GameStateBase> _gameStateDB = new Dictionary<string, GameStateBase>();

    public int GetState(string stateName)
    {
        if (_gameStateDB.ContainsKey(stateName))
        {
            return _gameStateDB[stateName].Value;
        }
        else
        {
            // 状态值为非负整数，-1代表不存在
            Debug.Log(string.Format("Warning : Cannot Find Game State : [{0}]", stateName));
            return -1;
        }
    }

    public void SetState(string stateName, int value)
    {
        _gameStateDB[stateName].Value = value;
    }
}
