using System;
using UnityEngine;

namespace MAIN_PROJECT._Scripts.Tools.Scriptable_Objects.Variables
{
    public class AVariableActions<T> : ScriptableObject, IVariableActions<T>
    {
        public virtual T Value { get; }
        public virtual T InitialValue { get; set; }
        public virtual Action<T> OnValueChanged { get; set; }

        [Tooltip("Prevents the SO from being destroyed when changing scenes in build game.")]
        public bool persistInBuild;

        private void OnEnable()
        {
            if (persistInBuild)
                hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        public void UnloadThisAsset()
        {
            Resources.UnloadAsset(this);
        }

        public virtual void UpdateValue(T val)
        {
        }

        public virtual void ResetToInitValue()
        {
        }

        public virtual void TriggerValueChanged(T val)
        {
        }
    }
}