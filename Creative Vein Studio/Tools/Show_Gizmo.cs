using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Show_Gizmo : MonoBehaviour
{
    public bool isCube = false;
    public bool useParentSize = true;
    public Color gizmoColor = Color.red;

    [Range(0.1f, 5)]
    public float circleRadius = 0.3f;
    
    [Range(0.1f, 1)]
    public float cubeSize = 1f;


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        if (isCube)
        {
            if (useParentSize)
            {
                Gizmos.DrawWireCube(transform.position, transform.localScale);
            }
            else
            {
                Gizmos.DrawWireCube(transform.position, new Vector3(cubeSize, cubeSize, cubeSize));
            }
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, circleRadius);
        }
    }
#endif


}
