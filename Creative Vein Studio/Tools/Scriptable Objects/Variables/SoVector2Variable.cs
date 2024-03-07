using System;
using UnityEngine;

namespace MAIN_PROJECT._Scripts.Tools.Scriptable_Objects.Variables
{
    [CreateAssetMenu(fileName = "Vector2Var", menuName = "CVeinStudio/Variables/")]
    public class SoVector2Variable : ScriptableObject, IVariableActions<Vector2>
    {
        [SerializeField] private Vector2 value;

        public Vector2 Value => value;
        private Vector2 _initValue;

        public Action<Vector2> OnValueChanged { get; set; }

        public void UpdateValue(Vector2 val)
        {
            value = val;
            TriggerValueChanged(value);
        }

        public void ResetToInitValue()
        {
            value = _initValue;
        }

        public void TriggerValueChanged(Vector2 val)
        {
            OnValueChanged?.Invoke(val);
        }

        public Vector2 InitialValue
        {
            set => _initValue = value;
        }
    }
}