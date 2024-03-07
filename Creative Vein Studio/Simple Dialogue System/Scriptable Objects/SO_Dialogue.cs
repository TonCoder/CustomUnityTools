using System;
using System.Collections.Generic;
using System.Linq;
using CreativeVeinStudio.Simple_Dialogue_System.Interface;
using CreativeVeinStudio.Simple_Dialogue_System.Models;
using CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects;
using CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects.Nodes;
using CreativeVeinStudio.Simple_Dialogue_System.Tools;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;


namespace CreativeVeinStudio.Simple_Dialogue_System
{
    [CreateAssetMenu(fileName = "Dialogue_", menuName = "CVeinStudio/Dialogue")]
    [System.Serializable]
    public class SO_Dialogue : ScriptableObject, IDialogueActions, ISerializationCallbackReceiver, ICustomVarActions
    {
        #region Exposed Variables

        [SerializeField] internal Object activeRootNode;
        [SerializeField] internal List<Object> nodes = new List<Object>();
        [SerializeField] internal SO_GlobalVars globalVars;
        [SerializeField] internal List<VariablesModel> localVariables = new List<VariablesModel>();

        #endregion

        #region Properties

        public SO_GlobalVars GlobalVars
        {
            get => globalVars;
            set => globalVars = value;
        }

        public List<VariablesModel> LocalVars => localVariables;
        public List<VariablesModel> GetLocalVars() => localVariables;

        public List<Object> GetAllNodes => nodes;

        public Object ActiveRootNode
        {
            get => activeRootNode;
            set => activeRootNode = value;
        }

        #endregion

        #region Private Variables

        // This is a dictionary of child nodes to make it easier to retrieve
        // private readonly Dictionary<string, INodes> _listOfChildNodes =
        //     new Dictionary<string, INodes>();

        #endregion

        #region Unity Functions

        // private void OnEnable()
        // {
        //     globalVars = ScriptableObject.CreateInstance<SO_GlobalVars>();
        //
        // }

        #endregion

        //*************************
        //  Editor actions - Add actions
        //*************************
        public INodes CreateNewNode(INodes parentNode, NodeType newNodeType, Vector2 positionToCreate)
        {
            INodes newNode = null;
            switch (newNodeType)
            {
                case NodeType.RootNode:
                    var rootNode = CreateInstance<SO_RootNode>();
                    newNode = SetNewNodeProps(parentNode, rootNode, newNodeType, positionToCreate);
                    rootNode.name = $"{newNode.Index}_{newNodeType.ToString().Replace("Node", "")}";
                    NodeAssetDatabaseHandler.RecordObject(this);
                    nodes.Add(rootNode);
                    break;

                case NodeType.SayNode:
                    var sayNode = CreateInstance<SO_SayNode>();
                    newNode = SetNewNodeProps(parentNode, sayNode, newNodeType, positionToCreate);
                    sayNode.name = $"{newNode.Index}_{newNodeType.ToString().Replace("Node", "")}";
                    NodeAssetDatabaseHandler.RecordObject(this);
                    nodes.Add(sayNode);
                    break;

                case NodeType.ChoiceNode:
                    var choicesNode = CreateInstance<SO_ChoicesNode>();
                    newNode = SetNewNodeProps(parentNode, choicesNode, newNodeType, positionToCreate);
                    choicesNode.name = $"{newNode.Index}_{newNodeType.ToString().Replace("Node", "")}";
                    NodeAssetDatabaseHandler.RecordObject(this);
                    nodes.Add(choicesNode);
                    break;

                case NodeType.DecisionNode:
                    var decisionNode = CreateInstance<SO_DecisionNode>();
                    newNode = SetNewNodeProps(parentNode, decisionNode, newNodeType, positionToCreate);
                    decisionNode.name = $"{newNode.Index}_{newNodeType.ToString().Replace("Node", "")}";
                    NodeAssetDatabaseHandler.RecordObject(this);
                    nodes.Add(decisionNode);
                    break;

                case NodeType.LinkNode:
                    var linkNode = CreateInstance<SO_LinkNode>();
                    newNode = SetNewNodeProps(parentNode, linkNode, newNodeType, positionToCreate);
                    linkNode.name = $"{newNode.Index}_{newNodeType.ToString().Replace("Node", "")}";
                    NodeAssetDatabaseHandler.RecordObject(this);
                    nodes.Add(linkNode);
                    break;

                case NodeType.End:
                    var endNode = CreateInstance<SO_EndNode>();
                    newNode = SetNewNodeProps(parentNode, endNode, newNodeType, positionToCreate);
                    endNode.name = $"{newNodeType.ToString().Replace("Node", "")}";
                    NodeAssetDatabaseHandler.RecordObject(this);
                    nodes.Add(endNode);
                    break;
                default:
                    Debug.LogError("No NODE of that type present");
                    break;
            }

            return newNode;
        }

        private INodes SetNewNodeProps(in INodes parentNode, INodes newNode, NodeType newNodeType,
            Vector2 positionToCreate)
        {
            newNode.ParentDialogue = this;
            newNode.Index = UpdateNodeIndex(GetAllNodes.Cast<INodes>()
                .Where(x => x.NodeType == newNodeType).ToList()) + 1;
            newNode.ID = Helpers.GetTimeStamp();
            newNode.EventName = newNodeType.ToString();
            newNode.NodeType = newNodeType;
            newNode.NodePos = positionToCreate;
            NodeAssetDatabaseHandler.RecordObject(this);
            return newNode;
        }

        private int UpdateNodeIndex(IReadOnlyList<INodes> nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Index = 1 + i;
                nodes[i].Name = $"{nodes[i].Index}_{nodes[i].NodeType.ToString().Replace("Node", "")}";
            }

            return nodes.Count;
        }

        //*************************
        // Delete actions
        //*************************
        public void DeleteCurrentNodeAndLinks(INodes node)
        {
            if (node == null) return;
            node.RemoveChildrenLinks(null);
            node.RemoveParentLinks(null);

            NodeAssetDatabaseHandler.RecordObject(this);
            nodes.Remove((Object)node);
            NodeAssetDatabaseHandler.DestroyObject((Object)node);

            /*switch (node.NodeType)
            {
                case NodeType.RootNode:
                    NodeAssetDatabaseHandler.DestroyObject(node as SO_RootNode);
                    break;
                case NodeType.SayNode:
                    NodeAssetDatabaseHandler.DestroyObject(node as SO_SayNode);
                    break;
                case NodeType.ChoiceNode:
                    NodeAssetDatabaseHandler.DestroyObject(node as SO_ChoicesNode);
                    break;
                case NodeType.DecisionNode:
                    NodeAssetDatabaseHandler.DestroyObject((Object)node as SO_DecisionNode);
                    break;
                case NodeType.LinkNode:
                    NodeAssetDatabaseHandler.DestroyObject(node as SO_LinkNode);
                    break;
                case NodeType.End:
                    NodeAssetDatabaseHandler.DestroyObject(node as SO_EndNode);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }*/
        }

        //************************
        // Serializer callbacks
        //************************

        // OnBeforeSerialize is called by the Unity engine when a SAVE action is performed ex: CTRL + S
        // We then use this to ensure that the asset we have created is actually saved and if it is then we add any 
        // child object / dialogue that was not saved (or has a path) to the parent asset, in our case the SO_Dialogue
        // https://docs.unity3d.com/2020.3/Documentation/ScriptReference/ISerializationCallbackReceiver.html
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (!Application.isEditor) return;
            if (!NodeAssetDatabaseHandler.PathExist(this)) return;

            foreach (var node in nodes.Cast<INodes>())
            {
                AddObjectToAsset(node);
            }
#endif
        }

        public void OnAfterDeserialize()
        {
            // throw new NotImplementedException();
        }

        private void AddObjectToAsset(INodes node)
        {
            if (Application.isEditor)
                NodeAssetDatabaseHandler.SaveAsset(node.NodeType switch
                {
                    NodeType.End => node as SO_EndNode,
                    NodeType.RootNode => node as SO_RootNode,
                    NodeType.SayNode => node as SO_SayNode,
                    NodeType.ChoiceNode => node as SO_ChoicesNode,
                    NodeType.LinkNode => node as SO_LinkNode,
                    NodeType.DecisionNode => node as SO_DecisionNode,
                    _ => throw new ArgumentOutOfRangeException()
                }, this);
        }

        public VariablesModel GetValueByName(string val)
        {
            return localVariables.Find(x => x.varName == val);
        }
    }
}


[System.Serializable]
public class CustomVar
{
    public VarType varType;
    public string varName;
    public string value;
}

public enum VarType
{
    Float,
    Integer,
    Boolean,
    String
}