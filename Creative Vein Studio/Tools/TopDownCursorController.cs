using _Main_App.Scripts.brokers;
using Cinemachine;
using UnityEngine;

namespace Creative_Vein_Studio.Tools
{
    public class TopDownCursorController : MonoBehaviour, ITopDownCursorActions
    {
        [SerializeField] public LayerMask GroundLayerMask; // Set this in the Inspector
        [SerializeField] private Camera cam;
        [SerializeField] private CinemachineVirtualCamera virtualCam;
        [SerializeField] private float minZoom = 12;
        [SerializeField] private float maxZoom = 20;

        [SerializeField] private SoBrokerHandControl handBroker;
        [SerializeField] private HandAnimationManager handAnim;
        [SerializeField] private float smoothTime = 0.3F; // Set this in the Inspector to your desired smooth time
        [SerializeField] private double pointerToClickDistance = 0.5f;
        [SerializeField] private Vector3 handOffset = new Vector3(0, 1, 0);

        // Set this in the Inspector to your desired raycast distance
        [SerializeField] private float raycastDistance = 100.0f;
        private Vector3 _checkUnderDirection; // Negative y direction
        private Vector3 _velocity = Vector3.zero;
        private Vector3 _mousePosition;
        private Vector3 _pointerPosition;
        private CinemachineFramingTransposer _virtualCamTransposer;
        private IInteractableActions ObjectToGrab { get; set; }

        public bool ItemOnHand { get; set; }
        public Transform HandPos => transform;

        private void Awake()
        {
            _virtualCamTransposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        }

        private void Update()
        {
            if (ObjectToGrab is { CanPickUp: true } && ItemOnHand)
            {
                ObjectToGrab.Follow(transform);
            }

            MovePointerToCursorPosition();
        }

        public void SetObjectToGrab(IInteractableActions obj)
        {
            if (ObjectToGrab?.ID == obj?.ID || ItemOnHand) return;
            ObjectToGrab = obj;
        }

        private void MovePointerToCursorPosition()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var hit, raycastDistance, GroundLayerMask))
            {
                _mousePosition = _pointerPosition = hit.point;
                _pointerPosition.y += handOffset.y; // Set the y position to the specified height
                _pointerPosition.x += handOffset.x; // Set the y position to the specified height
                _pointerPosition.z += handOffset.z; // Set the y position to the specified height
            }

            if (_pointerPosition == Vector3.zero) return;
            transform.position =
                Vector3.SmoothDamp(transform.position, _pointerPosition, ref _velocity, smoothTime);
        }

        // void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //     Gizmos.DrawRay(ray.origin, ray.direction * raycastDistance);
        // }

        public void ZoomScroll(float val)
        {
            if (val != 0)
            {
                _virtualCamTransposer.m_CameraDistance =
                    Mathf.Clamp(_virtualCamTransposer.m_CameraDistance + -val, minZoom, maxZoom);
            }
        }

        public void ProcessLeftClick()
        {
            if (Vector3.Distance(transform.position, _pointerPosition) > pointerToClickDistance) return;

            if (!ItemOnHand && ObjectToGrab is { CanPickUp: true })
            {
                handAnim.TriggerPickUpAnim();
                ObjectToGrab?.PickUp();
                ItemOnHand = true;
            }

            if (!ItemOnHand && ObjectToGrab == null)
            {
                handAnim.TriggerTapAnim(_mousePosition);
                SendPointerPosition();
            }
        }

        public void ProcessMouseUp()
        {
            ItemOnHand = false;
            handAnim.TriggerReleaseAnim(_mousePosition);
            ObjectToGrab?.Drop();
            ObjectToGrab = null;
        }

        public void ProcessRightClick()
        {
        }

        private void SendPointerPosition()
        {
            if (handBroker) handBroker.TriggerOnTapped(_mousePosition);
        }
    }

    public interface ITopDownCursorActions
    {
        Transform HandPos { get; }
        bool ItemOnHand { get; set; }
        void SetObjectToGrab(IInteractableActions obj);
    }
}