using CreativeVeinStudio.Simple_Dialogue_System.Abstracts;
using CreativeVeinStudio.Simple_Dialogue_System.Interface;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects.Nodes
{
    [System.Serializable]
    public class SO_EndNode : ABaseNode, INodeProps
    {
        [SerializeField] private Rect rect = new Rect(10, 10, 115, 55);

        public override Rect NodeRect => rect;


        public override Vector2 NodePos
        {
            get => rect.position;
            set => rect.position = value;
        }

        public string GetCharacterSpeaking() => "";
        public string GetDialogue() => "";

    }
}