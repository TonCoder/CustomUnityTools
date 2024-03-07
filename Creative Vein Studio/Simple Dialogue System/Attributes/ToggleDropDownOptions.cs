using System;
using System.Collections.Generic;
using CreativeVeinStudio.Simple_Dialogue_System.Models;
using CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CreativeVeinStudio.Simple_Dialogue_System.Attributes
{
    public class ToggleDropDownOptions : PropertyAttribute
    {
        public readonly List<VariablesModel> listOne;
        public readonly List<VariablesModel> listTwo;

        public ToggleDropDownOptions(Type type, string methodName)
        {
            var method = type.GetMethod(methodName);
            if (method != null)
            {
                listOne = method.Invoke(null, null) as List<VariablesModel>;
            }
            else
            {
                Debug.LogError("NO SUCH METHOD " + methodName + " FOR " + type);
            }
        }
    }
}