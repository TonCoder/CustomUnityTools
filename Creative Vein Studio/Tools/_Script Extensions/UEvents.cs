using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine;
namespace CVStudio
{
    public sealed partial class UEvents
    {
        /// <summary>
        /// This is a standard Unity Event. Used to keep Events in one place
        /// </summary>
        [System.Serializable]
        public class EBase : UnityEvent { }

        /// <summary>
        /// Allows you to pass a String value through the Event
        /// </summary>
        [System.Serializable]
        public class EString : UnityEvent<string> { }

        /// <summary>
        /// Allows you to pass a Scene value through the Event
        /// </summary>
        [System.Serializable]
        public class EScene : UnityEvent<Scene> { }


        /// <summary>
        /// Allows you to pass a Scene value through the Event
        /// </summary>
        [System.Serializable]
        public class EBool : UnityEvent<bool> { }


        /// <summary>
        /// Allows you to pass an int value through the Event
        /// </summary>
        [System.Serializable]
        public class EInt : UnityEvent<int> { }


        /// <summary>
        /// Allows you to pass a float value through the Event
        /// </summary>
        [System.Serializable]
        public class EFloat : UnityEvent<float> { }


        /// <summary>
        /// Allows you to pass a Vector2 value through the Event
        /// </summary>
        [System.Serializable]
        public class EVector2 : UnityEvent<Vector2> { }

        /// <summary>
        /// Allows you to pass a Vector3 value through the Event
        /// </summary>
        [System.Serializable]
        public class EVector3 : UnityEvent<Vector3> { }

        /// <summary>
        /// Allows you to pass a Quaternion/Rotation value through the Event
        /// </summary>
        [System.Serializable]
        public class EQuaternion : UnityEvent<Quaternion> { }

        /// <summary>
        /// Allows you to pass two float values through the Event
        /// </summary>
        [System.Serializable]
        public class EFloatDouble : UnityEvent<float, float> { }

        /// <summary>
        /// Allows you to pass a GameObject value through the Event
        /// </summary>
        [System.Serializable]
        public class EGObject : UnityEvent<GameObject> { }

        /// <summary>
        /// Allows you to pass a SO_GeneralSettings value through the Event
        /// </summary>
        [System.Serializable]
        public class EGeneralSettings : UnityEvent<SO_AudioSettings> { }
    }
}