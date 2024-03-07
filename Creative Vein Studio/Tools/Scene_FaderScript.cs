using System;
using MAIN_PROJECT._Scripts.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Scene_FaderScript : MonoBehaviour
{
    [SerializeField, Range(.1f, 5)] private float fadeTime = 0.5f;
    [SerializeField, Range(0f, 5)] private float delayTimer = 0;
    [SerializeField] private bool _startFadeOut = false;
    [SerializeField] private bool _startFadeIn = false;
    [SerializeField] private Image _fadePanel;
    [SerializeField] private Color _currentColor = Color.black;
    [Space(15)] [SerializeField] private UnityEvent beforeRunEvent;
    [SerializeField] private UnityEvent fadeOutDoneEvent;
    [SerializeField] private UnityEvent fadeInDoneEvent;

    private bool _isFading = false;
    private float alphaChange;

    private SimpleTimer DelayTimer = new SimpleTimer(1, true);

    private void Awake()
    {
        FadeBroker.OnFadeOut += StartFadeOut;
        FadeBroker.OnFadeIn += StartFadeIn;

        if (delayTimer > 0) DelayTimer.SetWaitTime(delayTimer);
        beforeRunEvent?.Invoke();
    }

    private void OnDisable()
    {
        FadeBroker.OnFadeOut -= StartFadeOut;
        FadeBroker.OnFadeIn -= StartFadeIn;
    }

    // Update is called once per frame
    private void Update()
    {
        DelayTimer.TickOneValidation();
        if (DelayTimer.TimeMet)
        {
            if (_startFadeOut)
            {
                while (_currentColor.a > 0)
                {
                    alphaChange = Time.deltaTime / fadeTime;
                    _currentColor.a -= alphaChange;
                    _fadePanel.color = _currentColor;
                    return;
                }

                if (_currentColor.a > 0) return;
                fadeOutDoneEvent?.Invoke();
                _fadePanel.enabled = false;
                _startFadeOut = false;
            }

            if (_startFadeIn)
            {
                while (_currentColor.a < 1)
                {
                    alphaChange = Time.deltaTime / fadeTime;
                    _currentColor.a += alphaChange;
                    _fadePanel.color = _currentColor;
                    return;
                }

                if (_currentColor.a < 1) return;
                _startFadeIn = false;
                fadeInDoneEvent?.Invoke();
            }
        }
    }

    [ContextMenu("Start Fade Out")]
    public void StartFadeOut()
    {
        _currentColor.a = 1;
        _startFadeOut = true;
        beforeRunEvent?.Invoke();
    }

    [ContextMenu("Start Fade In")]
    public void StartFadeIn()
    {
        _fadePanel.enabled = true;
        _currentColor.a = 0;
        _startFadeIn = true;
        beforeRunEvent?.Invoke();
    }
}

public static class FadeBroker
{
    public static Action OnFadeIn;
    public static Action OnFadeOut;

    public static void TriggerFadeIn()
    {
        OnFadeIn?.Invoke();
    }

    public static void TriggerFadeOut()
    {
        OnFadeOut?.Invoke();
    }
}