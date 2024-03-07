using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CreativeVeinStudio.Simple_Dialogue_System.Models
{
    [System.Serializable]
    public class ChoiceBtnModel
    {
        [SerializeField] internal TextMeshProUGUI textContent;
        [SerializeField] internal Button btnGo;
        [SerializeField] internal Image img;

        public ChoiceBtnModel(Button btnObject)
        {
            this.textContent = btnObject.GetComponentInChildren<TextMeshProUGUI>();
            this.img = btnObject.GetComponent<Image>();
            this.btnGo = btnObject;
        }

        public void ToggleEnable(bool enable)
        {
            this.textContent.enabled = enable;
            this.img.enabled = enable;
            this.btnGo.enabled = enable;
        }
    }
}