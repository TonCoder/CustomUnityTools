using System;
using UnityEngine;

namespace MAIN_PROJECT._Scripts.Tools.Scriptable_Objects.Variables
{
    [CreateAssetMenu(fileName = "Vector2Var", menuName = "CVeinStudio/Variables/")]
    public class SoVector3Variable : ScriptableObject, IVariableActions<Vector3>
    {
        [SerializeField] private Vector3 value;

        public Vector3 Value => value;

        private Vector3 _initValue;

        public Action<Vector3> OnValueChanged { get; set; }

        public Vector3 InitialValue
        {
            set => _initValue = value;
        }

        public void ResetToInitValue()
        {
            value = _initValue;
        }

        public void UpdateValue(Vector3 val)
        {
            value = val;
            TriggerValueChanged(value);
        }

        public void TriggerValueChanged(Vector3 val)
        {
            OnValueChanged?.Invoke(val);
        }
    }
}