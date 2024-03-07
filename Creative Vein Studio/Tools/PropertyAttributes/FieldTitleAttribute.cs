using System;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public class FieldTitleAttribute : PropertyAttribute
    {
        public readonly string fieldTitle;

        /// <summary>
        /// FIELD NEEDS TO BE SERIALIZABLE FOR THIS TO WORK
        /// </summary>
        /// <param name="value"></param>
        public FieldTitleAttribute(string value)
        {
            fieldTitle = value;
        }
    }
}