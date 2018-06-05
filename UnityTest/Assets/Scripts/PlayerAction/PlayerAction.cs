using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerActionConditionBase : MonoBehaviour
{
    public abstract bool Satisfied();
}

// 一个抽象的玩家行为
// 比如说，玩家1.在某一个事件线 2.需要用XXX 3.去打开某个东西 分别对应RequiredState, Condition, Action
public abstract class PlayerActionBase : MonoBehaviour
{
    public Dictionary<GameStateBase, int> RequiredStates = new Dictionary<GameStateBase, int>();
    public List<PlayerActionConditionBase> Conditions = new List<PlayerActionConditionBase>();
    public List<PlayerBehaviour> Actions = new List<PlayerBehaviour>();

    public enum ActionResult
    {
        Success, Objected
    }
    public ActionResult Act()
    {
        foreach (var state in RequiredStates.Keys)
        {
            if (StateManager.Instance.GetState(state.StateName) != RequiredStates[state])
                return ActionResult.Objected;
        }
        foreach (var condition in Conditions)
        {
            if (!condition.Satisfied()) return ActionResult.Objected;
        }

        foreach (var action in Actions)
        {
            action.Act();
        }
        return ActionResult.Success;
    }
}