using System;
using CreativeVeinStudio.Simple_Dialogue_System.Abstracts;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects.Nodes
{
    [Serializable]
    public class DecisionNodeSelection
    {
        [SerializeField] public ABaseNode node;
        [SerializeField] public bool isChanging;
    }
}