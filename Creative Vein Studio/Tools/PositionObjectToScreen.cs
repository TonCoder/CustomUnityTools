using UnityEngine;

namespace _MainApp.Scripts.Tools
{
    public class PositionObjectToScreen : MonoBehaviour
    {
        [SerializeField] private Camera mainCam;
        [SerializeField] public Vector3 objectPositionToCam; // The screen position where you want to keep the sprite
        [SerializeField] private bool positionSet;

        public bool clampX = false;
        public bool clampY = false;
        public bool clampZ = false;

        private Vector3 worldPosition;

        private void Start()
        {
            // transform.position = new Vector3(mainCam.transform.position.x, transform.position.y, 0);
            worldPosition = mainCam.ViewportToWorldPoint(objectPositionToCam);
            if (clampX) worldPosition.x = transform.position.x; // Keep the z-position constant
            if (clampY) worldPosition.y = transform.position.y; // Keep the z-position constant
            if (clampZ) worldPosition.z = transform.position.z; // Keep the z-position constant
            transform.position = worldPosition;
        }

        private void FixedUpdate()
        {
            if (positionSet) return;
            worldPosition = mainCam.ViewportToWorldPoint(objectPositionToCam);
            if (clampX) worldPosition.x = transform.position.x; // Keep the z-position constant
            if (clampY) worldPosition.y = transform.position.y; // Keep the z-position constant
            if (clampZ) worldPosition.z = transform.position.z; // Keep the z-position constant
            transform.position = worldPosition;
        }
    }
}