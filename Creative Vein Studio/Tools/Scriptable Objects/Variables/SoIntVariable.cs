using System;
using UnityEngine;

namespace MAIN_PROJECT._Scripts.Tools.Scriptable_Objects.Variables
{
    [CreateAssetMenu(fileName = "IntVar", menuName = "CVeinStudio/Variables/")]
    public class SoIntVariable : ScriptableObject, IVariableActions<int>
    {
        [SerializeField] private int value;

        public int Value => value;

        private int _initValue;

        public Action<int> OnValueChanged { get; set; }

        public void UpdateValue(int val)
        {
            value += val;
            TriggerValueChanged(value);
        }


        public void ResetToInitValue()
        {
            value = _initValue;
        }

        public void TriggerValueChanged(int val)
        {
            OnValueChanged?.Invoke(val);
        }

        public int InitialValue
        {
            set => _initValue = value;
        }
    }
}