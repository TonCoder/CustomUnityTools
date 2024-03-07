using CreativeVeinStudio.Simple_Dialogue_System.Interface;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Editor
{
    public class ExtendEditorWindow : EditorWindow
    {
        protected internal static IDialogueActions selectedDialogue;
        internal static SerializedObject serializedDialogueObj;
        internal static SerializedObject serializedNode;

        private const string WINDOW_TITLE = "Dialogue Flow Editor";

        #region Window actions

        //*************************
        // Window actions
        //*************************

        // Handles what happens when you double click on an ASSET object - ** Gets the callback of the action **
        // We check to see if it is the one we are looking for, and if so then process else ignore
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            selectedDialogue = EditorUtility.InstanceIDToObject(instanceID) as SO_Dialogue;
            return selectedDialogue != null;
        }

        [MenuItem("Tools/Creative Vein Studio/Dialogue Flow Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow(typeof(DialogueCanvasEditorWindow), false, WINDOW_TITLE);
            // var dialogue = Selection.activeObject as SO_Dialogue;
            // if (dialogue == null) return;
            // ShowEditorWindow(dialogue);
        }

        public static void ShowEditorWindow(SO_Dialogue dialogue)
        {
            GetWindow(typeof(DialogueCanvasEditorWindow), false, WINDOW_TITLE);
            serializedDialogueObj = new SerializedObject(dialogue);
        }

        #endregion
    }
}