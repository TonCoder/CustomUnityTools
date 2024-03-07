using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Rotation : MonoBehaviour
{
    [SerializeField] private Vector3 _rotateDirection;
    [SerializeField] private int _rotationSpeed;
    private Vector3 tempRotation;

    void Update()
    {
        tempRotation +=  _rotateDirection * _rotationSpeed;
        transform.Rotate(_rotateDirection * _rotationSpeed, Space.World);
    }
   
}
