using UnityEditor;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Tools
{
    public class NodeAssetDatabaseHandler
    {
        public static void RecordObject(SO_Dialogue soDialogue) => Undo.RecordObject(soDialogue, "Create new node");

        public static void DestroyObject(Object obj) => Undo.DestroyObjectImmediate(obj);

        public static bool PathExist(Object obj) => AssetDatabase.GetAssetPath(obj) != null;

        public static void SaveAsset(Object objToAdd, Object assetObject)
        {
            if (string.IsNullOrEmpty(AssetDatabase.GetAssetPath(objToAdd)))
            {
                AssetDatabase.AddObjectToAsset(objToAdd, assetObject);
            }
        }
    }
}