using UnityEngine;
using UnityEditor;

public class ContinousChoice : Choice
{
    public Choice NextChoice;
    public ContinousChoice() : base()
    {
        Type = ChoiceType.Continous;
    }
}

public class ExpandingChoice : Choice
{
    public Choice[] SubChoices;
    public ExpandingChoice() : base()
    {
        Type = ChoiceType.Expanding;
    }
}

public class CollapseChoice : Choice
{
    public CollapseChoice() : base()
    {
        Type = ChoiceType.Collapse;
    }
}