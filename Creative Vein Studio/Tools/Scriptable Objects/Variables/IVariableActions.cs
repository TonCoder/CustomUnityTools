using System;

namespace MAIN_PROJECT._Scripts.Tools.Scriptable_Objects.Variables
{
    public interface IVariableActions<T>
    {
        T Value { get; }
        T InitialValue { set; }

        Action<T> OnValueChanged { get; set; }

        void UpdateValue(T val);
        void ResetToInitValue();
        void TriggerValueChanged(T val);
    }
}