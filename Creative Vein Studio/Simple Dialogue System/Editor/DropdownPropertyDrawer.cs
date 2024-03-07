using System;
using System.Linq;
using CreativeVeinStudio.Simple_Dialogue_System.Attributes;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace CreativeVeinStudio.Simple_Dialogue_System.Editor
{
    [CustomPropertyDrawer(typeof(MakeDropDownAttribute))]
    public class DropdownPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (attribute is MakeDropDownAttribute globalVarList)
            {
                var list = globalVarList.variableList.Select(x => x.varName).ToArray();
                if (property.propertyType == SerializedPropertyType.String)
                {
                    int index = Mathf.Max(0, Array.IndexOf(list, property.stringValue));
                    index = EditorGUI.Popup(position, property.displayName, index, list);

                    property.stringValue = list[index];
                }
                else if (property.propertyType == SerializedPropertyType.Integer)
                {
                    property.intValue = EditorGUI.Popup(position, property.displayName, property.intValue, list);
                }
            }
        }
    }
}
#endif