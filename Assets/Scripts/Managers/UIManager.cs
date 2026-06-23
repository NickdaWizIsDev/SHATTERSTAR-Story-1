using System;
using System.Collections;
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
        public PauseManager PauseManager;

        [SerializeField] private InputActionReference pauseAction;

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
            StartCoroutine(FadeFromBlack(0.35f));
            pauseAction.action.Enable();
            pauseAction.action.started += Pause;
        }

        public IEnumerator FadeToBlack(float duration = 0.2f)
        {
            float time = 0;
            var c = blackOverlay.color;
            while (time < duration)
            {
                time += Time.unscaledDeltaTime;
                c.a = Mathf.Lerp(0, 1, time / duration);
                blackOverlay.color = c;
                yield return null;
            }
        }

        public IEnumerator FadeFromBlack(float duration = 0.2f)
        {
            var time = 0f;
            var c = blackOverlay.color;
            while (time < duration)
            {
                time += Time.unscaledDeltaTime;
                c.a = Mathf.Lerp(1, 0, time / duration);
                blackOverlay.color = c;
                yield return null;
            }
        }

        private void Pause(InputAction.CallbackContext callbackContext)
        {
            PauseManager.PauseGame();
        }
    }
}