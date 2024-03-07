using UnityEngine;

namespace _MainApp.Scripts.Tools
{
    public class SmoothFollow2D : MonoBehaviour
    {
        public Transform target; // The target that the camera will follow
        public float smoothSpeed = 0.125f; // The speed at which the camera will follow the target
        public Vector3 offset; // The offset of the camera from the target

        public bool clampX = false;
        public bool clampY = false;
        public bool clampZ = false;

        private void FixedUpdate()
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            if (clampX) smoothedPosition.x = transform.position.x; // Keep the z-position constant
            if (clampY) smoothedPosition.y = transform.position.y; // Keep the z-position constant
            if (clampZ) smoothedPosition.z = transform.position.z; // Keep the z-position constant
            transform.position = smoothedPosition;
        }
    }
}