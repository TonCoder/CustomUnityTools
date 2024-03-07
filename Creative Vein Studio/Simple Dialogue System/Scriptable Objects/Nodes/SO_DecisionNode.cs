using System;
using CreativeVeinStudio.Simple_Dialogue_System.Abstracts;
using CreativeVeinStudio.Simple_Dialogue_System.Attributes;
using CreativeVeinStudio.Simple_Dialogue_System.Attributes.Helpers;
using CreativeVeinStudio.Simple_Dialogue_System.Enums;
using CreativeVeinStudio.Simple_Dialogue_System.Interface;
using CreativeVeinStudio.Simple_Dialogue_System.Models;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects.Nodes
{
    [System.Serializable]
    public class SO_DecisionNode : ABaseNode, INodeProps
    {
        [SerializeField] private Rect rect = new Rect(10, 10, 115, 65);

        [SerializeField] internal DecisionNodeSelection onTrueNode;
        [SerializeField] internal DecisionNodeSelection onFalseNode;
        [Space] [SerializeField] internal Decision decision;

        public override DecisionNodeSelection IfTrueNode => onTrueNode;
        public override DecisionNodeSelection IfFalseNode => onFalseNode;

        public string GetCharacterSpeaking() => "";
        public string GetDialogue() => "";

        public override Rect NodeRect => rect;

        public override Vector2 NodePos
        {
            get => rect.position;
            set => rect.position = value;
        }

        public override void AddChild(INodes selectedNode)
        {
            if (selectedNode == this)
            {
                Debug.Log("Unable to link to self");
                return;
            }

            if (onTrueNode.isChanging)
            {
                onTrueNode.node = (ABaseNode)selectedNode;
                onTrueNode.isChanging = false;
            }

            if (onFalseNode.isChanging)
            {
                onFalseNode.node = (ABaseNode)selectedNode;
                onFalseNode.isChanging = false;
            }

            selectedNode.AddParent(this);
        }

        public override void RemoveChildrenLinks(INodes node)
        {
            if (node == null)
            {
                onTrueNode.isChanging = false;
                onFalseNode.isChanging = false;
                (onTrueNode.node)?.RemoveParentLinks(this);
                (onFalseNode.node)?.RemoveParentLinks(this);

                onTrueNode.node = onFalseNode.node = null;
                return;
            }

            onTrueNode.node = (onTrueNode.node == (ABaseNode)node) ? null : onTrueNode.node;
            onFalseNode.node = (onFalseNode.node == (ABaseNode)node) ? null : onFalseNode.node;
        }

        public bool ProcessDecision(SO_Dialogue dialogue, float ranQty)
        {
            switch (decision.condition)
            {
                case EConditional.GraterThan:
                case EConditional.LessThan:
                case EConditional.EqualTo:
                case EConditional.NotEqualTo:
                    // convert values to numbers
                    GetNumber(GetVariableData(dialogue, dialogue.GlobalVars, decision.variable2.value),
                        out var numberVal2);

                    if (decision.condition == EConditional.LessThan)
                        return ranQty < numberVal2;
                    if (decision.condition == EConditional.GraterThan)
                        return ranQty > numberVal2;
                    if (decision.condition == EConditional.EqualTo)
                        return (ranQty - numberVal2) == 0;
                    if (decision.condition == EConditional.NotEqualTo)
                        return ranQty - numberVal2 != 0;
                    break;
                default: return false;
            }

            return false;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public bool ProcessDecision(SO_Dialogue dialogue)
        {
            switch (decision.condition)
            {
                case EConditional.GraterThan:
                case EConditional.LessThan:
                case EConditional.EqualTo:
                case EConditional.NotEqualTo:
                    // convert values to numbers
                    GetNumber(GetVariableData(dialogue, dialogue.GlobalVars, decision.variable1.value), out
                        var numberVal1);

                    GetNumber(GetVariableData(dialogue, dialogue.GlobalVars, decision.variable2.value),
                        out var numberVal2);

                    if (decision.condition == EConditional.LessThan)
                        return numberVal1 < numberVal2;
                    if (decision.condition == EConditional.GraterThan)
                        return numberVal1 > numberVal2;
                    if (decision.condition == EConditional.EqualTo)
                        return (numberVal1 - numberVal2) == 0;
                    if (decision.condition == EConditional.NotEqualTo)
                        return numberVal1 - numberVal2 != 0;
                    break;
                case EConditional.Contains:
                    var value1 = GetVariableData(dialogue, dialogue.GlobalVars, decision.variable1.value);
                    var value2 = GetVariableData(dialogue, dialogue.GlobalVars, decision.variable2.value);
                    return value1.Contains(value2);
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return false;
        }

        private string GetVariableData(ICustomVarActions local, ICustomVarActions global, string valueName)
        {
            return decision.variable1.varType switch
            {
                EVariableOptions.IsString => valueName,
                EVariableOptions.IsLocal => local.GetValueByName(valueName)
                    .varValue,
                EVariableOptions.IsGlobal => global.GetValueByName(valueName)
                    .varValue,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void GetNumber(string val, out float number)
        {
            if (float.TryParse(val, out number)) return;
            Debug.LogError($"Unable to parse the value provided to a number");
            Debug.DebugBreak();
        }
    }

    [System.Serializable]
    public class Decision
    {
        [SerializeField,
         ToggleDropDownOptions(typeof(MakeDropdownListHelper), "GetGlobalVars")]
        public VariableTypes variable1;

        [SerializeField] public EConditional condition;

        [SerializeField, ToggleDropDownOptions(typeof(MakeDropdownListHelper), "GetGlobalVars")]
        public VariableTypes variable2;
    }
}