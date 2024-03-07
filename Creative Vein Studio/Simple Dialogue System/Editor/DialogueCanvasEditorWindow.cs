using System;
using System.Linq;
using CreativeVeinStudio.Simple_Dialogue_System.Editor.Helpers;
using CreativeVeinStudio.Simple_Dialogue_System.Enums;
using CreativeVeinStudio.Simple_Dialogue_System.Interface;
using CreativeVeinStudio.Simple_Dialogue_System.Models;
using CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects.Nodes;
using CreativeVeinStudio.Simple_Dialogue_System.Tools;
using UnityEngine;
using UnityEditor;
using UnityEditor.Graphs;
using UnityEditorInternal;


namespace CreativeVeinStudio.Simple_Dialogue_System.Editor
{
    public class DialogueCanvasEditorWindow : ExtendEditorWindow
    {
        [SerializeField] internal EDialogueStates currentState = EDialogueStates.None;
        [SerializeField] internal CanvasSettings canvasSettings = new CanvasSettings();
        [SerializeField] internal NodeCanvasSettings nodeSettings = new NodeCanvasSettings();

        [NonSerialized] private INodes _outputLinkNode;
        [NonSerialized] private INodes _inputLinkNode;

        public SerializedObject SDialogueObject => serializedDialogueObj;

        public SerializedObject SerializedNodeObj
        {
            get => serializedNode;
            set => serializedNode = value;
        }

        #region Private Vars

        private float zoomScale = 1f;

        private Event _currentEvent;

        private string dialogueName = string.Empty;

        private Vector2 _dragCanvasOffset = Vector2.zero;
        private Vector2 _draggingOffset;

        private INodes _selectedNode;
        private Vector2 _newNodePlacementPos;

        private Vector3[] _linePoints;
        private ReorderableList _reorderedList;
        private ReorderableList _argsList;

        // Property Panel settings
        private Vector2 _propScrollPos;
        private Vector2 _linkScroll;

        private DrawPropertyPanelActions propertyPanel;
        private DrawNodeActions drawNodeActions;

        #endregion

        //*************************
        // UNITY functions
        //*************************

        #region Unity Functions

        private void OnEnable()
        {
            Selection.selectionChanged += OnDialogueObjectSelectionChange;
            currentState = EDialogueStates.None;
            canvasSettings.Init();
            nodeSettings.Init();

            propertyPanel = new DrawPropertyPanelActions(this);
            drawNodeActions = new DrawNodeActions();

            OnDialogueObjectSelectionChange();
        }

        private void OnDisable()
        {
            Selection.selectionChanged -= OnDialogueObjectSelectionChange;
        }

        private void OnGUI()
        {
            GUIDraw();
        }

        private void GUIDraw()
        {
            if (selectedDialogue != null)
            {
                _currentEvent = Event.current;

                // Any action on the canvas should be performed before drawing anything else
                // DragAndScrollCanvas(Event.current);
                propertyPanel.DrawPropertiesPanel(this);

                DrawCanvasSection();

                ProcessMouseOrKeyEvents();

                // This needs to be processed after the DRAW LOOP so it does not get collection mutation issue
                ProcessState();
            }
            else
            {
                CreateDialogueIfNoneExist();
            }

            if (GUI.changed)
            {
                Repaint();
            }
        }

        private void ProcessState()
        {
            switch (currentState)
            {
                case EDialogueStates.None:
                    break;

                case EDialogueStates.DraggingNode:
                    DraggingNode();
                    break;

                case EDialogueStates.DraggingCanvas:
                    switch (_currentEvent.type)
                    {
                        case EventType.MouseUp when _currentEvent.button == 2:
                            currentState = EDialogueStates.None;
                            GUI.changed = true;
                            return;
                        case EventType.MouseDrag when _currentEvent.button == 2:
                            // _dragCanvasOffset = canvasSettings.MousePos * zoomScale;
                            // canvasSettings.ScrollPosition = _dragCanvasOffset - canvasSettings.MousePos;
                            // Used to update the editor GUI when there is a change on the data, in this case the dragging
                            GUI.changed = true;
                            break;
                    }

                    break;

                case EDialogueStates.RemovingLink:
                    Debug.Log($"Delete node links: {_inputLinkNode.NodeType}");
                    _inputLinkNode.RemoveParentLinks();
                    _inputLinkNode = null;
                    currentState = EDialogueStates.None;
                    break;

                case EDialogueStates.Linking:
                    // Linking(_outputLinkNode, new Color(0.37f, 1f, 0.53f), 25);
                    break;

                case EDialogueStates.DecisionLinkTrue:
                case EDialogueStates.DecisionLinkFalse:
                    // Linking(_outputLinkNode,
                    //     currentState == EDialogueStates.DecisionLinkTrue ? Color.green : Color.red,
                    //     currentState == EDialogueStates.DecisionLinkTrue ? -41 : -15);
                    break;

                case EDialogueStates.Deleting:
                    Debug.Log($"Delete node: {_selectedNode.NodeType}");
                    selectedDialogue.DeleteCurrentNodeAndLinks(_selectedNode);
                    _selectedNode.Selected = false;
                    _selectedNode = null;
                    currentState = EDialogueStates.None;
                    break;

                case EDialogueStates.Selecting:
                    SelectionMade();
                    break;
                case EDialogueStates.ContextMenu:
                    ContextMenuCalled();
                    break;
                default:
                    return;
            }
        }


        /// <summary>
        /// Any drawing actions should be performed within this function
        /// </summary>
        private void DrawCanvasSection()
        {
            GUILayout.BeginArea(new Rect(canvasSettings.PropertyPanelRect.width, 0,
                this.rootVisualElement.worldBound.width - canvasSettings.PropertyPanelRect.width,
                this.rootVisualElement.worldBound.height));

            EditorGUILayout.Space(10f);
            // Zoom Slider
            EditorExtend.HorizontalSection(null, () =>
            {
                GUILayout.Label("Zoom", GUILayout.Width(40f));
                zoomScale = GUILayout.HorizontalSlider(zoomScale, 0.5f, 1f, GUILayout.ExpandWidth(true));
            });
            EditorGUILayout.Space(10f);

            // Apply zoom and offset
            GUI.EndScrollView();

            // the viewport of the scroll object
            Rect scrollViewport =
                GUILayoutUtility.GetRect(0f, 0f, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            scrollViewport.position = new Vector2(0, 25);

            // the content inside of the scroll object
            Rect rectOfContentInsideOfViewPort =
                new Rect(0f, 30, CanvasSettings.CanvasSize * zoomScale, CanvasSettings.CanvasSize * zoomScale);

            // the texture used for the content inside of the viewport
            GUI.DrawTextureWithTexCoords(rectOfContentInsideOfViewPort, canvasSettings.CanvasBgTex,
                canvasSettings.TextureCords);

            canvasSettings.ScrollPosition =
                GUI.BeginScrollView(scrollViewport,
                    currentState == EDialogueStates.DraggingCanvas
                        ? canvasSettings.ScrollPosition + (_dragCanvasOffset - canvasSettings.MousePos)
                        : canvasSettings.ScrollPosition,
                    rectOfContentInsideOfViewPort,
                    true,
                    true);

            // canvasSettings.MousePos = _currentEvent.mousePosition / zoomScale; // Gets the mouse position in the current canvas
            canvasSettings.MousePos = _currentEvent.mousePosition / zoomScale;

            // Draw your content here
            var index = 0;
            foreach (var node in selectedDialogue.GetAllNodes.Cast<INodes>())
            {
                drawNodeActions.DrawLinkConnections(node, zoomScale);
                drawNodeActions.DrawNode(node, zoomScale, index, ENodeBgType.Node0, nodeSettings);
                drawNodeActions.DrawInputOutputBtns(node, zoomScale, ref _selectedNode,
                    ref _inputLinkNode, ref _outputLinkNode, ref currentState);
                index++;
            }

            if (currentState == EDialogueStates.Linking)
                Linking(_outputLinkNode, new Color(0.37f, 1f, 0.53f), 25);


            if (currentState is EDialogueStates.DecisionLinkTrue or EDialogueStates.DecisionLinkFalse)
                Linking(_outputLinkNode,
                    currentState == EDialogueStates.DecisionLinkTrue ? Color.green : Color.red,
                    currentState == EDialogueStates.DecisionLinkTrue ? 35 : 10);

            GUI.EndScrollView();
            GUILayout.EndArea();
        }

        private void CreateDialogueIfNoneExist()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(new GUIContent("Dialogue Name:", "This is the name of the dialogue asset"),
                GUILayout.ExpandWidth(false), GUILayout.Width(100));
            dialogueName = EditorGUILayout.TextField(dialogueName, GUILayout.ExpandWidth(false));

            EditorGUILayout.LabelField(new GUIContent("Dialogue Path:",
                    "This is the path for the dialogue tree to live in. ex: Assets/..filepath../"),
                GUILayout.ExpandWidth(false), GUILayout.Width(100));
            canvasSettings.Path = EditorGUILayout.TextField(canvasSettings.Path, GUILayout.ExpandWidth(false));

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Create Dialogue",
                    new GUIStyle(EditorStyles.miniButtonLeft)
                    {
                        padding = new RectOffset(10, 10, 5, 5),
                        fixedHeight = 40
                    },
                    GUILayout.ExpandWidth(false)))
            {
                if (string.IsNullOrEmpty(canvasSettings.Path) || string.IsNullOrEmpty(dialogueName)) return;
                CreateNewScriptableDialogueInPath($"{canvasSettings.Path + dialogueName}.asset");
            }

            EditorGUILayout.LabelField("Create new dialogue tree");
        }

        private void DraggingNode()
        {
            if (_selectedNode == null || _currentEvent.type == EventType.MouseUp && _currentEvent.button == 0)
            {
                currentState = EDialogueStates.None;
                return;
            }

            if (_currentEvent.type == EventType.MouseDrag && _currentEvent.button == 0)
            {
                _selectedNode.Drag(canvasSettings.MousePos + _draggingOffset);
                GUI.changed = true; // Used to update the editor GUI when there is a change on the data 
            }
        }

        private void Linking(INodes node, Color lineColor, float yInputSpacing = -30)
        {
            if (node == null) return;

            // DRAW LINE TO MOUSE POS - 
            // NOTE :  Since we are drawing the line outside of the AREA of the CANVAS (not inside the startArea/endArea)
            // we need to add the width of the property panel to accomodate for the difference in distance
            var linePosStart = new Vector2(node.NodeRect.xMax * zoomScale,
                (node.NodeRect.yMax - yInputSpacing) * zoomScale);

            var linePosEnd = new Vector2(
                (canvasSettings.MousePos.x)
                * zoomScale,
                canvasSettings.MousePos.y * zoomScale);

            Handles.color = lineColor;
            DrawNodeActions.Line(linePosStart, linePosEnd, 15, 3);

            GUI.changed = true;
        }

        private void ContextMenuCalled()
        {
            var rightClickArea = new Rect(canvasSettings.PropertyPanelRect.width, 0, CanvasSettings.CanvasSize,
                CanvasSettings.CanvasSize);
            _newNodePlacementPos = canvasSettings.MousePos;

            if (_outputLinkNode != null && _outputLinkNode.NodeType != NodeType.ChoiceNode &&
                _outputLinkNode.NodeType != NodeType.DecisionNode)
            {
                _outputLinkNode.RemoveChildrenLinks();
            }

            if (rightClickArea.Contains(_currentEvent.mousePosition))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Add Root Node"), false, AddRootNode);
                menu.AddItem(new GUIContent("Add Say Node"), false, AddSayNode);
                menu.AddItem(new GUIContent("Add Choice Node"), false, AddChoiceNode);
                menu.AddItem(new GUIContent("Add Decision Node"), false, AddDecisionNode);
                menu.AddItem(new GUIContent("Add End Node"), false, AddEndNode);
                menu.AddItem(new GUIContent("Add Link Node"), false, AddLinkNode);
                menu.ShowAsContext();
            }

            _currentEvent.Use();
            currentState = EDialogueStates.None;
        }

        private void SelectionMade()
        {
            // SET PREV SELECTION TO SELECTED FALSE
            if (_selectedNode != null) _selectedNode.Selected = false;
            _selectedNode = GetNodeAtClickedPoint(canvasSettings.MousePos);
            if (_selectedNode != null) _selectedNode.Selected = true;

            // IF both output and selected are not null, connect them.
            if (_outputLinkNode != null && _selectedNode != null)
            {
                if (_outputLinkNode.NodeType != NodeType.ChoiceNode &&
                    _outputLinkNode.NodeType != NodeType.DecisionNode)
                    _outputLinkNode.RemoveChildrenLinks();

                // add parent to the selected node
                if (_selectedNode != null && _selectedNode.NodeType != NodeType.RootNode)
                {
                    _outputLinkNode.AddChild(_selectedNode);
                    _outputLinkNode = null;
                    _selectedNode.Selected = false;
                    _selectedNode = null;
                }

                _outputLinkNode = null;
                GUI.changed = true;
                currentState = EDialogueStates.None;
                return;
            }

            // IF output is NOT NULL and selection still needs to be made
            if (_outputLinkNode != null && _selectedNode == null)
            {
                if (_outputLinkNode.NodeType != NodeType.ChoiceNode &&
                    _outputLinkNode.NodeType != NodeType.DecisionNode)
                {
                    _outputLinkNode.RemoveChildrenLinks();
                }

                var newNodeId = selectedDialogue.CreateNewNode(_outputLinkNode, NodeType.SayNode,
                    canvasSettings.MousePos);

                _outputLinkNode.AddChild(newNodeId);
                // newNodeId.AddParent(_outputLinkNode);

                _outputLinkNode = null;
                currentState = EDialogueStates.None;
                return;
            }

            if (_selectedNode != null)
            {
                _selectedNode.Selected = true;
                _draggingOffset = _selectedNode.NodePos - canvasSettings.MousePos;

                // Makes the selected object to be displayed in the INSPECTOR
                Selection.activeObject = _selectedNode.NodeType switch
                {
                    NodeType.RootNode => (SO_RootNode)_selectedNode,
                    NodeType.SayNode => (SO_SayNode)_selectedNode,
                    NodeType.End => (SO_EndNode)_selectedNode,
                    NodeType.LinkNode => (SO_LinkNode)_selectedNode,
                    NodeType.ChoiceNode => (SO_ChoicesNode)_selectedNode,
                    NodeType.DecisionNode => (SO_DecisionNode)_selectedNode,
                    _ => throw new ArgumentOutOfRangeException()
                };
                GUI.changed = true;
            }

            currentState = EDialogueStates.None;
        }

        private static void CreateNewScriptableDialogueInPath(string path)
        {
            var dialogueObj = ScriptableObject.CreateInstance<SO_Dialogue>();
            // path has to start at "Assets"
            AssetDatabase.CreateAsset(dialogueObj, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = dialogueObj;
        }

        #endregion


        //*************************
        // Event handling
        //*************************

        // This is the Scriptable Object that is selected on the Project window/panel
        private void OnDialogueObjectSelectionChange()
        {
            var dialogue = Selection.activeObject as SO_Dialogue;
            if (dialogue == null || selectedDialogue == dialogue)
            {
                return;
            }

            selectedDialogue = dialogue;
            ShowEditorWindow(dialogue);
            serializedDialogueObj ??= new SerializedObject(dialogue);
        }


        private void ProcessMouseOrKeyEvents()
        {
            switch (_currentEvent.type)
            {
                // DRAGGING CANVAS MIDDLE MOUSE BUTTON
                case EventType.MouseDown when _currentEvent.button == 2:
                    _dragCanvasOffset = canvasSettings.MousePos * zoomScale;
                    currentState = EDialogueStates.DraggingCanvas;

                    break;

                case EventType.KeyDown when _currentEvent.keyCode == KeyCode.Escape:
                    _outputLinkNode = null;
                    _inputLinkNode = null;
                    currentState = EDialogueStates.None;
                    break;

                // Process Right Click Menu
                case EventType.ContextClick:
                    currentState = EDialogueStates.ContextMenu;
                    break;

                // CREATING NEW NODE AFTER CLICKING ADD CHILD BTN AND CLICKING BLANK SPACE Or Attach to next selection
                case EventType.MouseUp when _currentEvent.button == 0 && _outputLinkNode != null:

                    currentState = EDialogueStates.Selecting;
                    break;

                // SELECTING
                case EventType.MouseDown when _currentEvent.button == 0:
                    GUI.FocusControl(null); // Removes focus on controls (text fields, checkbox, etc)
                    currentState = EDialogueStates.Selecting;
                    break;

                // DRAGGING NODE
                case EventType.MouseDrag when _currentEvent.button == 0 && _selectedNode != null:
                    currentState = EDialogueStates.DraggingNode;
                    break;
            }
        }

        private INodes GetNodeAtClickedPoint(Vector2 pos)
        {
            // Debug.Log(canvasSettings.MousePos);
            return selectedDialogue.GetAllNodes.Cast<INodes>().FirstOrDefault(x =>
                new Rect(x.NodeRect.position, (x.NodeRect.size * zoomScale)).Contains(pos));
        }

        private void AddRootNode()
        {
            selectedDialogue.CreateNewNode(_outputLinkNode, NodeType.RootNode, _newNodePlacementPos);
        }

        private void AddSayNode()
        {
            selectedDialogue.CreateNewNode(_outputLinkNode, NodeType.SayNode, _newNodePlacementPos);
        }

        private void AddChoiceNode()
        {
            selectedDialogue.CreateNewNode(_outputLinkNode, NodeType.ChoiceNode, _newNodePlacementPos);
        }

        private void AddEndNode()
        {
            selectedDialogue.CreateNewNode(_outputLinkNode, NodeType.End, _newNodePlacementPos);
        }

        private void AddLinkNode()
        {
            selectedDialogue.CreateNewNode(_outputLinkNode, NodeType.LinkNode, _newNodePlacementPos);
        }

        private void AddDecisionNode()
        {
            selectedDialogue.CreateNewNode(_outputLinkNode, NodeType.DecisionNode, _newNodePlacementPos);
        }
    }
}