using System;
using UnityEngine;

namespace MAIN_PROJECT._Scripts.Tools.Scriptable_Objects.Variables
{
    public class SoStringVariable : ScriptableObject, IVariableActions<string>
    {
        [SerializeField] private string value;
        public string Value => value;
        private string _initValue;

        public Action<string> OnValueChanged { get; set; }

        public void UpdateValue(string val)
        {
            value = val;
            TriggerValueChanged(value);
        }


        public void ResetToInitValue()
        {
            value = _initValue;
        }

        public void TriggerValueChanged(string val)
        {
            OnValueChanged?.Invoke(val);
        }

        public string InitialValue
        {
            set => _initValue = value;
        }
    }
}