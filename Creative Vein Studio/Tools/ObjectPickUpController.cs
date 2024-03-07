using Creative_Vein_Studio.Tools.Scriptable_Objects;
using UnityEngine;
using EPOOutline;
using UnityEngine.Events;


namespace Creative_Vein_Studio.Tools
{
    public class ObjectPickUpController : MonoBehaviour, IInteractableActions
    {
        public bool canRotateObject = false;
        public bool canPickThisUp = false;
        public Outlinable _outlinable;
        public Rigidbody _rigidbody;
        public Collider collider;
        public Vector3 pickUpOffset = Vector3.zero;
        public Vector3 rotationOffset = Vector3.zero;
        [Range(0, 1)] public float smoothTime = 0.3F; // Set this in the Inspector to your desired smooth time
        public SoPickUpBroker SoPickUpBroker;

        public UnityEvent onPickedUp;
        public UnityEvent onDropped;

        private ITopDownCursorActions _cursorActions;

        private Vector3 _velocity = Vector3.zero;
        private const string INTERACT_TAG = "playerHand";

        private void Start()
        {
            _outlinable ??= GetComponent<Outlinable>();
            if (canRotateObject && SoPickUpBroker != null)
            {
                SoPickUpBroker.OnRotateObject += RotateObject;
            }
        }

        private void OnDisable()
        {
            if (SoPickUpBroker != null)
                SoPickUpBroker.OnRotateObject -= RotateObject;
        }

        public string ObjName => name;
        public int ID => gameObject.GetInstanceID();

        public bool Grabbed { get; set; }

        public bool CanPickUp
        {
            get => canPickThisUp;
            set => canPickThisUp = value;
        }

        public void Follow(Transform objToFollow)
        {
            Vector3 targetPosition = objToFollow.position + pickUpOffset;
            transform.rotation = Quaternion.Euler(rotationOffset);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
        }

        private void RotateObject(float rotation)
        {
            if (Grabbed)
                rotationOffset.y += rotation;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(INTERACT_TAG) || !CanPickUp) return;
            _cursorActions ??= other.GetComponentInParent<ITopDownCursorActions>();
            if (_cursorActions is { ItemOnHand: true }) return;
            _cursorActions?.SetObjectToGrab(this);
            ToggleHighlight(true);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(INTERACT_TAG) || Grabbed) return;
            _cursorActions?.SetObjectToGrab(null);
            ToggleHighlight(false);
        }

        public void Drop()
        {
            onDropped?.Invoke();
            if (_rigidbody) _rigidbody.useGravity = true;
            if (collider) collider.enabled = true;
            Grabbed = false;
            _cursorActions?.SetObjectToGrab(null);
        }

        public void PickUp()
        {
            onPickedUp?.Invoke();
            if (_rigidbody) _rigidbody.useGravity = false;
            if (collider) collider.enabled = false;
            Grabbed = true;
        }

        private void ToggleHighlight(bool b)
        {
            if (_outlinable) _outlinable.enabled = b;
        }
    }


    public interface IInteractableActions
    {
        int ID { get; }
        void Follow(Transform followObject);
        bool CanPickUp { get; set; }
        bool Grabbed { get; set; }
        string ObjName { get; }
        public void Drop();
        void PickUp();
    }
}