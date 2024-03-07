using CreativeVeinStudio.Simple_Dialogue_System.Abstracts;
using CreativeVeinStudio.Simple_Dialogue_System.Interface;
using UnityEngine;


namespace CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects.Nodes
{
    [System.Serializable]
    public class SO_LinkNode : ABaseNode, INodeProps
    {
        [SerializeField] private Rect rect = new Rect(10, 10, 75, 35);

        public override Rect NodeRect => rect;


        public override Vector2 NodePos
        {
            get => rect.position;
            set => rect.position = value;
        }

        public string GetCharacterSpeaking() => "";
        public string GetDialogue() => "";

        public override void AddChild(INodes selectedNode)
        {
            var selection = (ABaseNode)selectedNode;
            if (selection == this)
            {
                Debug.Log("Unable to link to self");
                return;
            }

            if (parentIdList.Contains((ABaseNode)selectedNode))
            {
                Debug.Log("Link will cause Infinite loop");
                return;
            }

            childNodeIds.Add((ABaseNode)selectedNode);
            if (!selectedNode.ParentIdList.Contains(this))
            {
                selectedNode.AddParent(this);
            }
        }
    }
}