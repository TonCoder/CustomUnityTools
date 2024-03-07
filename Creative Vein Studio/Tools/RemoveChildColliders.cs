using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

public class RemoveChildColliders : MonoBehaviour
{
    public List<Collider> childColliders = new List<Collider>();

    public void ClearList() { childColliders.Clear(); }

    public void GetChildColliders()
    {
        childColliders = transform.GetComponentsInChildren<Collider>().ToList();
    }

    public void RemoveColliders()
    {
        if(childColliders.Count > 0)
        {
            childColliders.ForEach(DestroyImmediate);
            ClearList();
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(RemoveChildColliders))]
public class RemoveChildColliderBtn : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RemoveChildColliders action = (RemoveChildColliders)target;

        GUI.backgroundColor = Color.yellow;
        GUILayout.Space(10);
        if (GUILayout.Button("Clear List", GUILayout.Height(30)))
        {
            action.ClearList();
        }

        GUI.backgroundColor = Color.green;
        GUILayout.Space(10);
        if (GUILayout.Button("Get Child Colliders", GUILayout.Height(30)))
        {
            action.GetChildColliders();
        }

        GUI.backgroundColor = Color.red;
        GUILayout.Space(10);
        if (GUILayout.Button("Remove Child Colliders", GUILayout.Height(30)))
        {
            action.RemoveColliders();
        }
    }

}
#endif