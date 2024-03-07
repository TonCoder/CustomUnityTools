using CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects;
using UnityEditor;
using UnityEngine;


namespace CreativeVeinStudio.Simple_Dialogue_System.Editor
{
    [CustomEditor(typeof(SO_Dialogue))]
    public class DialogueEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var dialogue = target as SO_Dialogue;
            dialogue.GlobalVars =
                (SO_GlobalVars)EditorGUILayout.ObjectField("Global Variable", dialogue.GlobalVars,
                    typeof(SO_GlobalVars),
                    false);

            if (GUILayout.Button("Open Canvas"))
            {
                ExtendEditorWindow.ShowEditorWindow((SO_Dialogue)target);
            }
        }
    }
}