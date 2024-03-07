using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MAIN_PROJECT._Scripts.Tools
{
    [System.Serializable]
    public class SceneObjectCollection : MonoBehaviour
    {
        public static SceneObjectCollection Instance;

        [SerializeField] private ObjectCollection[] sceneObjects;
        [SerializeField] private Dictionary<string, GameObject> objectDictionary = new Dictionary<string, GameObject>();

        public void Awake()
        {
            Instance = this;
            if (objectDictionary.Count != sceneObjects.Length)
            {
                BuildObjectDictionary();
            }
        }

        [ContextMenu("Dictionary Clear")]
        public void ClearDictionary()
        {
            objectDictionary.Clear();
            Debug.Log($"Done");
        }

        [ContextMenu("Dictionary Qty")]
        public void DictionaryQty()
        {
            Debug.Log($"Dictionary Qty: {objectDictionary.Count}");
        }

        [ContextMenu("Build Dictionary")]
        private void BuildObjectDictionary()
        {
            objectDictionary.Clear();
            for (int i = 0; i < sceneObjects.Length; i++)
            {
                objectDictionary.Add(sceneObjects[i].objectName, sceneObjects[i].obj);
            }

            sceneObjects = null;
        }

        public T GetObjectByNameAsComponent<T>(in string objectName)
        {
            return objectDictionary[objectName].gameObject.GetComponent<T>();
        }

        public GameObject GetObjectByName(in string objectName)
        {
            return objectDictionary[objectName].gameObject;
        }
    }

    [System.Serializable]
    public class ObjectCollection
    {
        public string objectName;

        [FormerlySerializedAs("sceneObject")] public GameObject obj;
    }
}