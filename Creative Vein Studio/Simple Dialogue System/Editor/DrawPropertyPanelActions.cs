using System.Linq;
using CreativeVeinStudio.Simple_Dialogue_System.Abstracts;
using CreativeVeinStudio.Simple_Dialogue_System.Editor.Helpers;
using CreativeVeinStudio.Simple_Dialogue_System.Enums;
using CreativeVeinStudio.Simple_Dialogue_System.Interface;
using CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects;
using CreativeVeinStudio.Simple_Dialogue_System.Tools;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditorInternal;
using UnityEditor;

namespace CreativeVeinStudio.Simple_Dialogue_System.Editor
{
    [System.Serializable]
    public class DrawPropertyPanelActions
    {
        private static int _propertiesTab;

        private static readonly string[] PropertiesTabSelections =
            new string[] { "Global Vars", "Local Vars", "Node Props" };

        private static readonly float LabelWidth = EditorGUIUtility.labelWidth;
        private static bool _showLinked;
        private static Vector2 _linkScroll;

        private static bool _toggleEvent;

        private DialogueCanvasEditorWindow canvasEditor;
        private ReorderableList reorderableList;
        private SerializedObject serializedGlobalVar;

        public DrawPropertyPanelActions(DialogueCanvasEditorWindow cavasEditor)
        {
            canvasEditor = cavasEditor;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        internal void DrawPropertiesPanel(DialogueCanvasEditorWindow cavasEditor)
        {
            if (canvasEditor == null)
            {
                canvasEditor = cavasEditor;
            }

            GUILayout.BeginArea(
                new Rect(canvasEditor.canvasSettings.PropertyPanelRect.position.x,
                    canvasEditor.canvasSettings.PropertyPanelRect.position.y,
                    canvasEditor.canvasSettings.PropertyPanelRect.width,
                    canvasEditor.rootVisualElement.worldBound.height),
                EdStylesExtra.CustomNodeStyle(EditorGUIUtility.Load(ENodeBgType.Node0.ToString()) as Texture2D,
                    EdStylesExtra.BorderAndPadding, EdStylesExtra.BorderAndPadding, Color.white));

            EditorGUILayout.LabelField("PROPERTIES", EdStylesExtra.Title);
            EditorGUILayout.Space();

            //*************************
            // CREATE ROOT BUTTONS
            //*************************
            var addBtnPos = new Rect(new Vector2(canvasEditor.canvasSettings.PropertyPanelRect.max.x - 25,
                canvasEditor.canvasSettings.PropertyPanelRect.min.y), new Vector2(25, 25));
            if (GUI.Button(addBtnPos, "", EdStylesExtra.ButtonStyle("CollabCreate Icon")))
            {
                ExtendEditorWindow.selectedDialogue.CreateNewNode(null, NodeType.RootNode,
                    new Vector2(100, 100));
            }

            EditorGUILayout.Space(10);
            EditorExtend.HorizontalSection(null,
                () => { _propertiesTab = GUILayout.Toolbar(_propertiesTab, PropertiesTabSelections); },
                GUILayout.ExpandWidth(true));
            EdStylesExtra.HorizontalLine();

            switch (_propertiesTab)
            {
                case 0:
                    EditorGUILayout.Space();
                    // CREATE GLOBAL VARIABLE ASSET - IF DOESNT EXIST
                    if (ExtendEditorWindow.selectedDialogue.GlobalVars == null)
                    {
                        // CHECK TO SEE IF IT EXIST FIRST, is so apply else create
                        var res =
                            AssetDatabase.LoadAssetAtPath($"{canvasEditor.canvasSettings.Path}GlobalVars.asset",
                                    typeof(SO_GlobalVars)) as
                                SO_GlobalVars;
                        if (res != null) ExtendEditorWindow.selectedDialogue.GlobalVars = res;
                        else
                        {
                            if (GUILayout.Button("Create Global Asset"))
                            {
                                var globalVar = ScriptableObject.CreateInstance<SO_GlobalVars>();
                                AssetDatabase.CreateAsset(globalVar,
                                    $"{canvasEditor.canvasSettings.Path}GlobalVars.asset");
                                ExtendEditorWindow.selectedDialogue.GlobalVars = globalVar;
                            }
                        }
                    }
                    // USE EXISTING ONE
                    else
                    {
                        serializedGlobalVar ??= new SerializedObject(ExtendEditorWindow.selectedDialogue.GlobalVars);
                        serializedGlobalVar.Update();
                        EditorGUILayout.PropertyField(serializedGlobalVar.FindProperty("globalVariables"), true);
                        serializedGlobalVar.ApplyModifiedProperties();
                    }

                    break;

                case 1:
                    ExtendEditorWindow.serializedDialogueObj.Update();
                    EditorGUILayout.PropertyField(
                        ExtendEditorWindow.serializedDialogueObj.FindProperty("localVariables"), true);
                    ExtendEditorWindow.serializedDialogueObj.ApplyModifiedProperties();
                    break;
                case 2:
                    EditorGUILayout.Space();
                    DrawNodeOnPropertyPanel();
                    break;
            }

            GUILayout.EndArea();
        }

        private void DrawNodeOnPropertyPanel()
        {
            if (Selection.activeObject is not INodes node) return;

            canvasEditor.SerializedNodeObj = new SerializedObject(node as ABaseNode);

            EditorGUILayout.LabelField($"Node Properties: {node.NodeType}",
                EdStylesExtra.TextStyle(Color.white, true, TextAnchor.MiddleCenter, 14));
            EditorGUILayout.Space(15);


            // ROOT / SAY SECTION
            if (node.NodeType == NodeType.RootNode || node.NodeType == NodeType.SayNode)
            {
                EditorExtend.DrawField(canvasEditor.SerializedNodeObj, "speaking");
            }

            EditorGUILayout.Space(10);

            // LINK DRAW SECTION- Draw link list on NODES that are child to LinkNode 
            var linkList = node.ParentIdList.Where(x => x.NodeType == NodeType.LinkNode)?
                .OrderBy(x => x.Index)?.ToList();
            if (linkList is { Count: > 0 })
            {
                EditorGUILayout.Space();
                EditorExtend.Foldout(ref _showLinked, "Linked to:", null, () =>
                {
                    _linkScroll = EditorGUILayout.BeginScrollView(_linkScroll, GUILayout.ExpandWidth(true),
                        GUILayout.Height(55));

                    foreach (var link in linkList)
                    {
                        EditorExtend.HorizontalSection(EditorStyles.toolbar, () =>
                        {
                            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
                            EditorGUILayout.LabelField($"Link-{link.Index}",
                                EdStylesExtra.TextStyle(Color.white, true, TextAnchor.LowerLeft));
                            GUILayout.FlexibleSpace();
                            if (GUILayout.Button("", EdStylesExtra.ButtonStyle(20, 20, "d_TreeEditor.Trash")))
                            {
                                node.RemoveParentLinks(link);
                                link.RemoveChildrenLinks(node);
                            }
                        }, GUILayout.ExpandWidth(true));
                    }

                    EditorGUILayout.EndScrollView();
                    EditorGUILayout.Space(10);
                });
            }

            // LINK NODE Props
            if (node.NodeType == NodeType.LinkNode)
            {
                _linkScroll = EditorGUILayout.BeginScrollView(_linkScroll, GUILayout.ExpandWidth(true),
                    GUILayout.Height(100));

                EditorGUILayout.LabelField("Linked to:");
                EditorGUILayout.Space();

                foreach (var child in node.ChildIdList)
                {
                    EditorExtend.HorizontalSection(EditorStyles.toolbar, () =>
                    {
                        EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);
                        EditorGUILayout.LabelField($"{child.Index} {child.NodeType.ToString().Replace("Node", "")}",
                            EdStylesExtra.TextStyle(Color.white, true, TextAnchor.LowerLeft));
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("", EdStylesExtra.ButtonStyle(20, 20, "d_TreeEditor.Trash")))
                        {
                            child.RemoveParentLinks(node);
                            node.RemoveChildrenLinks(child);
                        }
                    }, GUILayout.ExpandWidth(true));
                }

                EditorGUILayout.EndScrollView();
                EditorGUILayout.Space(10);
            }

            // EVENT DRAW SECTION
            if (node.NodeType != NodeType.ChoiceNode && node.NodeType != NodeType.LinkNode)
            {
                EditorGUILayout.Space(5);

                EditorExtend.Foldout(ref _toggleEvent, "Event", null, () =>
                {
                    EditorGUILayout.Space();
                    EditorGUIUtility.labelWidth = 80;
                    var eventFieldContent = new GUIContent()
                    {
                        text = "Event Name:",
                        tooltip =
                            "This event name is used on the delegate call you register to. The run on START or END need to be selected"
                    };

                    // node.EventName = EditorGUILayout.TextField(eventFieldContent, node.EventName);
                    EditorExtend.DrawField(canvasEditor.SerializedNodeObj, "eventName");

                    EditorGUIUtility.labelWidth = LabelWidth;
                    EditorGUILayout.Space();
                    EditorGUILayout.LabelField("Run on:");
                    EditorGUILayout.Space(10);
                    EditorExtend.HorizontalSection(null, () =>
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Start", EdStylesExtra.ButtonStyle(node.HasStartEvent
                                    ? EdStylesExtra.Colors.ActiveColor
                                    : EdStylesExtra.Colors.BaseColor,
                                Color.white, 40, 100)))
                        {
                            var evntStart = canvasEditor.SerializedNodeObj.FindProperty("hasStartEvent");
                            evntStart.boolValue = !evntStart.boolValue;
                        }

                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("End", EdStylesExtra.ButtonStyle(node.HasEndEvent
                                    ? EdStylesExtra.Colors.ActiveColor
                                    : EdStylesExtra.Colors.BaseColor,
                                Color.white, 40, 100)))
                        {
                            var evntEnd = canvasEditor.SerializedNodeObj.FindProperty("hasEndEvent");
                            evntEnd.boolValue = !evntEnd.boolValue;
                        }

                        GUILayout.FlexibleSpace();
                    });

                    EditorGUILayout.Space(20);
                    reorderableList = EditorExtend.ReorderableLst(canvasEditor.SerializedNodeObj, "eventArguments",
                        false,
                        true, true, true, "Event Arguments");
                    reorderableList.DoLayoutList();
                });
            }

            // CHOICES SECTION
            if (node.NodeType == NodeType.ChoiceNode)
            {
                reorderableList = EditorExtend.ReorderableLst(canvasEditor.SerializedNodeObj, "choices", false,
                    true, true, true, "Choices");
                reorderableList.DoLayoutList();
            }

            // DECISION SECTION
            if (node.NodeType == NodeType.DecisionNode)
            {
                EditorGUILayout.Space(5);
                EditorExtend.DrawProperties(canvasEditor.SerializedNodeObj.FindProperty("decision"), true, "Decision:");
                GUI.changed = true;
            }

            EditorGUILayout.Space(10);

            // DIALOG DRAW SECTION
            if (node.NodeType is NodeType.RootNode or NodeType.SayNode)
            {
                EditorGUILayout.Space(5);
                EditorGUILayout.PropertyField(canvasEditor.SerializedNodeObj.FindProperty("dialogue"), true,
                    GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            }

            canvasEditor.SerializedNodeObj.ApplyModifiedProperties();
        }
    }
}

#endif