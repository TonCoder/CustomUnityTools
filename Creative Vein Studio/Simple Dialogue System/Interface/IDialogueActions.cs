using System.Collections.Generic;
using CreativeVeinStudio.Simple_Dialogue_System.Models;
using CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Interface
{
    public interface IDialogueActions
    {
        List<Object> GetAllNodes { get; }
        SO_GlobalVars GlobalVars { get; set; }

        List<VariablesModel> LocalVars { get; }
        List<VariablesModel> GetLocalVars();
        INodes CreateNewNode(INodes parentNode, NodeType newNodeType, Vector2 positionToCreate);
        void DeleteCurrentNodeAndLinks(INodes node);
    }
}