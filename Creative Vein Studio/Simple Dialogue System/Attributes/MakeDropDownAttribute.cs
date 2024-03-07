using System;
using System.Collections.Generic;
using System.Linq;
using CreativeVeinStudio.Simple_Dialogue_System.Models;
using CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Attributes
{
    public class MakeDropDownAttribute : PropertyAttribute
    {
        public readonly List<VariablesModel> variableList;

        public MakeDropDownAttribute(Type type, string methodName)
        {
            // get the type that was sent, ex the name of the class
            var method = type.GetMethod(methodName);
            if (method != null)
            {
                variableList = method.Invoke(null, null) as List<VariablesModel>;
            }
            else
            {
                Debug.LogError("NO SUCH METHOD " + methodName + " FOR " + type);
            }
        }
    }
}