using System;
using UnityEngine;

namespace _MainApp.Scripts.Tools.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Interface, Inherited = true, AllowMultiple = true)]
    public class SerializeInterfaceAttribute : PropertyAttribute
    {
        // public SerializedProperty obj;
        public SerializeInterfaceAttribute()
        {
            Debug.Log("TEST");
        }
    }
}