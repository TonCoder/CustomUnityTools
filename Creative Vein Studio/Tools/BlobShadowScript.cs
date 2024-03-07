using System;
using UnityEngine;
using System.Collections;

public class BlobShadowScript : MonoBehaviour
{
    [Header("Settings")] public GameObject blobShadowParent;
    public Vector3 blobShadowParentOffset = new Vector3(0f, 0.01f, 0f);
    public LayerMask hitLayer;
    public float rayLength = 5f;
    public Vector3 vectorDir = -Vector3.up;

    private void Awake()
    {
        blobShadowParent = transform.parent.gameObject;
    }

    void LateUpdate()
    {
        Ray ray = new Ray(blobShadowParent.transform.position,
            -blobShadowParent.transform.TransformDirection(Vector3.up));
        Debug.DrawRay(blobShadowParent.transform.position,
            -blobShadowParent.transform.TransformDirection(Vector3.up) * rayLength, Color.green);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, rayLength, ~hitLayer))
        {
            transform.position = hitInfo.point + blobShadowParentOffset;
            transform.up = hitInfo.normal;
        }
        else
        {
            transform.position = blobShadowParent.transform.position + blobShadowParentOffset;
        }
    }
}