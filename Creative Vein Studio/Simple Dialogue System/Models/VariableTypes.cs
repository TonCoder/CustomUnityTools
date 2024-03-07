using System;
using CreativeVeinStudio.Simple_Dialogue_System.Enums;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Models
{
    [Serializable]
    public class VariableTypes
    {
        [SerializeField] internal string value;
        [SerializeField] internal EVariableOptions varType;
    }

}