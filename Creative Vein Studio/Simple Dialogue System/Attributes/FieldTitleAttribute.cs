using System;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public class FieldTitleAttribute : PropertyAttribute
    {
        public readonly string title;

        public FieldTitleAttribute(string value)
        {
            title = value;
        }
    }
}