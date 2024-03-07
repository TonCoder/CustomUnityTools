using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CreativeVeinStudio.Simple_Dialogue_System.Tools
{
    [Serializable]
    public class NodeEvent : UnityEvent<string, List<string>>
    {
    }

    public class NodeEventTrigger : MonoBehaviour
    {
        [SerializeField] private string eventName;

        [SerializeField, Tooltip("sends eventName, speaker, dialogue")]
        private NodeEvent start;

        [SerializeField, Tooltip("sends eventName, speaker, dialogue")]
        private NodeEvent end;

        private void Awake()
        {
            ConversationBroker.RunStartEvent += HandleOnStartEvent;
            ConversationBroker.RunEndEvent += HandleOnEndEvent;
        }

        private void OnDisable()
        {
            ConversationBroker.RunStartEvent -= HandleOnStartEvent;
            ConversationBroker.RunEndEvent -= HandleOnEndEvent;
        }

        private void HandleOnStartEvent(string eName, List<string> args)
        {
            if (eventName == eName)
            {
                start?.Invoke(eName, args);
            }
        }

        private void HandleOnEndEvent(string eName, List<string> args)
        {
            if (eventName == eName)
            {
                end?.Invoke(eName, args);
            }
        }
    }
}