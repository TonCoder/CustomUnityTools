using System;
using System.Collections.Generic;
using CreativeVeinStudio.Simple_Dialogue_System.Abstracts;
using CreativeVeinStudio.Simple_Dialogue_System.Interface;
using CreativeVeinStudio.Simple_Dialogue_System.Models;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects.Nodes
{
    [System.Serializable]
    public class SO_ChoicesNode : ABaseNode, INodeProps
    {
        [SerializeField, NonReorderable] internal List<string> choices = new List<string>();
        [SerializeField] private Rect rect = new Rect(10, 10, 115, 55);

        public override List<string> Choices => choices;

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

            if (choices.Count > childNodeIds.Count)
            {
                if (childNodeIds.Contains((ABaseNode)selectedNode))
                {
                    Debug.Log("Node already assigned as child");
                    return;
                }

                childNodeIds.Add((ABaseNode)selectedNode);
                selectedNode.AddParent(this);
                return;
            }

            Debug.Log("Can't add more nodes than choices");
        }
    }
}