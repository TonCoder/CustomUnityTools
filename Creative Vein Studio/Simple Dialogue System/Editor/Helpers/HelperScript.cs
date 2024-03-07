using System;
using System.Collections.Generic;
using CreativeVeinStudio.Simple_Dialogue_System.Enums;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Editor.Helpers
{
    public class EditorExtend : UnityEditor.Editor
    {
        public static void ReorderableLst(out ReorderableList list, List<string> objectList, bool draggable = true,
            bool displayHeader = true, bool displayAddBtn = true, bool displayRemoveBtn = true, string label = "")
        {
            EditorGUILayout.Space();
            list = new ReorderableList(objectList, typeof(string), draggable, displayHeader, displayAddBtn,
                displayRemoveBtn)
            {
                drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, label),
                drawElementCallback = (rect, index, active, focused) =>
                {
                    rect.y += 2;
                    rect.height = EditorGUIUtility.singleLineHeight;

                    // Use index to enumerate the serializefield
                    var label = new GUIContent($"{index}:");

                    // draw the field with the index label
                    // EditorGUI.PropertyField(rect, list.serializedProperty?.GetArrayElementAtIndex(index), label);

                    var temp = EditorGUIUtility.labelWidth;
                    EditorGUIUtility.labelWidth = 30;
                    objectList[index] = EditorGUI.TextField(rect, label, objectList[index]);
                    EditorGUIUtility.labelWidth = temp;

                    // node.Choices[index] = EditorGUI.TextField(rect, label, node.Choices[index]);
                }
            };
        }


        public static ReorderableList ReorderableLst(SerializedObject serObj, string propertyName,
            bool draggable = true,
            bool displayHeader = true, bool displayAddBtn = true, bool displayRemoveBtn = true, string label = "")
        {
            var vars = serObj.FindProperty(propertyName);
            var lst = new ReorderableList(serObj, vars, draggable, displayHeader,
                displayAddBtn, displayRemoveBtn);
            lst.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, label);
            lst.drawElementCallback = (rect, index, active, focused) =>
            {
                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;
                var itm = vars.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, itm, true);
            };
            lst.elementHeightCallback = delegate(int index)
            {
                var element = lst.serializedProperty.GetArrayElementAtIndex(index);
                var elementHeight = EditorGUI.GetPropertyHeight(element);
                // optional, depending on the situation in question and the defaults you like
                // you may want to subtract the margin out in the drawElementCallback before drawing
                var margin = EditorGUIUtility.standardVerticalSpacing;
                return elementHeight + margin;
            };

            return lst;
        }

        public static void VerticalSection<T>([CanBeNull] GUIStyle style, T node, Action<T> content,
            [CanBeNull] params GUILayoutOption[] options)
            where T : class
        {
            if (style != null) EditorGUILayout.BeginVertical(style, options);
            else EditorGUILayout.BeginHorizontal(options);
            content.Invoke(node);
            EditorGUILayout.EndVertical();
        }

        public static void VerticalSection([CanBeNull] GUIStyle style,
            Action content, [CanBeNull] params GUILayoutOption[] options)
        {
            if (style != null) EditorGUILayout.BeginVertical(style, options);
            else EditorGUILayout.BeginHorizontal(options);
            content.Invoke();
            EditorGUILayout.EndVertical();
        }

        public static void HorizontalSection<T>([CanBeNull] GUIStyle style, T obj, Action<T> content)
            where T : class
        {
            if (style != null) EditorGUILayout.BeginHorizontal(style);
            else EditorGUILayout.BeginHorizontal();
            content.Invoke(obj);
            EditorGUILayout.EndHorizontal();
        }

        public static void HorizontalSection([CanBeNull] GUIStyle style, Action content,
            params GUILayoutOption[] options)
        {
            if (style != null) EditorGUILayout.BeginHorizontal(style, options);
            else EditorGUILayout.BeginHorizontal();
            content.Invoke();
            EditorGUILayout.EndHorizontal();
        }

        public static void Area<T>(Action<T> content, params object[] args) where T : class
        {
            var rect = new Rect();
            var nodeBg = ENodeBgType.Node0;
            var style = EdStylesExtra.CustomNodeStyle(nodeBg,
                new[] { 12, 12, 12, 12 },
                new[] { 10, 10, 10, 10 },
                Color.white);
            T node = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] is Rect)
                {
                    rect = (Rect)args[i];
                }

                if (args[i] is ENodeBgType)
                {
                    nodeBg = (ENodeBgType)args[i];
                    style = EdStylesExtra.CustomNodeStyle(nodeBg,
                        new[] { 12, 12, 12, 12 },
                        new[] { 10, 10, 10, 10 },
                        Color.white);
                }

                if (args[i] is T)
                {
                    node = (T)args[i];
                }

                if (args[i] is GUIStyle)
                {
                    style = (GUIStyle)args[i];
                }
            }

            GUILayout.BeginArea(rect, style);

            content.Invoke(node);
            GUILayout.EndArea();
        }

        public static void Area<T>(Rect _rect, ENodeBgType _bgType, GUIStyle _style, T _node, Action<T> content)
            where T : class
        {
            var style = _style ?? EdStylesExtra.CustomNodeStyle(_bgType,
                new[] { 12, 12, 12, 12 },
                new[] { 10, 10, 10, 10 },
                Color.white);

            GUILayout.BeginArea(_rect, style);

            content.Invoke(_node);
            GUILayout.EndArea();
        }

        public static void Area(Rect _rect, ENodeBgType _bgType, GUIStyle _style, Action content)
        {
            var style = _style ?? EdStylesExtra.CustomNodeStyle(_bgType,
                new[] { 12, 12, 12, 12 },
                new[] { 10, 10, 10, 10 },
                Color.white);

            GUILayout.BeginArea(_rect, style);

            content.Invoke();
            GUILayout.EndArea();
        }

        public static void Area(Rect _rect, Texture2D _bg, GUIStyle _style, [CanBeNull] Action content)
        {
            var style = _style ?? EdStylesExtra.CustomNodeStyle(_bg,
                new[] { 1, 1, 1, 1 },
                new[] { 1, 1, 1, 1 },
                Color.white);

            GUILayout.BeginArea(_rect, style);
            content?.Invoke();
            GUILayout.EndArea();
        }

        public static void Foldout(ref bool showLinked, string title, GUIStyle style, Action content)
        {
            showLinked = EditorGUILayout.BeginFoldoutHeaderGroup(showLinked, title, style);
            if (showLinked)
            {
                content?.Invoke();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();
        }


        // Draws all of the properties from the given Serialized Property and children if given
        public static void DrawProperties(SerializedProperty prop, bool drawChildren = true,
            [CanBeNull] string label = null)
        {
            if (prop == null)
            {
                Debug.LogWarning("No property provided to DRAWPROPERTY function.");
                return;
            }

            if (label != null)
            {
                EditorGUILayout.LabelField(label,
                    EdStylesExtra.Title);
                EdStylesExtra.HorizontalLine();
            }

            var lastpropPath = string.Empty;
            foreach (SerializedProperty p in prop)
            {
                if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
                {
                    EditorGUILayout.BeginHorizontal();
                    p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                    EditorGUILayout.EndHorizontal();
                    if (p.isExpanded)
                    {
                        EditorGUI.indentLevel++;
                        DrawProperties(p, drawChildren);
                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastpropPath) && p.propertyPath.Contains(lastpropPath))
                    {
                        continue;
                    }

                    lastpropPath = p.propertyPath;
                    EditorGUILayout.PropertyField(p, drawChildren);
                }
            }
        }

        public static void DrawField(SerializedProperty target, string propName)
        {
            if (target != null)
            {
                EditorGUILayout.PropertyField(target.FindPropertyRelative(propName), true);
            }
        }

        public static void DrawField(SerializedObject target, string propName)
        {
            if (target != null)
            {
                EditorGUILayout.PropertyField(target.FindProperty(propName), true);
            }
        }
    }
}