using System;
using System.Collections.Generic;
using System.Linq;
using CreativeVeinStudio.Simple_Dialogue_System.Attributes;
using CreativeVeinStudio.Simple_Dialogue_System.Interface;
using CreativeVeinStudio.Simple_Dialogue_System.Models;
using CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects.Nodes;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CreativeVeinStudio.Simple_Dialogue_System.Tools
{
    [RequireComponent(typeof(AudioSource))]
    public sealed class ConversationController : MonoBehaviour
    {
        #region Exposed Variables

        [FormerlySerializedAs("dialogueAsset")] [FieldTitle("General setup")] [SerializeField]
        private SO_Dialogue _dialogueAsset;

        [SerializeField] private TextDisplayController textDisplayDisplayController;

        [Tooltip("If a choice NODE next in the list, it will automatically continue to displaying the choices")]
        [Space]
        [SerializeField]
        private bool continueToChoices;

        [SerializeField] private bool sendValuesAsEvent = false;
        [SerializeField] private int maxChoiceBtnQty = 4;

        [FieldTitle("UI setup")] [Space] [SerializeField]
        private DialogueContent uiSetup;

        [Space] [SerializeField] private GameObject choiceContainer;
        [SerializeField] private GameObject choiceBtnPrefab;

        #endregion

        private INodeProps ActiveNode { get; set; }
        private INodeProps PrevNode { get; set; }
        private readonly List<ChoiceBtnModel> _choiceButtons = new List<ChoiceBtnModel>();
        private bool _isMakingChoice;
        private bool _stillTalking = false;
        private ConversationStates convoState;
        private const string RanCountText = "RanQty";
        private const string DefaultCharacterName = "Enter NAME";

        #region Unity Functions

        internal void OnEnable()
        {
            if (uiSetup.continueIndicator != null) uiSetup.continueIndicator.SetActive(false);
            convoState = ConversationStates.IsWaiting;
        }

        internal void Start()
        {
            if (_dialogueAsset == null)
            {
#if UNITY_EDITOR
                Debug.Log($"Please provide an SO_Dialogue to the current Conversation Controller on {transform.name}");
#endif
            }

            Init();
        }

        #endregion

        #region Functions

        private void Init()
        {
            for (int i = 0; i < maxChoiceBtnQty; i++)
            {
                var btn = Instantiate(choiceBtnPrefab, choiceContainer.transform).GetComponent<Button>();
                _choiceButtons.Add(new ChoiceBtnModel(btn));
            }

            _choiceButtons.ForEach(x => x.ToggleEnable(false));
        }

        /// <summary>
        /// Will let you start a conversation. NOTE: Conversations will always start with Rootnodes.
        /// Provide the index of the root to begin that branch
        /// </summary>
        /// <param name="rootIndex"></param>
        public void StartConversation(int rootIndex)
        {
            var node = _dialogueAsset.GetAllNodes.Cast<INodeProps>().FirstOrDefault(node =>
                node.NodeType == NodeType.RootNode && node.Index == rootIndex) as SO_RootNode;
            if (node == null)
            {
#if UNITY_EDITOR

                Debug.Log("Start Conversation FAILED. NO dialogueText with the index provided found");
                Debug.Break();
#endif
                return;
            }

            ActiveNode = node;
            PrevNode = node;
            convoState = ConversationStates.Talking;
            ProcessState();
        }

        /// <summary>
        /// Continue conversation will determine the next process to take depending on the node type
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void ContinueConversation()
        {
            if (ActiveNode == null)
            {
#if UNITY_EDITOR
                Debug.Log("No active node to process");
#endif
                return;
            }

            if (convoState == ConversationStates.SelectingChoices) return;

            if (textDisplayDisplayController != null && !textDisplayDisplayController.FullTextShowed)
            {
                if (ActiveNode.NodeType is NodeType.RootNode or NodeType.SayNode)
                {
                    textDisplayDisplayController.CompleteText();
                    return;
                }
            }

            convoState = ActiveNode.NodeType switch
            {
                NodeType.ChoiceNode => ConversationStates.PresentChoices,
                NodeType.RootNode => ConversationStates.Talking,
                NodeType.SayNode => ConversationStates.Talking,
                NodeType.LinkNode => ConversationStates.Link,
                NodeType.End => ConversationStates.EndConversation,
                NodeType.DecisionNode => ConversationStates.ProcessDecision,
                _ => throw new ArgumentOutOfRangeException()
            };

            ProcessState();
        }

        private void ProcessState()
        {
            while (true)
            {
                switch (convoState)
                {
                    case ConversationStates.IsWaiting:
                    case ConversationStates.SelectingChoices:
                        return;

                    case ConversationStates.PresentChoices:
                        ShowChoices();
                        convoState = ConversationStates.SelectingChoices;
                        return;

                    case ConversationStates.Talking:
                        ProceedConversation();
                        return;

                    case ConversationStates.Link:
                        ActiveNode = ActiveNode.ChildIdList.Cast<INodeProps>().First();
                        ContinueConversation();
                        break;

                    case ConversationStates.DoneSpeaking:
                        _stillTalking = false;

                        if (ActiveNode.HasEndEvent)
                        {
                            ConversationBroker.OnRunEndEvent(ActiveNode.EventName, ActiveNode.EventArgs);
                        }

                        if (!ActiveNode.ChildIdList.Any())
                        {
                            convoState = ConversationStates.EndConversation;
                            continue;
                        }

                        PrevNode = ActiveNode;
                        ActiveNode = ActiveNode.ChildIdList.Cast<INodeProps>().First();
                        // if (ActiveNode.NodeType is NodeType.LinkNode or NodeType.DecisionNode)
                        // {
                        //     ContinueConversation();
                        //     return;
                        // }

                        if (ActiveNode.NodeType is NodeType.ChoiceNode && continueToChoices)
                        {
                            ContinueConversation();
                            return;
                        }

                        convoState = ConversationStates.IsWaiting;
                        continue;
                    case ConversationStates.EndConversation:
                        if (ActiveNode.HasEndEvent)
                        {
                            ConversationBroker.OnRunEndEvent(ActiveNode.EventName, ActiveNode.EventArgs);
                        }

                        if (sendValuesAsEvent)
                        {
                            ConversationBroker.OnEndConvo();
                        }

                        PrevNode = null;
                        ActiveNode = null;
                        return;
                    case ConversationStates.ProcessDecision:
                        var decision = (SO_DecisionNode)ActiveNode;
                        var isValid = false;
                        if (decision.decision.variable1.value.Contains(RanCountText) ||
                            decision.decision.variable1.value.Contains(RanCountText))
                        {
                            isValid = decision.ProcessDecision(_dialogueAsset, PrevNode.RanQty);
                        }
                        else
                        {
                            isValid = decision.ProcessDecision(_dialogueAsset);
                        }

                        if (isValid)
                        {
                            ActiveNode = (INodeProps)ActiveNode.IfTrueNode.node;
                        }
                        else
                        {
                            ActiveNode = (INodeProps)ActiveNode.IfFalseNode.node;
                        }

                        ContinueConversation();
                        break;
                    default:
                        Debug.LogWarning("Nothing running");
                        return;
                }

                break;
            }
        }

        private void ProceedConversation()
        {
            try
            {
                if (_stillTalking) return;
                _stillTalking = true;
                if (uiSetup.continueIndicator != null) uiSetup.continueIndicator.SetActive(false);

                // check if there are events and run them
                if (ActiveNode is { HasStartEvent: true })
                {
                    ConversationBroker.OnRunStartEvent(ActiveNode.EventName, ActiveNode.EventArgs);
                }

                // if its a say / root node then process convo
                if (ActiveNode.NodeType is NodeType.RootNode or NodeType.SayNode)
                {
                    ActiveNode.RanQty++;
                    if (sendValuesAsEvent)
                    {
                        ConversationBroker.OnStartConvo(ActiveNode.GetCharacterSpeaking(),
                            ActiveNode.GetDialogue());
                    }
                    else
                    {
                        SetUIValues(ActiveNode.GetCharacterSpeaking()
                            , ActiveNode.GetDialogue());
                    }

                    convoState = ConversationStates.IsWaiting;
                    ProcessState();
                }
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.LogException(e);
#endif

                throw;
            }
        }


        private void SetUIValues(string characterName, string dialogue)
        {
            characterName = characterName.Contains(DefaultCharacterName) || string.IsNullOrEmpty(characterName)
                ? _dialogueAsset.name
                : characterName;

            if (sendValuesAsEvent)
                ConversationBroker.OnStartConvo(characterName, dialogue);
            else
            {
                if (uiSetup.characterName)
                    uiSetup.characterName.text = characterName;

                if (textDisplayDisplayController)
                {
                    uiSetup.dialogueText.text = string.Empty;
                    textDisplayDisplayController.QueueText(dialogue);
                    textDisplayDisplayController.StartTyper(uiSetup.dialogueText, SentenceEnded);
                    return;
                }

                if (uiSetup.dialogueText)
                    uiSetup.dialogueText.text = dialogue;
            }

            SentenceEnded();
        }

        private void SentenceEnded()
        {
            if (uiSetup.continueIndicator) uiSetup.continueIndicator.SetActive(true);
            convoState = ConversationStates.DoneSpeaking;
            ProcessState();
        }

        private void ShowChoices()
        {
            var index = 0;
            foreach (var child in ActiveNode.ChildIdList.Cast<INodeProps>())
            {
                _choiceButtons[index].textContent.text = ((SO_ChoicesNode)ActiveNode).Choices[index];
                _choiceButtons[index].btnGo.onClick.AddListener(delegate { ChoiceSelectedEvent(child); });
                _choiceButtons[index].ToggleEnable(true);
                index++;
            }
        }

        private void ChoiceSelectedEvent(INodeProps node)
        {
            _choiceButtons.ForEach(x =>
            {
                x.btnGo.onClick.RemoveAllListeners();
                x.ToggleEnable(false);
            });

            ActiveNode = node;
            convoState = ConversationStates.IsWaiting;
            ContinueConversation();
        }

        #endregion
    }

    public static class ConversationBroker
    {
        #region Events Delegates

        public static event Action<string, List<string>> RunStartEvent;
        public static event Action<string, List<string>> RunEndEvent;

        public static void OnRunStartEvent(string eventName, List<string> args)
        {
            RunStartEvent?.Invoke(eventName, args);
        }

        public static void OnRunEndEvent(string eventName, List<string> args)
        {
            RunEndEvent?.Invoke(eventName, args);
        }

        #endregion


        #region Conversation Delegates

        public static event Action<string, string> StartConvo;
        public static event Action EndConvo;

        public static void OnStartConvo(string speaker, string dialogue)
        {
            StartConvo?.Invoke(speaker, dialogue);
        }

        public static void OnEndConvo()
        {
            EndConvo?.Invoke();
        }

        #endregion
    }

    [Serializable]
    public enum ConversationStates
    {
        IsWaiting,
        PresentChoices,
        SelectingChoices,
        Talking,
        DoneSpeaking,
        EndConversation,
        ProcessDecision,
        Link
    }
}