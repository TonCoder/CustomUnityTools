using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CreativeVeinStudio.Simple_Dialogue_System.Attributes;
using CreativeVeinStudio.Simple_Dialogue_System.Enums;
using CreativeVeinStudio.Simple_Dialogue_System.Models;
using UnityEditor;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Editor
{
    [CustomPropertyDrawer(typeof(ShowVarListAttribute))]
    public class ShowVarListPropertyDrawer : PropertyDrawer
    {
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        private bool gotValues = false;
        private List<VariablesModel> globals;
        private List<VariablesModel> locals;
        private string methodName;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!gotValues)
            {
                var obj = new SerializedObject(property.serializedObject.targetObject);
                var sD = (SO_Dialogue)obj.FindProperty("parentDialogue")?.objectReferenceValue;
                if (sD == null) return;
                globals = sD.GlobalVars.List;
                locals = sD.LocalVars;
            }

            gotValues = true;
            ShowGUI(position, property, label);
        }

        private void ShowGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var defaultPos = position;
            position.x = EditorGUIUtility.labelWidth - 10;
            position.width = 20;
            var propertyValue = property.FindPropertyRelative("value");

            var indx = property.FindPropertyRelative("varType").enumValueIndex;

            if (indx == (int)EVariableOptions.IsString && GUI.Button(position, "S",
                    new GUIStyle(EditorStyles.miniButtonLeft) { fixedWidth = position.width }))
            {
                property.FindPropertyRelative("varType").enumValueIndex =
                    (int)EVariableOptions.IsGlobal;
            }

            if (indx == (int)EVariableOptions.IsGlobal && GUI.Button(position, "G",
                    new GUIStyle(EditorStyles.miniButtonLeft) { fixedWidth = position.width }))
            {
                property.FindPropertyRelative("varType").enumValueIndex =
                    (int)EVariableOptions.IsLocal;
            }

            if (indx == (int)EVariableOptions.IsLocal && GUI.Button(position, "L",
                    new GUIStyle(EditorStyles.miniButtonLeft) { fixedWidth = position.width }))
            {
                property.FindPropertyRelative("varType").enumValueIndex =
                    (int)EVariableOptions.IsString;
            }


            position = defaultPos;


            if (property.FindPropertyRelative("varType").enumValueIndex == (int)EVariableOptions.IsLocal)
            {
                if (locals.Count > 0)
                {
                    var list = locals.Select(x => new GUIContent(x.varName)).ToArray();
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
                else
                {
                    EditorGUI.LabelField(position, "No values available");
                }
            }
            else if (property.FindPropertyRelative("varType").enumValueIndex == (int)EVariableOptions.IsGlobal)
            {
                if (globals.Count > 0)
                {
                    var list = globals.Select(x => new GUIContent(x.varName)).ToArray();
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
                else
                {
                    EditorGUI.LabelField(position, "No values available");
                }
            }
            else
            {
                EditorGUI.PropertyField(position, propertyValue, label);
            }
        }
    }
}