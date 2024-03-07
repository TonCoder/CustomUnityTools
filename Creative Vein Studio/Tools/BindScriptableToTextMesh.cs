using System;
using MAIN_PROJECT._Scripts.Tools.Scriptable_Objects.Variables;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MAIN_PROJECT._Scripts.Tools
{
    [System.Serializable]
    public enum VarTypes
    {
        FLOAT,
        INT,
        BOOL,
        VECTOR2,
        VECTOR3,
        STRING
    }

    public class BindScriptableToTextMesh : MonoBehaviour
    {
        [SerializeField] private Object scriptableObject;
        [SerializeField] private VarTypes variableType;
        [SerializeField] private string prefix;
        [SerializeField] private string suffix;

        private TextMeshProUGUI _textMesh;

        private void Awake()
        {
            _textMesh ??= GetComponent<TextMeshProUGUI>();
            SetObservable();
        }

        private void SetObservable()
        {
            switch (variableType)
            {
                case VarTypes.FLOAT:
                    ((IVariableActions<float>)scriptableObject).OnValueChanged += UpdateValue;
                    break;
                case VarTypes.INT:
                    ((IVariableActions<int>)scriptableObject).OnValueChanged += UpdateValue;
                    break;
                case VarTypes.BOOL:
                    ((IVariableActions<bool>)scriptableObject).OnValueChanged += UpdateValue;
                    break;
                case VarTypes.VECTOR2:
                    ((IVariableActions<Vector2>)scriptableObject).OnValueChanged += UpdateValue;
                    break;
                case VarTypes.VECTOR3:
                    ((IVariableActions<Vector3>)scriptableObject).OnValueChanged += UpdateValue;
                    break;
                case VarTypes.STRING:
                    ((IVariableActions<string>)scriptableObject).OnValueChanged += UpdateValue;
                    break;
            }
        }

        private void RemoveObservable()
        {
            switch (variableType)
            {
                case VarTypes.FLOAT:
                    ((IVariableActions<float>)scriptableObject).OnValueChanged -= UpdateValue;
                    break;
                case VarTypes.INT:
                    ((IVariableActions<int>)scriptableObject).OnValueChanged -= UpdateValue;
                    break;
                case VarTypes.BOOL:
                    ((IVariableActions<bool>)scriptableObject).OnValueChanged -= UpdateValue;
                    break;
                case VarTypes.VECTOR2:
                    ((IVariableActions<Vector2>)scriptableObject).OnValueChanged -= UpdateValue;
                    break;
                case VarTypes.VECTOR3:
                    ((IVariableActions<Vector3>)scriptableObject).OnValueChanged -= UpdateValue;
                    break;
                case VarTypes.STRING:
                    ((IVariableActions<string>)scriptableObject).OnValueChanged -= UpdateValue;
                    break;
            }
        }

        private void OnDisable()
        {
            RemoveObservable();
        }

        private void UpdateValue<T>(T val)
        {
            _textMesh.text =
                $"{prefix}{val}{suffix}";
        }
    }
}