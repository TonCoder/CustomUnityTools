using System;
using UnityEngine;

namespace MAIN_PROJECT._Scripts.Tools.Scriptable_Objects.Variables
{
    [CreateAssetMenu(fileName = "FloatVar", menuName = "CVeinStudio/Variables/")]
    public class SoFloatVariables : ScriptableObject, IVariableActions<float>
    {
        [SerializeField] private float value;

        public float Value => value;

        private float _initValue;

        public Action<float> OnValueChanged { get; set; }

        public void UpdateValue(float val)
        {
            value += val;
            TriggerValueChanged(value);
        }

        public void ResetToInitValue()
        {
            value = _initValue;
        }

        public void TriggerValueChanged(float val)
        {
            OnValueChanged?.Invoke(val);
        }

        public float InitialValue
        {
            set => _initValue = value;
        }
    }
}