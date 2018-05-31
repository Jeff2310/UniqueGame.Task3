using UnityEngine;
using System.Collections;

[System.Serializable]
public class VariableCondition
{
    public enum CompareType { Equal,MoreThan,LessThan,MoreEqual,LessEqual}

    public int varIndex;
    public float value;
    public CompareType compareType;

    public bool Condition()
    {
        float varValue = GameVariables.Instance.GetVar(varIndex);
        switch (compareType)
        {
            case CompareType.Equal:
                if (value==varValue)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case CompareType.MoreThan:
                if (varValue>value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case CompareType.LessThan:
                if (varValue<value)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            case CompareType.MoreEqual:
                if (varValue>=value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            case CompareType.LessEqual:
                if (varValue<=value)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            default:
                return false;
        }
    }
}

