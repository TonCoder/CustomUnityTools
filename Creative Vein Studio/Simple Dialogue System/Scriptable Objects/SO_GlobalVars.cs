using System;
using System.Collections.Generic;
using System.Linq;
using CreativeVeinStudio.Simple_Dialogue_System.Interface;
using CreativeVeinStudio.Simple_Dialogue_System.Models;
using UnityEngine;
using UnityEngine.Serialization;

namespace CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects
{
    [System.Serializable]
    public class SO_GlobalVars : ScriptableObject, ICustomVarActions
    {
        [SerializeField] internal List<VariablesModel> globalVariables;

        public List<VariablesModel> List
        {
            get => globalVariables;
            set => globalVariables = value;
        }

        public VariablesModel GetValueByName(string val)
        {
            return globalVariables.Find(x => x.varName == val);
        }
    }

    [System.Serializable]
    public enum VarType
    {
        String,
        Float,
        Boolean
    }
}