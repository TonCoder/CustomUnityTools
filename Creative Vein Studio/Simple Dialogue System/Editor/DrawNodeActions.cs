using System;
using System.Linq;
using CreativeVeinStudio.Simple_Dialogue_System.Editor.Helpers;
using CreativeVeinStudio.Simple_Dialogue_System.Enums;
using CreativeVeinStudio.Simple_Dialogue_System.Interface;
using CreativeVeinStudio.Simple_Dialogue_System.Models;
using CreativeVeinStudio.Simple_Dialogue_System.Tools;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

namespace CreativeVeinStudio.Simple_Dialogue_System.Editor
{
    public class DrawNodeActions
    {
        private static Vector3[] _linePoints;
        private static readonly Rect NodeLabelPosition = new Rect(5, -80, 50, 15);
        private const int TextLength = 18;
        private Rect dialoguePos = new Rect(5, 30, 120, 12);
        private const float _pointOffset = 15;

        internal void DrawNode(INodes node, float zoomScale, int indx, in ENodeBgType bgType,
            NodeCanvasSettings nodeSettings)
        {
            #region Node Label Section

            zoomScale = zoomScale == 0 ? 1 : zoomScale;

            // Get and create position for Label
            var pos = new Rect(node.NodeRect.position * zoomScale
                               + new Vector2(5 * zoomScale,
                                   node.NodeType switch
                                   {
                                       NodeType.ChoiceNode => -20 * zoomScale,
                                       _ => -35 * zoomScale
                                   }), NodeLabelPosition.size * zoomScale);

            // LABEL AREA
            var labelText = $"{node.Index} {node.NodeType.ToString().Replace("Node", "")}";
            EditorGUI.LabelField(pos, labelText,
                EdStylesExtra.TextStyle(
                    node.Selected ? Color.green : Color.white, true, TextAnchor.MiddleLeft,
                    node.Selected ? (int)Math.Ceiling(15 * zoomScale) : (int)Math.Ceiling((12 * zoomScale))));

            if (node.NodeType != NodeType.ChoiceNode)
            {
                // Create Event name label
                pos.y += EditorGUIUtility.singleLineHeight * zoomScale;
                EditorGUI.LabelField(pos, $"{node.EventName}",
                    EdStylesExtra.TextStyle(Color.white, true, TextAnchor.MiddleLeft,
                        (int)Math.Ceiling(12 * zoomScale)));
            }
            // END LABEL AREA

            #endregion

            // Line stroke

            EditorExtend.Area(new Rect(node.NodeRect.position * zoomScale, node.NodeRect.size * zoomScale),
                node.NodeType switch
                {
                    NodeType.RootNode => nodeSettings.RootBg,
                    NodeType.SayNode => nodeSettings.SayBg,
                    NodeType.End => nodeSettings.EndBg,
                    NodeType.LinkNode => nodeSettings.LinkBg,
                    NodeType.ChoiceNode => nodeSettings.ChoiceBg,
                    NodeType.DecisionNode => nodeSettings.DecisionBg,
                }, null, () =>
                {
                    if (node.NodeType is NodeType.RootNode or NodeType.SayNode)
                    {
                        var txtLength = node?.DialogueText?.Length ?? 0;
                        var txt = node.DialogueText?.Remove(txtLength > TextLength ? TextLength : txtLength,
                            txtLength > TextLength ? (txtLength - TextLength) : 0);
                        if (zoomScale < 0.7) return;
                        dialoguePos.width = (node.NodeRect.size.x - 12) * zoomScale;
                        EditorGUI.LabelField(dialoguePos, txt,
                            new GUIStyle(EditorStyles.toolbarTextField)
                                { fixedHeight = (20 * zoomScale), stretchWidth = true });
                    }
                });
        }

        internal void DrawInputOutputBtns(INodes node, float zoomScale,
            ref INodes selectedNode,
            ref INodes inputLinkNode, ref INodes outputLinkNode,
            ref EDialogueStates currentState)
        {
            float btnSize = 15;
            zoomScale = zoomScale == 0 ? 1 : zoomScale;

            var delNodePos = new Rect(
                new Vector2((node.NodePos.x + node.NodeRect.size.x - 5) * zoomScale,
                    (node.NodePos.y - 13) * zoomScale),
                Vector2.one * ((btnSize + 3) * zoomScale));

            var outputBtnPos = new Rect(
                (node.NodePos + new Vector2(node.NodeRect.width - 6, node.NodeRect.height * 0.465f)) * zoomScale,
                Vector2.one * (btnSize * zoomScale));

            var inputBtnPos = new Rect(
                (node.NodePos + new Vector2(-10, node.NodeRect.height * 0.465f)) * zoomScale,
                Vector2.one * (btnSize * zoomScale));

            var linkRect = new Rect(
                (node.NodePos + new Vector2(node.NodeRect.width - 14, node.NodeRect.height * 1f)) * zoomScale,
                Vector2.one * (btnSize * zoomScale));

            if (node.NodeType != NodeType.LinkNode &&
                node.ParentIdList?.Count(x => x.NodeType == NodeType.LinkNode) > 0)
            {
                EditorGUI.LabelField(linkRect, "", EdStylesExtra.ButtonStyle("d_Linked@2x"));
            }

            // Delete
            if (GUI.Button(delNodePos, "", EdStylesExtra.ButtonStyle("d_winbtn_mac_close_h@2x")))
            {
                selectedNode = node;
                currentState = EDialogueStates.Deleting;
            }

            // INPUT
            if (node.NodeType != NodeType.RootNode)
            {
                if (GUI.Button(inputBtnPos, "",
                        EdStylesExtra.ButtonStyle(node.ParentIdList.Any()
                            ? "sv_icon_dot2_pix16_gizmo"
                            : "sv_icon_dot0_pix16_gizmo")))
                {
                    if (inputLinkNode == node)
                    {
                        currentState = EDialogueStates.RemovingLink;
                        return;
                    }

                    inputLinkNode = node;

                    if (outputLinkNode != null)
                        currentState = EDialogueStates.Selecting;

                    return;
                }
            }

            // OUTPUT
            if (node.NodeType != NodeType.End && node.NodeType != NodeType.DecisionNode)
            {
                if (GUI.Button(outputBtnPos, "", EdStylesExtra.ButtonStyle(node.ChildIdList.Any()
                        ? "sv_icon_dot4_pix16_gizmo"
                        : "sv_icon_dot0_pix16_gizmo")))
                {
                    Debug.Log("Output clicked!!");
                    outputLinkNode = node;
                    currentState = EDialogueStates.Linking;
                    return;
                }
            }

            if (node.NodeType == NodeType.DecisionNode)
            {
                // update position
                outputBtnPos.y -= 10;

                // create label field
                EditorGUI.LabelField(
                    new Rect(new Vector2(outputBtnPos.position.x - 20, outputBtnPos.position.y), outputBtnPos.size),
                    "True",
                    EdStylesExtra.TextStyle(Color.white, false, TextAnchor.MiddleRight,
                        (int)Mathf.Ceil(12 * zoomScale)));

                // create button
                if (GUI.Button(outputBtnPos, "", EdStylesExtra.ButtonStyle(node.IfTrueNode?.node != null
                        ? "d_winbtn_mac_max@2x"
                        : "sv_icon_dot0_pix16_gizmo")))
                {
                    Debug.Log("Output clicked!!");
                    outputLinkNode = node;
                    outputLinkNode.IfTrueNode.isChanging = true;
                    currentState = EDialogueStates.DecisionLinkTrue;
                    return;
                }

                // update position
                outputBtnPos.y += 26 * zoomScale;

                // create label field
                EditorGUI.LabelField(
                    new Rect(new Vector2(outputBtnPos.position.x - 20, outputBtnPos.position.y), outputBtnPos.size),
                    "False", EdStylesExtra.TextStyle(Color.white, false, TextAnchor.MiddleRight,
                        (int)Mathf.Ceil(12 * zoomScale))); // create false button

                if (GUI.Button(outputBtnPos, "", EdStylesExtra.ButtonStyle(node.IfFalseNode?.node != null
                        ? "d_winbtn_mac_close@2x"
                        : "sv_icon_dot0_pix16_gizmo")))
                {
                    Debug.Log("Output clicked!!");
                    outputLinkNode = node;
                    outputLinkNode.IfFalseNode.isChanging = true;
                    currentState = EDialogueStates.DecisionLinkFalse;
                    return;
                }
            }
        }

        internal void DrawLinkConnections(INodes node, float zoomScale = 1)
        {
            if (node.NodeType is NodeType.End or NodeType.LinkNode) return;

            Vector2 linePosStart = Vector2.zero;
            Vector2 linePosEnd = Vector2.zero;

            if (node.NodeType == NodeType.DecisionNode)
            {
                if (node.IfTrueNode?.node != null)
                {
                    Handles.color = Color.green;
                    linePosStart = new Vector2(node.NodeRect.xMax * zoomScale, (node.NodeRect.yMax - 40) * zoomScale);
                    linePosEnd = new Vector2(node.IfTrueNode.node.NodeRect.position.x * zoomScale,
                        (node.IfTrueNode.node.NodeRect.center.y + 5) * zoomScale);
                    Line(linePosStart, linePosEnd, _pointOffset * zoomScale);
                }

                if (node.IfFalseNode?.node != null)
                {
                    Handles.color = Color.red;
                    linePosStart = new Vector2(node.NodeRect.xMax * zoomScale, (node.NodeRect.yMax - 10) * zoomScale);
                    linePosEnd = new Vector2(node.IfFalseNode.node.NodeRect.position.x * zoomScale,
                        (node.IfFalseNode.node.NodeRect.center.y + 5) * zoomScale);
                    Line(linePosStart, linePosEnd, _pointOffset * zoomScale);
                }

                return;
            }

            if (!node.ChildIdList.Any()) return;
            float childrenQty = node.ChildIdList.Count;
            var index = 0;
            foreach (var child in node.ChildIdList)
            {
                Handles.color = Color.white;
                switch (node.NodeType)
                {
                    case NodeType.RootNode:
                    case NodeType.SayNode:
                        linePosStart = new Vector2(node.NodeRect.xMax * zoomScale,
                            (node.NodeRect.yMax - 28) * zoomScale);
                        linePosEnd = new Vector2(child.NodeRect.position.x * zoomScale,
                            (child.NodeRect.center.y + 5) * zoomScale);
                        break;

                    case NodeType.ChoiceNode:
                        if (node.Choices.Count > index)
                        {
                            linePosStart = new Vector2(node.NodeRect.xMax * zoomScale,
                                (node.NodeRect.yMax - 20) * zoomScale);
                            linePosEnd = new Vector2(child.NodeRect.position.x * zoomScale,
                                (child.NodeRect.center.y + 5) * zoomScale);

                            Handles.color = Color.Lerp(Color.green, Color.blue,
                                Mathf.InverseLerp(0.2f, 0.9f, index / childrenQty));
                            var txtLength = node.Choices[index].Length;
                            var txt = node.Choices[index].Remove(txtLength > 5 ? 5 : txtLength,
                                txtLength > 5 ? (txtLength - 5) : 0);
                            Handles.Label((linePosStart + linePosEnd) * 0.5f, $"{index}:{txt} ");
                        }

                        break;

                    default:
                        linePosStart = new Vector2(node.NodeRect.xMax * zoomScale,
                            (node.NodeRect.yMax - 20) * zoomScale);
                        linePosEnd = new Vector2(child.NodeRect.position.x * zoomScale,
                            (child.NodeRect.center.y + 5) * zoomScale);
                        break;
                }

                Line(linePosStart, linePosEnd, _pointOffset * zoomScale);

                index++;
            }
        }

        public static void Line(Vector2 linePosStart, Vector2 linePosEnd,
            float pointOffset = 15, float lineWidth = 2.5f)
        {
            var controlPointOffset = Vector2.one;
            controlPointOffset.x *= pointOffset;

            _linePoints = new Vector3[6]
            {
                linePosStart,
                linePosStart + controlPointOffset,
                linePosStart + controlPointOffset,
                linePosEnd - controlPointOffset,
                linePosEnd - controlPointOffset,
                linePosEnd
            };

            Handles.DrawAAPolyLine(lineWidth, 6, _linePoints);
        }
    }
}
#endif