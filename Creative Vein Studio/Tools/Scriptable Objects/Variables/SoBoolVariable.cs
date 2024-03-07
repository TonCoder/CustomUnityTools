using System;
using UnityEngine;

namespace MAIN_PROJECT._Scripts.Tools.Scriptable_Objects.Variables
{
    [CreateAssetMenu(fileName = "BoolVar", menuName = "CVeinStudio/Variables/")]
    public class SoBoolVariable : AVariableActions<bool>
    {
        [SerializeField] private bool value;
        public override bool Value => value;

        private bool _initValue;
        public override Action<bool> OnValueChanged { get; set; }

        public override bool InitialValue
        {
            set => _initValue = value;
        }

        public override void ResetToInitValue()
        {
            value = _initValue;
            TriggerValueChanged(value);
        }

        public override void TriggerValueChanged(bool val)
        {
            OnValueChanged?.Invoke(val);
        }

        public override void UpdateValue(bool val)
        {
            value = val;
        }
    }
}