using CreativeVeinStudio.Simple_Dialogue_System.Abstracts;
using CreativeVeinStudio.Simple_Dialogue_System.Interface;
using CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects.Nodes;
using UnityEditor;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Editor
{
    [CustomEditor(typeof(ABaseNode), true)]
    public class NodesEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}