using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace CreativeVeinStudio.Simple_Dialogue_System.Tools
{
    [Serializable, RequireComponent(typeof(AudioSource))]
    public class TextDisplayController : MonoBehaviour
    {
        [SerializeField, Range(1f, 10f)] private float showSpeed = 5f;

        [SerializeField] private AudioClip typingSound;
        // [Space] [SerializeField] private UnityEvent onStartShow;
        // [SerializeField] private UnityEvent onEndShow;

        #region Private Variables

        private Queue<char> _queuedText;
        private string _fullText;
        private bool _completeText = false;
        private bool _fullTextShowed = false;
        private AudioSource _audioSource;

        #endregion


        public bool FullTextShowed => _fullTextShowed;

        public float TextDisplaySpeed
        {
            get => Mathf.InverseLerp(0.1f, 0.01f, (showSpeed * 0.01f));
            set => showSpeed = value;
        }

        private void Start()
        {
            if (typingSound != null)
            {
                _audioSource = GetComponent<AudioSource>();
                _audioSource.clip = typingSound;
            }
        }

        public void QueueText(string text)
        {
            _fullText = text;
            _queuedText = new Queue<char>(_fullText.ToCharArray());
        }

        public void StartTyper(TextMeshProUGUI textField, Action completeCallback = null)
        {
            // onStartShow?.Invoke();
            _fullTextShowed = false;
            StartCoroutine(DeQueue(textField, completeCallback));
        }

        public void CompleteText() => _completeText = true;

        private IEnumerator DeQueue(TMP_Text textField, Action cb = null)
        {
            while (_queuedText.Count > 0)
            {
                if (_completeText)
                {
                    textField.text = _fullText;
                    _fullTextShowed = true;
                    _completeText = false;
                    cb?.Invoke();
                    yield break;
                }

                textField.text += _queuedText.Dequeue();
                if (typingSound != null) _audioSource.Play();
                yield return new WaitForSeconds(TextDisplaySpeed);
            }

            _queuedText.Clear();
            _fullTextShowed = true;
            cb?.Invoke();
            // onEndShow?.Invoke();
        }
    }
}