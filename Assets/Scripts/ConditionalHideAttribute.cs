using System;
using System.Collections;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
    AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class ConditionalHideAttribute : PropertyAttribute
{
    //The name of what we are controlling
    public string ConditionalSourceField = "";
    //Will we hide this field
    public bool HideInInspector = false;

    public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector = false)
    {
        //This class                What was passed
        ConditionalSourceField = conditionalSourceField;
        HideInInspector = hideInInspector;
    }
}
