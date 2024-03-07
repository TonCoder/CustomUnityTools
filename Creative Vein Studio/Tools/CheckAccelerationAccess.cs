using Unity.Collections;
using UnityEngine;

namespace _MainApp.Scripts.Tools
{
    public class CheckAccelerationAccess : MonoBehaviour
    {
        public static CheckAccelerationAccess instance;

        [SerializeField, ReadOnly] private bool hasAcceleration;
        [SerializeField, ReadOnly] private bool hasAccessToAcceleration;

        public bool HasAccessToAcceleration => hasAccessToAcceleration;
        public bool HasAcceleration => hasAcceleration;

        private void Awake()
        {
            instance = this;
        }

        void Start()
        {
            // Check if the device supports accelerometer
            if (SystemInfo.supportsAccelerometer && Application.platform == RuntimePlatform.Android
                || SystemInfo.supportsAccelerometer && Application.platform == RuntimePlatform.IPhonePlayer)
            {
                hasAcceleration = true;
                Debug.Log("Accelerometer is supported on this device.");
            }
            else
            {
                hasAcceleration = false;
                Debug.Log("Accelerometer is not supported on this device.");
            }


            // If accelerometer is supported but not enabled, request access
            if (hasAcceleration && !Input.gyro.enabled)
            {
                hasAccessToAcceleration = false;
                StartCoroutine(EnableAccelerometer());
            }
        }

        System.Collections.IEnumerator EnableAccelerometer()
        {
            yield return new WaitForSeconds(1f); // Wait for a brief moment before requesting access

            // Request access to use the accelerometer
            Input.gyro.enabled = true;

            // Check if the access was granted
            if (Input.gyro.enabled)
            {
                hasAccessToAcceleration = true;
                Debug.Log("Accelerometer access granted.");
            }
            else
            {
                hasAccessToAcceleration = false;
                Debug.Log("Failed to grant access to the accelerometer.");
            }
        }
    }
}