using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Connecting_Line_Gizmo : MonoBehaviour
{
    public List<Transform> objectList = new List<Transform>();
    public Color lineColor = Color.yellow;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        //List<Vector3> posList = new List<Vector3>();
        //foreach (Transform obj in objectList)
        //{
        //    posList.Add(obj.position);
        //}
        Handles.color = lineColor;
        Handles.DrawAAPolyLine(objectList.Select(x=> x.position).ToArray());
    }
#endif
}
