using System;
using System.Collections.Generic;
using CreativeVeinStudio.Simple_Dialogue_System.Abstracts;
using UnityEngine;
using UnityEngine.Events;

namespace CreativeVeinStudio.Simple_Dialogue_System.Models
{
    [Serializable]
    public class EventString : UnityEvent<string, string> { }

    [Serializable]
    public class NodeEventListener
    {
        [SerializeField] internal string name;
        [SerializeField] internal ABaseNode node;
        [SerializeField] internal EventString onStart;
        [SerializeField] internal EventString onEnd;
    }

    [System.Serializable]
    public class NodeEventList
    {
        [SerializeField] internal string name;
        [SerializeField] internal List<NodeEventListener> nodeEvents;
    }
}