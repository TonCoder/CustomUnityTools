using System;
using System.Collections.Generic;
using CreativeVeinStudio.Simple_Dialogue_System.Interface;
using CreativeVeinStudio.Simple_Dialogue_System.Models;
using CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects.Nodes;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Abstracts
{
    [System.Serializable]
    public abstract class ABaseNode : ScriptableObject, INodes
    {
        [SerializeField] protected SO_Dialogue parentDialogue;
        [SerializeField] protected int id;
        [SerializeField] protected int index;
        [SerializeField] protected int ranQty;
        [SerializeField] protected NodeType type;
        [SerializeField] protected bool hasStartEvent;
        [SerializeField] protected bool hasEndEvent;

        [SerializeField] protected string eventName = string.Empty;

        [SerializeField] protected List<ABaseNode> parentIdList = new List<ABaseNode>();
        [SerializeField] protected List<ABaseNode> childNodeIds = new List<ABaseNode>();
        [SerializeField] protected List<string> eventArguments = new List<string>();

        private void OnDisable()
        {
            ranQty = 0;
        }

        // Node Props
        public string Name
        {
            get => name;
            set => name = value;
        }

        public abstract Rect NodeRect { get; }
        public abstract Vector2 NodePos { get; set; }

        public int ID
        {
            get => id;
            set => id = value;
        }

        public int Index
        {
            get => index;
            set => index = value;
        }

        public virtual int RanQty
        {
            get => ranQty;
            set => ranQty = value;
        }

        public bool Selected { get; set; }

        public string EventName
        {
            get => eventName;
            set => eventName = value;
        }

        public bool HasStartEvent
        {
            get => hasStartEvent;
            set => hasStartEvent = value;
        }

        public bool HasEndEvent
        {
            get => hasEndEvent;
            set => hasEndEvent = value;
        }

        public NodeType NodeType
        {
            get => type;
            set => type = value;
        }

        // Link Props
        public List<ABaseNode> ParentIdList
        {
            get => parentIdList;
            set => parentIdList = value;
        }

        public List<ABaseNode> ChildIdList
        {
            get => childNodeIds;
            set => childNodeIds = value;
        }

        public List<string> EventArgs
        {
            get => eventArguments;
            set => eventArguments = value;
        }

        public virtual string DialogueText { get; }

        public SO_Dialogue ParentDialogue
        {
            get => parentDialogue;
            set => parentDialogue = value;
        }

        // Toggles
        public virtual bool ToggleEvent { get; set; } = false;

        // Choice Props
        public virtual List<string> Choices { get; }

        // Decision Props
        public virtual DecisionNodeSelection IfTrueNode { get; set; }
        public virtual DecisionNodeSelection IfFalseNode { get; set; }


        public virtual void RemoveParentLinks(INodes node)
        {
            // removes specific node from the parent list
            if (node != null)
            {
                parentIdList.Remove((ABaseNode)node);
                return;
            }

            // removes this node from all the parents connected to it
            foreach (var parent in parentIdList)
            {
                parent.RemoveChildrenLinks(this);
            }

            parentIdList.Clear();
        }

        public virtual void RemoveChildrenLinks(INodes node)
        {
            if (node != null)
            {
                childNodeIds.Remove((ABaseNode)node);
                return;
            }

            foreach (var child in childNodeIds)
            {
                child.RemoveParentLinks(this);
            }

            childNodeIds.Clear();
        }

        public virtual void AddChild(INodes selectedNode)
        {
            var selection = (ABaseNode)selectedNode;
            if (selection == this)
            {
                Debug.Log("Unable to link to self");
                return;
            }

            if (childNodeIds.Contains(selection))
            {
                Debug.Log("Child already assigned");
                return;
            }

            if (parentIdList.Contains((ABaseNode)selectedNode))
            {
                Debug.Log("Link will cause Infinite loop");
                return;
            }

            childNodeIds.Add(selection);
            if (!selection.parentIdList.Contains(this))
                selection.AddParent(this);
        }

        public virtual void AddParent(INodes selectedNode)
        {
            var selection = (ABaseNode)selectedNode;

            if (parentIdList.Contains(selection) || selection == this)
            {
                return;
            }

            parentIdList.Add(selection);
        }

        public void Drag(Vector2 currentEventDelta)
        {
            NodePos = currentEventDelta;
        }
    }
}