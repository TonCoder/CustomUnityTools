using System;
using UnityEngine;

namespace Creative_Vein_Studio.Tools.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "PickUpBroker", menuName = "CVeinStudio/Brokers/PickUp")]
    public class SoPickUpBroker: ScriptableObject
    {
        public static Action<float> OnRotateObject;

        public static void TriggerOnRotateObject(float val)
        {
            OnRotateObject?.Invoke(val);
        }
    }
}