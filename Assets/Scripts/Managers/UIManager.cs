using System;
using System.Collections;
using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public CanvasGroup HUD;
        public Image blackOverlay;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            FadeFromBlack(0.75f);
        }

        public Tween FadeToBlack(float duration = 0.2f)
        {
            return Tween.Alpha(blackOverlay, endValue: 1f, duration, useUnscaledTime: true);
        }

        public Tween FadeFromBlack(float duration = 0.2f, float delay = 0.2f)
        {
            return Tween.Alpha(blackOverlay, endValue: 0f, duration, startDelay: delay, useUnscaledTime: true);
        }
    }
}