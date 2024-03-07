using System;
using System.Linq;
using CreativeVeinStudio.Simple_Dialogue_System.Attributes;
using CreativeVeinStudio.Simple_Dialogue_System.Enums;
using CreativeVeinStudio.Simple_Dialogue_System.Models;
using UnityEditor;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Editor
{
    [CustomPropertyDrawer(typeof(ToggleDropDownOptions))]
    public class ToggleDropDownPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // base.OnGUI(position, property, label);
            var defaultPos = position;
            // EditorGUI.LabelField(position, label);
            position.x = EditorGUIUtility.labelWidth - 10;
            position.width = 20;
            var propertyValue = property.FindPropertyRelative("value");

            if (GUI.Button(position, "G", new GUIStyle(EditorStyles.miniButtonLeft) { fixedWidth = position.width }))
            {
                property.FindPropertyRelative("varType").enumValueIndex =
                    (int)EVariableOptions.IsGlobal;

                // propertyValue.stringValue = property.FindPropertyRelative("isGlobal").boolValue
                //     ? "prev Node - RanQty"
                //     : "";
            }

            position = defaultPos;
            if (property.FindPropertyRelative("varType").enumValueIndex ==
                (int)EVariableOptions.IsGlobal)
            {
                if (attribute is ToggleDropDownOptions globalVarList)
                {
                    var list = globalVarList.listOne.Select(x => new GUIContent(x.varName)).ToArray();
                    if (propertyValue.propertyType == SerializedPropertyType.String)
                    {
                        int index = Mathf.Max(0,
                            Array.IndexOf(list,
                                list.FirstOrDefault(x =>
                                    x.text == propertyValue.stringValue)));
                        index = EditorGUI.Popup(position, new GUIContent(label), index, list);
                        propertyValue.stringValue = list[index].text;
                    }
                }
            }
            else
            {
                EditorGUI.PropertyField(position, propertyValue, label);
            }
        }
    }
}