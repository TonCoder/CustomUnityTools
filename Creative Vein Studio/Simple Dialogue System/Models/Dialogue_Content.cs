using System;
using TMPro;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Models
{
    [Serializable]
    public class DialogueContent
    {
        [SerializeField] internal TextMeshProUGUI characterName;
        [SerializeField] internal TextMeshProUGUI dialogueText;
        [SerializeField] internal GameObject continueIndicator;
    }
}