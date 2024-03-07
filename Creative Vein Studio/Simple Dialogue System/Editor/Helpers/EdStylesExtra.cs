using System;
using CreativeVeinStudio.Simple_Dialogue_System.Enums;
using UnityEditor;
using UnityEngine;

namespace CreativeVeinStudio.Simple_Dialogue_System.Editor.Helpers
{
    [Serializable]
    public class EdStylesExtra : UnityEditor.Editor
    {
        public virtual Gradient GetNoodleGradient(Color? from, Color? to)
        {
            Gradient grad = new Gradient();

            Color a = Color.Lerp(from ??= Color.green, to ??= Color.white, 0.8f);
            Color b = Color.Lerp(from ??= Color.green, to ??= Color.white, 0.8f);

            grad.SetKeys(
                new GradientColorKey[] { new GradientColorKey(a, 0f), new GradientColorKey(b, 1f) },
                new GradientAlphaKey[] { new GradientAlphaKey(1f, 0f), new GradientAlphaKey(1f, 1f) }
            );

            return grad;
        }

        public static void HorizontalLine() => EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        public static GUIStyle Title => TextStyle(Color.white, true, TextAnchor.UpperCenter);

        public static GUIStyle TextStyle(Color textColor, bool isBold, TextAnchor textAnchor = TextAnchor.UpperLeft,
            int fontSize = 12) =>
            new GUIStyle
            {
                alignment = textAnchor,
                normal =
                {
                    textColor = textColor,
                },
                fontSize = fontSize,
                richText = true,
                fontStyle = isBold ? FontStyle.Bold : FontStyle.Normal,
            };

        public static GUIStyle TextField(Color textColor, Texture2D background, Vector4 padding, Vector4 margin) =>
            new GUIStyle
            {
                stretchWidth = true,
                padding = new RectOffset((int)padding.x, (int)padding.y, (int)padding.z, (int)padding.w),
                margin = new RectOffset((int)margin.x, (int)margin.y, (int)margin.z, (int)margin.w),
                normal = { textColor = textColor, background = background }
            };

        public static GUIStyle CenteredBoldWhiteText(int fontSize) => new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            normal =
            {
                textColor = Color.white,
            },
            richText = true,
            fontStyle = FontStyle.Bold,
            fontSize = fontSize
        };

        public static GUIStyle CustomNodeStyle(Texture2D nodeBg, int[] border, int[] padding, Color textColor) =>
            new GUIStyle
            {
                normal =
                {
                    background = nodeBg,
                    textColor = textColor
                },
                padding = new RectOffset(border[0], border[1], border[2], border[3]),
                border = new RectOffset(padding[0], padding[1], padding[2], padding[3]),
            };

        public static GUIStyle CustomNodeStyle(ENodeBgType eNodeBg, int[] border, int[] padding, Color textColor) =>
            new GUIStyle
            {
                normal =
                {
                    background = EditorGUIUtility.Load(eNodeBg.ToString()) as Texture2D,
                    textColor = textColor
                },
                padding = new RectOffset(border[0], border[1], border[2], border[3]),
                border = new RectOffset(padding[0], padding[1], padding[2], padding[3]),
            };

        public static GUIStyle ButtonStyle() => new GUIStyle(EditorStyles.miniButtonMid)
        {
            fontStyle = FontStyle.Bold,
            fontSize = 15,
            fixedHeight = 20
        };

        public static GUIStyle ButtonStyle(Color bgColor, Color textColor, float height = 40, float width = 80) =>
            new GUIStyle(EditorStyles.miniButtonMid)
            {
                normal =
                {
                    background = AddTextureColor(new Texture2D(100, 100), bgColor),
                    textColor = textColor
                },
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                fontSize = 15,
                fixedWidth = width,
                fixedHeight = height,
                onHover = new GUIStyleState()
                {
                    background = AddTextureColor(new Texture2D(100, 100),
                        new Color(0.38f, 0.38f, 0.38f))
                }
            };

        public static GUIStyle ButtonStyle(float width, float height, string iconName, bool stretchWidth = true) =>
            new GUIStyle
            {
                normal =
                {
                    background = iconName.ToLower().Contains("icon")
                        ? EditorGUIUtility.IconContent(iconName).image as Texture2D
                        : EditorGUIUtility.FindTexture(iconName)
                },
                stretchWidth = stretchWidth,
                stretchHeight = true,
                fixedWidth = width,
                fixedHeight = height,
            };

        public static GUIStyle ButtonStyle(string iconName) =>
            new GUIStyle
            {
                normal =
                {
                    background = iconName.ToLower().Contains("icon")
                        ? EditorGUIUtility.IconContent(iconName).image as Texture2D
                        : EditorGUIUtility.FindTexture(iconName)
                },
                stretchWidth = true,
                stretchHeight = true
            };

        public static GUIStyle ButtonStyle(float height, string iconName) =>
            new GUIStyle
            {
                normal =
                {
                    background = iconName.ToLower().Contains("icon")
                        ? EditorGUIUtility.IconContent(iconName).image as Texture2D
                        : EditorGUIUtility.FindTexture(iconName)
                },
                stretchWidth = true,
                fixedHeight = height,
            };

        public static GUIStyle ButtonStyle(float width, float height, Color bgColor, Color textColor, bool stretchWidth)
        {
            return new GUIStyle(EditorStyles.miniButton)
            {
                normal =
                {
                    background = AddTextureColor(new Texture2D(100, 100), bgColor),
                    textColor = textColor
                },
                alignment = TextAnchor.MiddleCenter,
                stretchWidth = stretchWidth,
                fixedWidth = width,
                fixedHeight = height,
                fontStyle = FontStyle.Bold
            };
        }

        public static GUIStyle ButtonStyle(float height, Color bgColor, Color textColor)
        {
            return new GUIStyle(EditorStyles.miniButton)
            {
                normal =
                {
                    background = AddTextureColor(new Texture2D(100, 100), bgColor),
                    textColor = textColor
                },
                alignment = TextAnchor.MiddleCenter,
                stretchWidth = true,
                fixedHeight = height,
                fontStyle = FontStyle.Bold
            };
        }

        public static GUIContent GetIcon(string iconName, string text = "", string tooltip = "")
        {
            return new GUIContent(text, iconName.Trim().ToLower().Contains("icon")
                ? EditorGUIUtility.IconContent(iconName).image as Texture2D
                : EditorGUIUtility.FindTexture(iconName), tooltip);
        }

        public static Texture2D AddTextureColor(Texture2D texture, Color color)
        {
            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width; x++)
                {
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
            return texture;
        }

        public static int[] BorderAndPadding { get; } = new[] { 11, 11, 11, 11 };

        public static GUIStyle NodeAreaStyle(ENodeBgType _bgType) =>
            CustomNodeStyle(_bgType,
                BorderAndPadding,
                BorderAndPadding,
                Color.white);

        public static GUIStyle AlignText(TextAnchor anchor) => new GUIStyle() { alignment = anchor };

        public static class Colors
        {
            public static Color ActiveColor { get; } = new Color(0.3f, 0.66f, 0.4f);
            public static Color BaseColor { get; } = new Color(0.38f, 0.37f, 0.37f);
            public static Color PanelColor { get; } = new Color(0.3f, 0.31f, 0.32f, 0.9f);
        }

        public static class Icons
        {
            public static GUIContent AlignVertical { get; } =
                EditorGUIUtility.IconContent("align_vertically_bottom_active");

            public static GUIContent DialogueIcon { get; } = GetIcon("DefaultAsset Icon");
            public static GUIContent AudioIcon { get; } = GetIcon("Microphone Icon");
            public static GUIContent EventIcon { get; } = GetIcon("d_EventSystem Icon");
            public static GUIContent Dropdown { get; } = GetIcon("d_icon dropdown@2x");

            public static GUIContent PlayIcon { get; } = EdStylesExtra.GetIcon("d_PlayButton On@2x");
            public static GUIContent StopIcon { get; } = EdStylesExtra.GetIcon("d_PreMatQuad@2x");
        }
    }
}