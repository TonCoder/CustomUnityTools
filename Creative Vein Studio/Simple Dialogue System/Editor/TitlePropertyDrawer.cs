using CreativeVeinStudio.Simple_Dialogue_System.Attributes;
using UnityEditor;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Editor
{
    [CustomPropertyDrawer(typeof(FieldTitleAttribute))]
    public class TitlePropertyDrawer : PropertyDrawer
    {
        private SerializedProperty content;
        private float propertyHeight = 0;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return propertyHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var labelRect = new Rect(position.x, position.y, position.width, 33);
            var att = attribute as FieldTitleAttribute;
            EditorGUI.LabelField(labelRect, att.title,
                new GUIStyle(EditorStyles.toolbar)
                {
                    alignment = TextAnchor.UpperCenter, fontStyle = FontStyle.Bold, fontSize = 15
                });

            var propertyRect = new Rect(position.x, labelRect.height, position.width,
                EditorGUI.GetPropertyHeight(property));
            EditorGUI.PropertyField(propertyRect, property, label, true);

            propertyHeight = labelRect.height + propertyRect.height;
        }
    }
}