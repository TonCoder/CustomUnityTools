using System;
using System.Text.RegularExpressions;
using CreativeVeinStudio.Simple_Dialogue_System.Abstracts;
using CreativeVeinStudio.Simple_Dialogue_System.Attributes;
using CreativeVeinStudio.Simple_Dialogue_System.Attributes.Helpers;
using CreativeVeinStudio.Simple_Dialogue_System.Enums;
using CreativeVeinStudio.Simple_Dialogue_System.Interface;
using CreativeVeinStudio.Simple_Dialogue_System.Models;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Scriptable_Objects.Nodes
{
    [System.Serializable]
    public class SO_RootNode : ABaseNode, INodeProps
    {
        [SerializeField, ShowVarList] private VariableTypes speaking = new VariableTypes();
        [SerializeField] private Rect rect = new Rect(10, 10, 135, 65);
        [SerializeField, TextArea(40, 500)] internal string dialogue;

        private Rect rectMax = new Rect(10, 10, 213, 200);
        private Rect rectShrink = new Rect(10, 20, 125, 70);

        private string fixedDialogue;
        private VariableTypes fixedSpeaking = new VariableTypes();

        private void OnDisable()
        {
            fixedSpeaking = new VariableTypes();
            fixedDialogue = "";
        }

        public override Rect NodeRect => rect;

        public override Vector2 NodePos
        {
            get => rect.position;
            set => rect.position = rectShrink.position = rectMax.position = value;
        }

        public override string DialogueText => dialogue;

        public string GetDialogue()
        {
            if (!string.IsNullOrEmpty(fixedDialogue)) return fixedDialogue;
            return ProcessDialogue(parentDialogue.GlobalVars);
        }

        public string GetCharacterSpeaking()
        {
            if (speaking.varType == EVariableOptions.IsString)
                fixedSpeaking.value = speaking?.value ?? "";
            if (speaking.varType == EVariableOptions.IsLocal)
                fixedSpeaking.value = parentDialogue.GetValueByName(speaking.value)?.varValue ?? "";
            if (speaking.varType == EVariableOptions.IsGlobal)
                fixedSpeaking.value = parentDialogue.GlobalVars.GetValueByName(speaking.value)?.varValue ?? "";

            return fixedSpeaking.value;
        }

        /// <summary>
        /// Checks the dialogue for parameters to be replaced with their corresponding global values
        /// </summary>
        /// <param name="dialogue"></param>
        /// <returns>Returns the adjusted dialogue</returns>
        /// <exception cref="NotImplementedException"></exception>
        private string ProcessDialogue(SO_GlobalVars globalVars)
        {
            const string pattern = @"\[(.*?)\]";
            fixedDialogue = dialogue;
            MatchCollection matches = Regex.Matches(fixedDialogue, pattern);

            for (int i = 0; i < matches.Count; i++)
            {
                var replaceWithVal =
                    globalVars.GetValueByName(matches[i].ToString().Replace("[", "").Replace("]", "").Trim());
                fixedDialogue = fixedDialogue.Replace(matches[i].ToString(), replaceWithVal.varValue);
            }

            return fixedDialogue;
        }
    }
}