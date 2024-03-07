using System;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ShowVarListAttribute : PropertyAttribute
    {
        public string methodName;

        public ShowVarListAttribute()
        {
        }
    }
}