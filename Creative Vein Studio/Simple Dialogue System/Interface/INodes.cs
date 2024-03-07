using System.Collections.Generic;
using CreativeVeinStudio.Simple_Dialogue_System.Abstracts;
using CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects.Nodes;
using JetBrains.Annotations;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Interface
{
    public interface INodes
    {
        int ID { get; set; }
        string EventName { get; set; }
        int Index { get; set; }
        bool Selected { get; set; }
        bool HasStartEvent { get; set; }
        bool HasEndEvent { get; set; }
        Rect NodeRect { get; }
        Vector2 NodePos { get; set; }
        NodeType NodeType { get; set; }
        List<ABaseNode> ParentIdList { get; }
        List<ABaseNode> ChildIdList { get; }
        List<string> EventArgs { get; set; }
        bool ToggleEvent { get; set; }
        List<string> Choices { get; }
        DecisionNodeSelection IfTrueNode { get; set; }
        DecisionNodeSelection IfFalseNode { get; set; }
        string Name { get; set; }
        string DialogueText { get; }
        SO_Dialogue ParentDialogue { get; set; }

        void RemoveParentLinks([CanBeNull] INodes node = null);
        void RemoveChildrenLinks([CanBeNull] INodes node = null);
        void AddChild(INodes selectedNode);
        void AddParent(INodes outputLinkNode);

        // editor node actions
        void Drag(Vector2 currentEventDelta);
    }


    [System.Serializable]
    public enum NodeType
    {
        RootNode = 0,
        SayNode = 1,
        ChoiceNode = 2,
        LinkNode = 3,
        End = 4,
        DecisionNode = 5
    }
}