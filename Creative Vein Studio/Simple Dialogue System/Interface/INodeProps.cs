using System.Collections.Generic;
using CreativeVeinStudio.Simple_Dialogue_System.Abstracts;
using CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects;
using CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects.Nodes;

namespace CreativeVeinStudio.Simple_Dialogue_System.Interface
{
    public interface INodeProps
    {
        int ID { get; }
        string EventName { get; }
        int Index { get; }
        int RanQty { get; set; }
        bool HasStartEvent { get; }
        bool HasEndEvent { get; }
        NodeType NodeType { get; }
        List<ABaseNode> ParentIdList { get; }
        List<ABaseNode> ChildIdList { get; }
        List<string> EventArgs { get; }
        List<string> Choices { get; }
        DecisionNodeSelection IfTrueNode { get; }
        DecisionNodeSelection IfFalseNode { get; }
        string Name { get; }
        string GetDialogue();
        string GetCharacterSpeaking();
    }
}