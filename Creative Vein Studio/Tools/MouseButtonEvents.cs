using CVStudio;
using UnityEngine;
using UnityEngine.Events;

namespace Creative_Vein_Studio.Tools
{
    public class MouseButtonEvents : MonoBehaviour
    {
        public UnityEvent onLeftMouseButtonUp; // Set this in the Inspector
        public UnityEvent onLeftMouseButtonDown; // Set this in the Inspector
        public UnityEvent onMiddleMouseButtonDown; // Set this in the Inspector
        public UnityEvent onRightMouseButtonDown; // Set this in the Inspector
        public UEvents.EFloat onMouseScroll; // Set this in the Inspector

        private Vector2 scroll;


        // Enum for mouse button indices
        private enum MouseButton
        {
            LeftButton = 0,
            RightButton = 1,
            MiddleButton = 2
        }

        void Update()
        {
            scroll = Input.mouseScrollDelta;
            if (Input.GetMouseButtonDown((int)MouseButton.LeftButton))
            {
                onLeftMouseButtonDown.Invoke();
            }
            else if (Input.GetMouseButtonUp((int)MouseButton.LeftButton))
            {
                onLeftMouseButtonUp.Invoke();
            }
            else if (Input.GetMouseButtonDown((int)MouseButton.MiddleButton))
            {
                onMiddleMouseButtonDown.Invoke();
            }
            else if (Input.GetMouseButtonDown((int)MouseButton.RightButton))
            {
                onRightMouseButtonDown.Invoke();
            }
            else if (scroll != Vector2.zero)
            {
                onMouseScroll?.Invoke(scroll.y);
            }
        }
    }
}