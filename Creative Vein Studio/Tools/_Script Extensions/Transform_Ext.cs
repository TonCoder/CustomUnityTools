using System.Collections.Generic;
using UnityEngine;

namespace CVStudio
{
    public static class Transform_Ext
    {
        public static void ResetTransform(this Transform trans)
        {
            trans.position = Vector3.zero;
            trans.localPosition = Vector3.zero;
            trans.localScale = new Vector3(1, 1, 1);
            trans.localRotation = Quaternion.identity;
        }

        public static void ScaleBy(this Transform trans, int value)
        {
            trans.localScale = new Vector3(1 * value, 1 * value, 1 * value);
        }
        public static void ResetTransformAndScaleBy(this Transform trans, int scaleQty)
        {
            trans.position = Vector3.zero;
            trans.localPosition = Vector3.zero;
            trans.localRotation = new Quaternion(0, 0, 0, 0);
            trans.localScale = new Vector3(1 * scaleQty, 1 * scaleQty, 1 * scaleQty);
        }

        public static void SmoothFollow(this Transform trans, Transform targetTrans, float rotationSpeed)
        {
            Vector3 newPos = targetTrans.position - trans.position;
            newPos.y = 0;

            var rotate = Quaternion.Lerp(trans.rotation, Quaternion.LookRotation(newPos), Time.deltaTime * rotationSpeed);
            trans.rotation = rotate;
        }

        public static List<GameObject> FindChildObjectsWithTag(this Transform parent, string tag)
        {
            List<GameObject> taggedGameObjects = new List<GameObject>();

            for (int i = 0; i < parent.childCount; i++)
            {
                Transform child = parent.GetChild(i);
                if (child.tag == tag)
                {
                    taggedGameObjects.Add(child.gameObject);
                }
                if (child.childCount > 0)
                {
                    taggedGameObjects.AddRange(FindChildObjectsWithTag(child, tag));
                }
            }
            return taggedGameObjects;
        }
    }
}