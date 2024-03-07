using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CreativeVeinStudio.Simple_Dialogue_System.Models
{
    [System.Serializable]
    public class CanvasSettings
    {
        [FormerlySerializedAs("canvasBg")] [SerializeField]
        private Texture2D canvasBgTex;

        [SerializeField] private Rect textureCords;
        [SerializeField] private Rect propertyPanelRect = new Rect(Vector2.zero, new Vector2(310, 160));
        [SerializeField] private Vector2 scrollPosition;
        [SerializeField] private Vector2 mousePos = Vector2.zero;
        [SerializeField] private string path = "Assets/CreativeVeinStudio/Example Scenes/Dialogue Content/Objects/";

        public const float CanvasSize = 16000f;
        public const int BackgroundSize = 50;

        public string Path
        {
            get => path;
            set => path = value;
        }

        public Rect PropertyPanelRect
        {
            get => propertyPanelRect;
            set => propertyPanelRect = value;
        }

        public Vector2 ScrollPosition
        {
            get => scrollPosition;
            set => scrollPosition = value;
        }

        public Vector2 MousePos
        {
            get => mousePos;
            set => mousePos = value;
        }


        public Rect TextureCords => textureCords;
        public Texture2D CanvasBgTex => canvasBgTex;

        public void Init()
        {
            this.propertyPanelRect = new Rect(Vector2.zero, new Vector2(310, 160));
            this.textureCords = new Rect(0, 0, CanvasSize / BackgroundSize, CanvasSize / BackgroundSize);
            this.canvasBgTex = Resources.Load("background") as Texture2D;
        }
    }

    [System.Serializable]
    public class NodeCanvasSettings
    {
        [SerializeField] private Texture2D rootBg;
        [SerializeField] private Texture2D sayBg;
        [SerializeField] private Texture2D choiceBg;
        [SerializeField] private Texture2D decisionBg;
        [SerializeField] private Texture2D endBg;
        [SerializeField] private Texture2D linkBg;
        [SerializeField] private List<Vector2> inputPos;
        [SerializeField] private List<Vector2> outputPos;

        public readonly Rect nodeLabelPosition = new Rect(5, -80, 50, 15);
        public Texture2D RootBg => rootBg;
        public Texture2D SayBg => sayBg;
        public Texture2D ChoiceBg => choiceBg;
        public Texture2D DecisionBg => decisionBg;
        public Texture2D EndBg => endBg;
        public Texture2D LinkBg => linkBg;

        public List<Vector2> InputPos
        {
            get => inputPos;
            set => inputPos = value;
        }

        public List<Vector2> OutputPos
        {
            get => outputPos;
            set => outputPos = value;
        }

        public void Init()
        {
            this.rootBg = Resources.Load("RootBG") as Texture2D;
            this.sayBg = Resources.Load("SayBG") as Texture2D;
            this.choiceBg = Resources.Load("ChoiceBG") as Texture2D;
            this.decisionBg = Resources.Load("DecisionBG") as Texture2D;
            this.endBg = Resources.Load("EndBG") as Texture2D;
            this.linkBg = Resources.Load("LinkBG") as Texture2D;

            InputPos = new List<Vector2>();
            OutputPos = new List<Vector2>();
        }
    }
}