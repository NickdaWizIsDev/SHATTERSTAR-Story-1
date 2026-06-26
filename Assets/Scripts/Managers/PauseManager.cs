using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using PrimeTween;
using UnityEngine.InputSystem;

namespace Managers
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private InputActionReference pauseAction;
        
        [Header("Pause References")]
        [SerializeField] private CanvasGroup mainPauseCanvas;
        [SerializeField] private Button resume;
        [SerializeField] private Button options;
        [SerializeField] private Button menu;

        [SerializeField] private CanvasGroup optionsMenu;
        [SerializeField] private Button backToPause;
        
        private enum State {PLAYING, PAUSED, OPTIONS}
        [SerializeField] private State currentState = State.PLAYING;
        
        [Header("Tween Settings")]
        [SerializeField] private float tweenDuration = 0.3f;
        [SerializeField] private Vector3 movementOffset = new Vector3(0, -30f, 0); // Slides the UI down slightly when hidden
        
        private void OnEnable()
        {
            resume?.onClick.AddListener(ResumeGame);
            options?.onClick.AddListener(OpenOptionsMenu);
            menu?.onClick.AddListener(OpenMainMenu);
            
            backToPause?.onClick.AddListener(CloseOptionsMenu);
            
            pauseAction.action.Enable();
            pauseAction.action.started += OnPause;
        }

        private void OnDisable()
        {
            resume?.onClick.RemoveListener(ResumeGame);
            options?.onClick.RemoveListener(OpenOptionsMenu);
            menu?.onClick.RemoveListener(OpenMainMenu);
            
            backToPause?.onClick.RemoveListener(CloseOptionsMenu);
            
            pauseAction.action.Disable();
            pauseAction.action.started -= OnPause;
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            switch (currentState)
            {
                case State.PLAYING:
                    PauseGame();
                    break;
                case State.PAUSED:
                    ResumeGame();
                    break;
                case State.OPTIONS:
                    CloseOptionsMenu();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Start()
        {
            // Initial setup: hide menus and apply the offset so they are ready to animate in
            mainPauseCanvas.alpha = 0;
            mainPauseCanvas.blocksRaycasts = false;
            mainPauseCanvas.transform.localPosition = movementOffset;

            optionsMenu.alpha = 0;
            optionsMenu.blocksRaycasts = false;
            optionsMenu.transform.localPosition = movementOffset;
        }

        public void PauseGame()
        {
            if(currentState == State.PAUSED) return;
            currentState = State.PAUSED;
            GameManager.Instance.timeScale = 0;
            GameManager.Instance.Player.DisableInput();
            
            mainPauseCanvas.blocksRaycasts = true;

            // Animate Alpha and Position 
            // CRITICAL: useUnscaledTime must be true because timeScale is 0!
            Tween.Alpha(mainPauseCanvas, endValue: 1f, duration: tweenDuration, useUnscaledTime: true);
            Tween.LocalPosition(mainPauseCanvas.transform, endValue: Vector3.zero, duration: tweenDuration, ease: Ease.OutBack, useUnscaledTime: true);
        }

        public void ResumeGame()
        {
            if(currentState == State.PLAYING) return;
            currentState = State.PLAYING;
            
            mainPauseCanvas.blocksRaycasts = false;

            Tween.Alpha(mainPauseCanvas, endValue: 0f, duration: tweenDuration, useUnscaledTime: true);
            Tween.LocalPosition(mainPauseCanvas.transform, endValue: movementOffset, duration: tweenDuration, ease: Ease.InBack, useUnscaledTime: true)
                .OnComplete(() => 
                {
                    // Only restore time and input AFTER the menu has fully animated out
                    GameManager.Instance.timeScale = 1;
                    GameManager.Instance.Player.EnableInput();
                });
        }

        public void OpenOptionsMenu()
        {
            if(currentState == State.OPTIONS) return;
            currentState = State.OPTIONS;
            
            mainPauseCanvas.blocksRaycasts = false;
            optionsMenu.blocksRaycasts = true;

            // Hide Main Pause Menu by sliding it in the opposite direction
            Tween.Alpha(mainPauseCanvas, endValue: 0f, duration: tweenDuration, useUnscaledTime: true);
            Tween.LocalPosition(mainPauseCanvas.transform, endValue: -movementOffset, duration: tweenDuration, ease: Ease.InBack, useUnscaledTime: true);

            // Show Options Menu
            optionsMenu.transform.localPosition = movementOffset; // Reset position before animating in
            Tween.Alpha(optionsMenu, endValue: 1f, duration: tweenDuration, useUnscaledTime: true);
            Tween.LocalPosition(optionsMenu.transform, endValue: Vector3.zero, duration: tweenDuration, ease: Ease.OutBack, useUnscaledTime: true);
        }

        public void CloseOptionsMenu()
        {
            if(currentState == State.PAUSED) return;
            currentState = State.PAUSED;
            
            optionsMenu.blocksRaycasts = false;
            mainPauseCanvas.blocksRaycasts = true;

            // Hide Options Menu
            Tween.Alpha(optionsMenu, endValue: 0f, duration: tweenDuration, useUnscaledTime: true);
            Tween.LocalPosition(optionsMenu.transform, endValue: movementOffset, duration: tweenDuration, ease: Ease.InBack, useUnscaledTime: true);

            // Show Main Pause Menu
            Tween.Alpha(mainPauseCanvas, endValue: 1f, duration: tweenDuration, useUnscaledTime: true);
            Tween.LocalPosition(mainPauseCanvas.transform, endValue: Vector3.zero, duration: tweenDuration, ease: Ease.OutBack, useUnscaledTime: true);
        }
        
        public void OpenMainMenu()
        {
            GameManager.Instance.timeScale = 1; 
            SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        }
    }
}